using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlTextSearcher.Interfaces;

namespace UrlTextSearcher
{
    public class Validator: IValidator
    {
        public bool ValidateUrl(string uri)
        {
            Uri uriResult;
            return Uri.TryCreate(uri, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        public bool ValidateSearchWord(string word)
        {
            if (String.IsNullOrEmpty(word) || String.IsNullOrWhiteSpace(word))
            {
                return false;
            }
            return true;
        }
    }
}
