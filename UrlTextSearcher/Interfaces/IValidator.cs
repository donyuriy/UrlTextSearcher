using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlTextSearcher.Interfaces
{
    interface IValidator
    {
        bool ValidateUrl(string uri);
        bool ValidateSearchWord(string word);
    }
}
