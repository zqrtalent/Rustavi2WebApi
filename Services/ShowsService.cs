using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rustavi2WebApi.Models.Services;

namespace rustavi2WebApi.Services
{
    internal class ShowsService : IShowsService
    {
        private readonly IHtmlParser<IEnumerable<ShowItem>> _showsParser;
        private readonly string _urlShows = "http://rustavi2.ge/ka/shows";

        public ShowsService(IHtmlParser<IEnumerable<ShowItem>> showsParser)
        {
            _showsParser = showsParser;
        }

        public async Task<ShowItemDetail> GetShowDetail(string name)
        {
            await Task.CompletedTask;

            return new ShowItemDetail
            {
                 Name = "კურიერი",
                 Desc = "ყოველდღე 21:00",
                 PageUrl = "http://rustavi2.ge/ka/shows/kurieri",
                 MainVideo = new ShowVideoItem
                 {
                     Id = "",
                    Title = "კურიერი - 21:00 18 თებერვალი, 2019, ნაწილი 2",
                    VideoPageUrl = "",
                    CoverImageUrl = "",
                 },
                 VideoItemsBySection = new Dictionary<string, IEnumerable<ShowVideoItem>>
                 {
                     { string.Empty,  new ShowVideoItem[] 
                     {
                        new ShowVideoItem
                        {
                            Id = "",
                            Title = "კურიერი - 21:00 18 თებერვალი, 2019, ნაწილი 2",
                            VideoPageUrl = "",
                            CoverImageUrl = "",
                        }
                     } 
                     }
                 }
            };
        }

        public async Task<IEnumerable<ShowItem>> GetShows()
        {
            return await WebClientService.HttpGet(_urlShows, async (string html) => 
            {
                return await _showsParser.Parse(html);
            });
        }
    }
}
