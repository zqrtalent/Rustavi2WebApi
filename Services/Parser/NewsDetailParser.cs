using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rustavi2WebApi.Models.Services;

namespace rustavi2WebApi.Services.Parser
{
    using System.Globalization;
    using HtmlAgilityPack;

    internal class NewsDetailParser : IHtmlParser<NewsItemDetail>
    {
        public const string _newsCoverImagePath = @"http://rustavi2.ge$0";

        public async Task<NewsItemDetail> Parse(string htmlContent)
        {
            var result = new NewsItemDetail(); 
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);
            
            await Task.CompletedTask;

            var newsNode = doc.DocumentNode.SelectSingleNode(@"//div[@class='l']");
            if(newsNode != null)
            {
                result.Title = newsNode.SelectSingleNode(@".//div[@class='title']")?.InnerHtml ?? string.Empty;
                var time = newsNode.SelectSingleNode(@".//div[@itemprop='datePublished']")?.Attributes.SingleOrDefault(x => x.Name == "content")?.Value ?? string.Empty;

                DateTime newsDate;
                var fmtProvider = new CultureInfo("en-US");
                if(DateTime.TryParseExact(time.Replace('T', ' '), "yyyy-MM-dd HH:mm", fmtProvider, DateTimeStyles.None, out newsDate))
                {
                    result.Time = newsDate;
                }

                var phNode = newsNode.SelectSingleNode(@".//div[@class='ph']");
                var style = phNode?.Attributes?.SingleOrDefault(x => x.Name == "style")?.Value ?? string.Empty;
                if(style.StartsWith("background-image:url("))
                {
                    result.CoverImageUrl = _newsCoverImagePath.Replace("$0", style.Substring("background-image:url(".Length).TrimEnd(')'));
                }

                result.StoryDetail = newsNode.SelectSingleNode(@".//span[@itemprop='articleBody']")?.InnerHtml ?? string.Empty;
            }

            return result;
        }
    }
}
