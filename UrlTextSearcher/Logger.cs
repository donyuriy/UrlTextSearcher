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
        private int _counter = 0;

        /// <summary>
        /// Returns formated string to write the message
        /// </summary>
        /// <param name="count">Count of identified matches</param>
        /// <param name="url">Current serched URL</param>
        /// <returns></returns>
        public void LogNumberOfMatches(int count, string url)
        {
            _counter++;
            string log;
            if (count > 0)
            {
                log = $"По адресу \"{url}\" найдено {count} совпадений.\n";
            }
            else
            {
                log = $"{url} отсканирован, совпадений не найдено.";
            }
            FormMain.AppendStaticTextBox(log);
        }

        public void LogNextSearchigLevel(int level)
        {
            string log = $"--------------\nПерход на следующий уровень ссылок: {level}\n--------------";
            FormMain.AppendStaticTextBox(log);
        }

        public void SearchAccomplished()
        {
            string log = $"Поиск окончен. Отсканировано {_counter} адресов.";
            FormMain.AppendStaticTextBox(log);
        }
    }
}
