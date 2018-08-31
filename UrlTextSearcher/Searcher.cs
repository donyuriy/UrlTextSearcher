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
        private int _counter;
        private int _depthOfLinkDisplay;
        private int _depthOfLinkCounter;
        private int _allReferencesStartIndex;
        private int queueCopyEnableFlag = 1;
        private IValidator _validator;
        private ILogger _logger;
        private List<string> _allReferences;
        private ConcurrentQueue<string> _queue;

        public Searcher(string word, string url, int depthOfLinkDisplay)
        {
            _word = word.ToLower();
            _url = url;
            _depthOfLinkDisplay = depthOfLinkDisplay;
            _depthOfLinkCounter = 0;
            _allReferencesStartIndex = 0;
            _logger = new Logger();
            _validator = new Validator();
            _allReferences = new List<string>();
            _queue = new ConcurrentQueue<string>();
            _allReferences.Add(_url);
        }

        public void FindAllReferencesDeep(List<string> urls)
        {
            var temporaryUrlList = ExtentionClass.Clone(urls);
            while (_depthOfLinkCounter < _depthOfLinkDisplay)
            {
                FindDeep(temporaryUrlList);
                LockedCopyUrlListToQueue();
                FindMatches();
            }

            void FindDeep(List<string> urlsList)
            {
                _depthOfLinkCounter++;
                for (int i = _allReferencesStartIndex; i < urls.Count; i++)
                {
                    var htmlText = GetPageAsHtml(temporaryUrlList[i]);
                    if (string.IsNullOrEmpty(htmlText))
                    {
                        continue;
                    }
                    temporaryUrlList.AddRange(FindAllUrlsInDocument(htmlText));
                }
                RemoveDuplicatedUrls();         // TODO: метод удаления дублирующих ссылок ДОДЕЛАТЬ
                _allReferencesStartIndex = urls.Count;
                urls = ExtentionClass.Clone(temporaryUrlList);
            }

            _allReferences = temporaryUrlList;
        }

        private void RemoveDuplicatedUrls()
        {
            for (int i = 0; i < _allReferences.Count; i++)
            {
                for (int j = i + 1; j < _allReferences.Count; j++)
                {
                    if (_allReferences[i].Trim().Equals(_allReferences[j].Trim()))
                    {
                        _allReferences.RemoveAt(j);
                    }
                }
            }

        }

        private void FindMatches()
        {
            if (!_queue.IsEmpty)
            {
                for (int i = 0; i < _queue.Count; i++)
                {
                    string url = string.Empty;
                    _queue.TryDequeue(out url);
                    string text = GetPageAsText(url);
                    _counter = 0;
                    if (text.ToLower().IndexOf(_word.ToLower()) > 0)
                    {
                        int _startIndex = 0;
                        while ((_startIndex = text.ToLower().IndexOf(_word.ToLower(), _startIndex + _word.Length)) > 0)
                        {
                            _counter++;
                        }
                    }
                    _logger.LogNumberOfMatches(_counter, url);
                }
                Interlocked.Exchange(ref queueCopyEnableFlag, 1);
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
            var currentPage = GetPageAsHtml(url);
            Regex htmlTagExpression = new Regex("<[^>]*>");
            currentPage = htmlTagExpression.Replace(currentPage, "");
            return currentPage;
        }
        private string GetPageAsHtml(string url)
        {
            try
            {
                lock (this)
                {
                    using (WebClient webClient = new WebClient())
                    {
                        webClient.Encoding = Encoding.UTF8;
                        return webClient.DownloadString(url);
                    }
                }
            }
            catch
            {
                return string.Empty;
            }
        }
        private void LockedCopyUrlListToQueue()
        {
            if (Convert.ToBoolean(queueCopyEnableFlag))
            {               
                _queue = new ConcurrentQueue<string>(_allReferences);
                Interlocked.Exchange(ref queueCopyEnableFlag, 0);
            }
            else
            {
                Thread.Sleep(2000);
                LockedCopyUrlListToQueue();
            }
        }
    }
}
