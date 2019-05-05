using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rustavi2WebApi.Models.Services;

namespace rustavi2WebApi.Services
{
    internal class ShowsService : IShowsService
    {
        private readonly string _urlShows = "http://rustavi2.ge/ka/shows";
        private readonly IHtmlParser<IEnumerable<ShowItem>> _showsParser;
        private readonly IHtmlParser<ShowItemDetail> _showDetailParser;

        public ShowsService(IHtmlParser<IEnumerable<ShowItem>> showsParser, IHtmlParser<ShowItemDetail> showDetailParser)
        {
            _showsParser = showsParser;
            _showDetailParser = showDetailParser;
        }

        public async Task<IEnumerable<ShowItem>> GetShows()
        {
            return await WebClientService.HttpGet(_urlShows, async (string html) => 
            {
                return await _showsParser.Parse(html);
            });
        }

        public async Task<ShowItemDetail> GetShowDetail(string id)
        {
            var result = await WebClientService.HttpGet($"{_urlShows}/{id}", async (string html) => 
            {
                return await _showDetailParser.Parse(html);
            });

            if(result != null)
            {
                result.Id = id;
            }

            return result;
        }
    }
}
