using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rustavi2WebApi.Models.Services;

namespace rustavi2WebApi.Services.Parser
{
    using HtmlAgilityPack;

    internal class ShowVideosParser : IHtmlParser<IEnumerable<ShowVideoItem>>
    {
        public const string _videoCoverImagePath = @"http://rustavi2.ge$0";

        public async Task<IEnumerable<ShowVideoItem>> Parse(string htmlContent)
        {
            var result = new List<ShowVideoItem>(); 
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);
            
            await Task.CompletedTask;

            var videoItemsNodes = doc.DocumentNode.SelectNodes(@".//div[@class='ireport_bl']");
            if(videoItemsNodes?.Any() ?? false)
            {
                /*
                <div class="ph" style="background-image:url(/shows_videos/40949.jpg)"><a href="/ka/video/40949?v=2"></a></div>
                <div class="title rioni"><a href="/ka/video/40949?v=2" class="link white">კურიერი - 21:00 12 მარტი, 2019, ნაწილი 2</a></div>
                 */
                foreach(var itemNode in videoItemsNodes)
                {
                    var title = itemNode.SelectSingleNode(".//a[@class='link white']")?.InnerHtml ?? string.Empty;
                    var coverImageUrl = itemNode.SelectSingleNode(".//div[@class='ph']")?.BackgroundImageUrl() ?? string.Empty;
                    var videoPageUrl = itemNode.SelectSingleNode(".//a[@class='link white']")?.HrefAttribute() ?? string.Empty;

                    var videoId = WebClientService.ExtractIdFromUrl(videoPageUrl);
                    result.Add(new ShowVideoItem
                    {
                        Id = videoId,
                        Title = title,
                        VideoPageUrl = !string.IsNullOrEmpty(coverImageUrl) ? _videoCoverImagePath.Replace("$0", videoPageUrl) : string.Empty,
                        CoverImageUrl = !string.IsNullOrEmpty(coverImageUrl) ? _videoCoverImagePath.Replace("$0", coverImageUrl) : string.Empty
                    });   
                }
            }

            return result;
        }
    }
}
