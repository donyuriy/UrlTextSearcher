using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UrlTextSearcher.Interfaces;

namespace UrlTextSearcher
{
    public class Logger : ILogger
    {
        private int _countOfUrls = 0;
       
        /// <summary>
        /// Returns formated string to write the message
        /// </summary>
        /// <param name="count">Count of identified matches</param>
        /// <param name="url">Current serched URL</param>
        /// <returns></returns>
        public void LogNumberOfMatches(int count, string url)
        {
            Interlocked.Increment(ref _countOfUrls);
            string log;
            if (count > 0)
            {
                log = $" {url} найдено {count} совпадений.";
            }
            else
            {
                log = $"{url} совпадений не найдено.";
            }
            FormMain.AppendStaticTextBox(log);
        }
        public void LogScanningLevel(int levelNumber)
        {
            string log = $"\nПереход на следующий уровень URL: {levelNumber}.\n";
            FormMain.AppendStaticTextBox(log);
        }

        public void LogNextSearchigLevel(int numberOfUrls)
        {
            string log = $"\nНайдено {numberOfUrls} ссылок.\n";
            FormMain.AppendStaticTextBox(log);
        }

        public void SearchAccomplished()
        {
            string log = $"\nПоиск окончен. Отсканировано {_countOfUrls} адресов.";
            FormMain.AppendStaticTextBox(log);
        }
    }
}
