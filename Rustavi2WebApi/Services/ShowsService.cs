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
        private readonly string _urlVideoPage = "http://rustavi2.ge/ka/video/$0?v=2";
        private readonly IHtmlParser<IEnumerable<ShowItem>> _showsParser;
        private readonly IHtmlParser<ShowItemDetail> _showDetailParser;
        private readonly IHtmlParser<string> _iframeSrcParser;
        private readonly IHtmlParser<ItemVideoDetails> _itemVideoParser;

        public ShowsService(
            IHtmlParser<IEnumerable<ShowItem>> showsParser, 
            IHtmlParser<ShowItemDetail> showDetailParser,
            IHtmlParser<string> iframeSrcParser, 
            IHtmlParser<ItemVideoDetails> itemVideoParser)
        {
            _showsParser = showsParser;
            _showDetailParser = showDetailParser;
            _iframeSrcParser = iframeSrcParser;
            _itemVideoParser = itemVideoParser;
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

        public async Task<ItemVideoDetails> GetShowVideoDetail(string name, string videoId)
        {
            var iframeSrcUrl = await WebClientService.HttpGet(_urlVideoPage.Replace("$0", videoId), async (string html) => 
            {
                return await _iframeSrcParser.Parse(html);
            });

            if(string.IsNullOrEmpty(iframeSrcUrl))
            {
                return null;
            }

            var videoDetails = await WebClientService.HttpGet(iframeSrcUrl, async (string html) => 
            {
                return await _itemVideoParser.Parse(html);
            });
            
            if(videoDetails != null)
            {
                videoDetails.Id = videoId;
                videoDetails.VideoType = ItemVideoType.NewsVideo;
            }

            return videoDetails;
        }
    }
}
