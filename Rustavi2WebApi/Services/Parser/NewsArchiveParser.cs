using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rustavi2WebApi.Models.Services;

namespace rustavi2WebApi.Services.Parser
{
    using System.Globalization;
    using HtmlAgilityPack;

    internal class NewsArchiveParser : IHtmlParser<IEnumerable<NewsItem>>
    {
        public const string _newsCoverImagePath = @"http://rustavi2.ge/news_photos/$0_cover.jpg";

        public async Task<IEnumerable<NewsItem>> Parse(string htmlContent)
        {
            var result = new List<NewsItem>(); 
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);
            
            await Task.CompletedTask;

            var newsNode = doc.DocumentNode.SelectSingleNode(@"//div[@class='nw_cont']");
            if(newsNode != null)
            {
                var fmtProvider = new CultureInfo("en-US");
                var datePart = string.Empty;
                foreach(var childNode in newsNode.ChildNodes)
                {
                    if(childNode.HasClass("date_l"))
                    {
                        datePart = childNode.InnerHtml;
                    }
                    else
                    if(childNode.HasClass("nw_line") && childNode.FirstChild != null)
                    {
                        /*
                        <div class="nw_line">
                        <div><span class="dt">22:29</span> <a href="/ka/news/126563" class="link">„თუკი კლანი არ მოშორდა მმართველ რგოლს, კანონზე ლაპარაკს აზრი არ გააჩნია" - მშვენიერაძე  <img src="/img/video.gif"></a></div>
                        </div>
                         */

                         var time = childNode.SelectSingleNode(@".//span[@class='dt']")?.InnerHtml ?? string.Empty;
                         var linkElem = childNode.SelectSingleNode(@".//a[@class='link']");
                         var newsUrl = linkElem?.Attributes.SingleOrDefault(a => a.Name == "href")?.Value ?? string.Empty;
                         var title = linkElem?.InnerHtml ?? string.Empty;

                         if(!string.IsNullOrEmpty(title))
                         {
                             int idx = title.IndexOf("<img");
                             if(idx != -1)
                                title = title.Substring(0, idx);
                         }

                         DateTime newsDate;
                         DateTime.TryParseExact($"{datePart} {time}", "dd-MM-yyyy HH:mm", fmtProvider, DateTimeStyles.None, out newsDate);

                        var id = newsUrl.Substring(newsUrl.LastIndexOf('/') + 1);
                        result.Add(new NewsItem
                        {
                            Id = id,
                            Time = newsDate,
                            Title = title,
                            CoverImageUrl = _newsCoverImagePath.Replace("$0", id)
                        });
                    }
                }
            }

            return result;
        }
    }
}
