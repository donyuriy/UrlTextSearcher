using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlTextSearcher.Interfaces
{
    public interface ILogger
    {
        void LogNumberOfMatches(int count, string url);
        void LogNextSearchigLevel(int level);
        void SearchAccomplished();
        void LogScanningLevel(int levelNumber);
    }
}
