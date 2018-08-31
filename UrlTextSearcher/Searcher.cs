using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using UrlTextSearcher.Interfaces;

namespace UrlTextSearcher
{
    public class Searcher
    {
        private string _word;
        private string _url;
        private int _depthOfLinkDisplay;
        private int _depthOfLinkCounter;
        private int queueCopyEnableFlag = 1;
        private IValidator _validator;
        private ILogger _logger;
        private ConcurrentQueue<string> _queueToGetMatches;
        private ConcurrentQueue<string> _queueBeforeUrlUpdate;
        private ConcurrentQueue<string> _queueAfterUrlUpdate;

        public Searcher(ILogger logger,
            string word,
            string url,
            int depthOfLinkDisplay)
        {
            _word = word.ToLower();
            _url = url;
            _depthOfLinkDisplay = depthOfLinkDisplay;
            _depthOfLinkCounter = 0;
            _logger = logger;
            _validator = new Validator();
            _queueToGetMatches = new ConcurrentQueue<string>();
        }

        public void FindAllReferencesDeep(List<string> urlsList)
        {           
            _queueAfterUrlUpdate = new ConcurrentQueue<string>(urlsList);

            object lockedObject = new object();
            lock (lockedObject)
            {
                while (_depthOfLinkCounter < _depthOfLinkDisplay)
                {
                    Interlocked.Increment(ref _depthOfLinkCounter);
                    LockedCopyUrlListToQueue(_queueAfterUrlUpdate);
                    FindMatches();
                    _queueBeforeUrlUpdate = new ConcurrentQueue<string>(_queueAfterUrlUpdate);
                    FindDeep(_queueBeforeUrlUpdate);
                }
            }


            void FindDeep(ConcurrentQueue<string> urlsQueue)
            {
                for (int i = 0; i < urlsQueue.Count; i++)
                {
                    urlsQueue.TryDequeue(out string currentUrl);
                    var htmlText = GetPageAsHtml(currentUrl);
                    lock (_queueAfterUrlUpdate)
                    {
                        foreach (var item in FindAllUrlsInDocument(htmlText))
                        {
                            _queueAfterUrlUpdate.Enqueue(item);         //add all new urls to queue
                        }
                    }
                }
                _queueAfterUrlUpdate = RemoveDuplicatedUrls(_queueAfterUrlUpdate);  //remove duplicated urls from queue
                _logger.LogNextSearchigLevel(_queueAfterUrlUpdate.Count);   //log count for found URLs
            }
        }

        private void FindMatches()
        {
            object lockedObject = new object();
            lock (lockedObject)
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
                    Interlocked.Exchange(ref queueCopyEnableFlag, 1);
                }
            }
        }

        private ConcurrentQueue<string> RemoveDuplicatedUrls(ConcurrentQueue<string> urlsQueue)
        {
            lock (this)
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

        private List<string> FindAllUrlsInDocument(string text)
        {
            try
            {
                Regex regex = new Regex(@"(http|ftp|https):\/\/([\w\-_]+(?:(?:\.[\w\-_]+)+))([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?");

                MatchCollection matches = regex.Matches(text);
                List<string> referencesOnPage = new List<string>();
                for (int i = 0; i < matches.Count; i++)
                {
                    referencesOnPage.Add(matches[i].Value);
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
        /// <returns></returns>
        private string GetPageAsText(string url)
        {
            object lockedObject = new object();
            lock (lockedObject)
            {
                var currentPage = GetPageAsHtml(url);
                Regex htmlTagExpression = new Regex("<[^>]*>");
                currentPage = htmlTagExpression.Replace(currentPage, "");
                return currentPage;
            }

        }
        private string GetPageAsHtml(string url)
        {
            lock (this)
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
        }
        private void LockedCopyUrlListToQueue(ConcurrentQueue<string> urlsQueue)
        {
            object lockedObject = new object();
            lock (lockedObject)
            {
                if (Convert.ToBoolean(queueCopyEnableFlag) && urlsQueue.Count > 0)
                {
                    Interlocked.Exchange(ref queueCopyEnableFlag, 0);
                    _queueToGetMatches = new ConcurrentQueue<string>(urlsQueue);                    
                }
            }
        }
    }
}
