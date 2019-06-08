using System;

namespace rustavi2WebApi.Services.Extensions
{
    public static class UrlExtensions
    {
        public static string ExtractIdFromUrl(string url)
        {
            if(!string.IsNullOrEmpty(url))
            {
                var startIdx = url.LastIndexOf('/');
                var endIdx = url.LastIndexOf('?');

                if(startIdx != -1)
                {
                    if(endIdx != -1)
                        return url.Substring(startIdx+1, endIdx - startIdx - 1);
                    else
                        return url.Substring(startIdx+1);
                }
            }

            return string.Empty;
        }
    }
}
