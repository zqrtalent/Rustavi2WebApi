using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rustavi2WebApi.Services.Extensions
{
    using System.Net.Http;
    using System.Net.Http.Headers;

    public static class HttpClientExtensions
    {
        public static async Task<T> HttpGet<T>(this HttpClient httpClient, string url, Func<string, Task<T>> parseHtml) where T : class
        {
            T result = default(T);

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_13_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/72.0.3626.109 Safari/537.36");
            var response = httpClient.SendAsync(request).GetAwaiter().GetResult();

            var responseContent = await response.Content.ReadAsStringAsync();
            result = await parseHtml(responseContent);
            
            return result;
        }
    }
}
