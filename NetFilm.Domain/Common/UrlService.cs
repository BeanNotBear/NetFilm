using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFilm.Domain.Common
{
    public static class UrlService
    {
        public static string CreateUrl(this string key)
        {
			string DISTRIBUTION_DOMAIN = "https://dqg1h1bamqrgk.cloudfront.net";
            string url = $"{DISTRIBUTION_DOMAIN}/{key}";
            return url;
		}
    }
}
