using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rustavi2WebApi.Models.Services;

namespace rustavi2WebApi.Services.Parser
{
    using System.Globalization;
    using HtmlAgilityPack;
    using rustavi2WebApi.Services.Extensions;

    internal class ShowsParser : IHtmlParser<IEnumerable<ShowItem>>
    {
        public const string _coverImagePath = @"http://rustavi2.ge/$0"; // ShowCoverImageUrl
        public const string _showPageUrl = @"http://rustavi2.ge$0"; //ShowPageUrl

        public async Task<IEnumerable<ShowItem>> Parse(string htmlContent)
        {
            var result = new List<ShowItem>(); 
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);
            
            await Task.CompletedTask;

            var showsNode = doc.DocumentNode.SelectSingleNode(@"//div[@class='sh_line']");
            if(showsNode != null)
            {
                foreach(var childNode in showsNode.ChildNodes)
                {
                    if(childNode.HasClass("bl"))
                    {
                        /*
                        <div class="bl">
                            <a href="/ka/shows/kurieri"><div class="ph" style="background-image:url(../shows/3.jpg)"></div></a>
                            <a href="/ka/shows/kurieri" class="lnk link">კურიერი</a>
                            <div class="t">ყოველდღე 21:00</div>
                        </div>
                         */

                        var bkgndStyle = childNode.SelectSingleNode(@".//div[@class='ph']")?.Attributes.SingleOrDefault(a => a.Name == "style")?.Value ?? string.Empty;
                        int idxStart = bkgndStyle.IndexOf("url(../");
                        var coverImageUrl = idxStart != -1 ? bkgndStyle.Substring(idxStart + "url(../".Length, bkgndStyle.Length - idxStart - "url(../".Length - 1) : string.Empty;

                        var showUrl = childNode.SelectSingleNode(@".//a")?.Attributes?.SingleOrDefault(a => a.Name == "href")?.Value ?? string.Empty;
                        var showName = childNode.SelectSingleNode(@".//a[@class='lnk link']")?.InnerHtml;
                        var desc = childNode.SelectSingleNode(@".//div[@class='t']")?.InnerHtml;

                        result.Add(new ShowItem
                        {
                            Id = UrlExtensions.ExtractIdFromUrl(showUrl),
                            Name = showName,
                            Desc = desc,
                            PageUrl = _showPageUrl.Replace("$0", showUrl),
                            CoverImageUrl = string.IsNullOrEmpty(coverImageUrl) ? string.Empty : _coverImagePath.Replace("$0", coverImageUrl)
                        });
                    }
                }
            }

            return result;
        }
    }
}
