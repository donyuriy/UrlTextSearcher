using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UrlTextSearcher;
using UrlTextSearcher.Interfaces;

namespace UrlTextSearcher
{
    public class ThreadCreator
    {
        public static List<Thread> ThreadList { get; private set; }

        private int _numberOfThreads;
        private string _searchingWord;
        private string _searchingUrl;
        private int _depthOfLinkDisplay;
        private ILogger _logger;

        public ThreadCreator(
            ILogger logger,
            int numberOfThreads,
            int depthOfLinkDisplay,
            string searchingWord,
            string searchingUrl)
        {
            _numberOfThreads = numberOfThreads;
            _depthOfLinkDisplay = depthOfLinkDisplay;
            _searchingWord = searchingWord;
            _searchingUrl = searchingUrl;
            _logger = logger;
            ThreadList = new List<Thread>();
        }

        public void StartMultithreading()
        {
            Searcher searcher = new Searcher(_logger, _searchingWord, _searchingUrl, _depthOfLinkDisplay);
            for (int i = 0; i < _numberOfThreads; i++)
            {
                Thread t = new Thread(() => searcher.FindAllReferencesDeep(new string[] { _searchingUrl }));
                t.IsBackground = true;
                ThreadList.Add(t);
            }
            ThreadList.ForEach(item => item.Start());
        }
    }
}
