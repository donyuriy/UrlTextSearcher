using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using UrlTextSearcher.Interfaces;

namespace UrlTextSearcher
{
    public class Searcher
    {
        private int _depthOfLinkToDisplay;
        private string _word;
        private string _url;
        private int _depthOfLinkCounter;
        private int queueCopyEnableFlag = 1;
        private ILogger _logger;

        public Searcher(
            ILogger logger,
            string word,
            string url,
            int depthOfLinkDisplay)
        {
            _word = word.ToLower();
            _url = url;
            _depthOfLinkToDisplay = depthOfLinkDisplay;
            Interlocked.Exchange(ref _depthOfLinkCounter, 0);
            _logger = logger;
        }

        public void FindAllReferencesDeep(string[] urlsList)
        {
            lock (this)
            {
                var _queueAfterUrlUpdate = new ConcurrentQueue<string>(urlsList);   //queue with URLs of current level to search all matches
                var _queueBeforeUrlUpdate = new ConcurrentQueue<string>(urlsList);
                var _queueToGetMatches = new ConcurrentQueue<string>(urlsList);

                _queueToGetMatches = LockedCopyUrlQueueToQueue(_queueAfterUrlUpdate);
                FindMatches(_queueToGetMatches);
                while (_depthOfLinkCounter < _depthOfLinkToDisplay)
                {
                    _queueAfterUrlUpdate = SearchInDepth(_queueBeforeUrlUpdate, _queueAfterUrlUpdate);
                    _queueToGetMatches = LockedCopyUrlQueueToQueue(_queueAfterUrlUpdate);
                    FindMatches(_queueToGetMatches);
                }
            }
        }

        private ConcurrentQueue<string> SearchInDepth(ConcurrentQueue<string> _queueBeforeUrlUpdate, ConcurrentQueue<string> _queueAfterUrlUpdate)
        {
            object locker = new object();
            lock (locker)
            {
                for (int i = 0; i < _queueBeforeUrlUpdate.Count; i++)
                {
                    _queueBeforeUrlUpdate.TryDequeue(out string currentUrl);
                    var htmlText = GetPageAsHtml(currentUrl);
                    _queueAfterUrlUpdate = new ConcurrentQueue<string>();

                    foreach (var item in FindAllUrlsInDocument(htmlText))
                    {
                        _queueAfterUrlUpdate.Enqueue(item);         //add all new urls to queue
                    }
                }
                Interlocked.Increment(ref _depthOfLinkCounter);
                return _queueAfterUrlUpdate = RemoveDuplicatedUrls(_queueAfterUrlUpdate);  //remove duplicated urls from queue
            }
        }

        /// <summary>
        /// Find count of matches in array of URLs. Returns result to ILogger object.
        /// </summary>
        /// <param name="_queueToGetMatches">ConcurrentQueue<string> with URLs</param>
        private void FindMatches(ConcurrentQueue<string> _queueToGetMatches)
        {
            int _countOfMatches = 0;
            if (!_queueToGetMatches.IsEmpty)
            {
                for (int i = 0; i < _queueToGetMatches.Count; i++)
                {
                    _queueToGetMatches.TryDequeue(out string url);
                    string text = GetPageAsText(url);
                    if (text.ToLower().IndexOf(_word.ToLower()) > 0)
                    {
                        int _startIndex = 0;
                        while ((_startIndex = text.ToLower().IndexOf(_word.ToLower(), _startIndex + _word.Length)) > 0)
                        {
                            _countOfMatches++;
                        }
                    }
                    _logger.LogNumberOfMatches(_countOfMatches, url);
                    _countOfMatches = 0;
                }
                Interlocked.Exchange(ref queueCopyEnableFlag, 1);   //enable to search on next URL level
            }
        }

        /// <summary>
        /// Remove all duplicated URLs form queue (ConcurrentQueue<string>)
        /// </summary>
        /// <param name="urlsQueue">Returns ConcurrentQueue<string> with no duplicated URLs on current level.</param>
        /// <returns></returns>
        private ConcurrentQueue<string> RemoveDuplicatedUrls(ConcurrentQueue<string> urlsQueue)
        {
            object locker = new object();
            lock (locker)
            {
                var urlsList = new List<string>(urlsQueue);
                for (int i = 0; i < urlsList.Count; i++)
                {
                    for (int j = i + 1; j < urlsList.Count; j++)
                    {
                        if (urlsList[i].Trim().Equals(urlsList[j].Trim()))
                        {
                            urlsList.RemoveAt(j);
                        }
                    }
                }
                return new ConcurrentQueue<string>(urlsList);
            }
        }

        /// <summary>
        /// Find all URLs in current document by Regex template.
        /// </summary>
        /// <param name="text">Recognizable text as string</param>
        /// <returns>Returns ConcurrentQueue<string> queue of URLs or null when Exception is occured</returns>
        private ConcurrentQueue<string> FindAllUrlsInDocument(string text)
        {
            try
            {
                Regex regex = new Regex(@"(http|ftp|https):\/\/([\w\-_]+(?:(?:\.[\w\-_]+)+))([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?");

                MatchCollection matches = regex.Matches(text);
                var referencesOnPage = new ConcurrentQueue<string>();
                for (int i = 0; i < matches.Count; i++)
                {
                    referencesOnPage.Enqueue(matches[i].Value);
                }
                return referencesOnPage;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Downloads page as text
        /// </summary>
        /// <param name="url">URL of current page</param>
        /// <returns>string url</returns>
        private string GetPageAsText(string url)
        {
            var currentPage = GetPageAsHtml(url);
            Regex htmlTagExpression = new Regex("<[^>]*>");
            currentPage = htmlTagExpression.Replace(currentPage, "");
            return currentPage;
        }

        /// <summary>
        /// URL downloader
        /// </summary>
        /// <param name="url">string URL</param>
        /// <returns></returns>
        private string GetPageAsHtml(string url)
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.Encoding = Encoding.UTF8;
                    return webClient.DownloadString(url);
                }

            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Enable queue copy by flag.
        /// </summary>
        /// <param name="urlsQueue">Copyable queue</param>
        /// <returns>Copied queue</returns>
        private ConcurrentQueue<string> LockedCopyUrlQueueToQueue(ConcurrentQueue<string> urlsQueue)
        {
            if (Convert.ToBoolean(queueCopyEnableFlag) && urlsQueue.Count > 0)
            {
                Interlocked.Exchange(ref queueCopyEnableFlag, 0);
                return new ConcurrentQueue<string>(urlsQueue);
            }
            return new ConcurrentQueue<string>();
        }
    }
}
