using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rustavi2WebApi.Models.Services;

namespace rustavi2WebApi.Services
{
    internal class NewsService : INewsService
    {
        private readonly IHtmlParser<IEnumerable<NewsItem>> _newsParser;
        private readonly IHtmlParser<NewsItemDetail> _newsDetailParser;
        private readonly IHtmlParser<string> _iframeSrcParser;
        private readonly IHtmlParser<ItemVideoDetails> _itemVideoParser;

        private readonly string _urlNewsArchive = "http://rustavi2.ge/ka/news/page-";
        private readonly string _urlNewsDetail = "http://rustavi2.ge/ka/news/";

        public NewsService(IHtmlParser<IEnumerable<NewsItem>> newsParser, IHtmlParser<NewsItemDetail> newsDetailParser,
                            IHtmlParser<string> iframeSrcParser, IHtmlParser<ItemVideoDetails> itemVideoParser)
        {
            _newsParser = newsParser;
            _newsDetailParser = newsDetailParser;
            _iframeSrcParser = iframeSrcParser;
            _itemVideoParser = itemVideoParser;
        }

        public async Task<IEnumerable<NewsItem>> GetLatestNews()
        {
            return await WebClientService.HttpGet(_urlNewsArchive + "1", async (string html) => 
            {
                return await _newsParser.Parse(html);
            });
        }

        public async Task<NewsItemDetail> GetNewsDetail(string id)
        {
            var newsDetail = await WebClientService.HttpGet(_urlNewsDetail + id, async (string html) => 
            {
                return await _newsDetailParser.Parse(html);
            });

            if(newsDetail != null)
            {
                newsDetail.Id = id;
            }
            return newsDetail;
        }

        public async Task<ItemVideoDetails> GetNewsVideoDetail(string id)
        {
            var iframeSrcUrl = await WebClientService.HttpGet(_urlNewsDetail + id, async (string html) => 
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
                videoDetails.Id = id;
                videoDetails.VideoType = ItemVideoType.NewsVideo;
            }

            return videoDetails;
        }
    }
}
