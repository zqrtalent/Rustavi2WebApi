using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rustavi2WebApi.Models.Services;

namespace rustavi2WebApi.Services
{
    using System.Net.Http;
    using Microsoft.Extensions.Options;
    using rustavi2WebApi.Extensions;
    using rustavi2WebApi.Services.Extensions;
    using rustavi2WebApi.Settings;

    internal class NewsService : INewsService
    {
        private readonly IHtmlParser<IEnumerable<NewsItem>> _newsParser;
        private readonly IHtmlParser<NewsItemDetail> _newsDetailParser;
        private readonly IHtmlParser<string> _iframeSrcParser;
        private readonly IHtmlParser<ItemVideoDetails> _itemVideoParser;

        private readonly string _urlNewsArchive = "http://rustavi2.ge/ka/news/page-";
        private readonly string _urlNewsDetail = "http://rustavi2.ge/ka/news/";

        private readonly IOptionsMonitor<UrlReplaceSettings> _urlReplaceSettings;
        private readonly HttpClient _httpClient;
        public NewsService( HttpClient httpClient, 
                            IHtmlParser<IEnumerable<NewsItem>> newsParser, 
                            IHtmlParser<NewsItemDetail> newsDetailParser,
                            IHtmlParser<string> iframeSrcParser, 
                            IHtmlParser<ItemVideoDetails> itemVideoParser, 
                            IOptionsMonitor<UrlReplaceSettings> urlReplaceSettings)
        {
            _httpClient = httpClient;
            _newsParser = newsParser;
            _newsDetailParser = newsDetailParser;
            _iframeSrcParser = iframeSrcParser;
            _itemVideoParser = itemVideoParser;
            _urlReplaceSettings = urlReplaceSettings;
        }

        public async Task<IEnumerable<NewsItem>> GetLatestNews()
        {
            var newsItems = await _httpClient.HttpGet(_urlNewsArchive + "1", async (string html) => 
            {
                return await _newsParser.Parse(html);
            });

            return newsItems?.Select(x => {
               x.CoverImageUrl = _urlReplaceSettings.CurrentValue.ImageUrlHostReplace.ReplaceAllTheOccurences(x.CoverImageUrl);
               return x; 
            })?.ToList();
        }

        public async Task<NewsItemDetail> GetNewsDetail(string id)
        {
            var newsDetail = await _httpClient.HttpGet(_urlNewsDetail + id, async (string html) => 
            {
                return await _newsDetailParser.Parse(html);
            });

            if(newsDetail != null)
            {
                newsDetail.Id = id;
                newsDetail.CoverImageUrl = _urlReplaceSettings.CurrentValue.ImageUrlHostReplace.ReplaceAllTheOccurences(newsDetail.CoverImageUrl);
                newsDetail.VideoUrl = _urlReplaceSettings.CurrentValue.HlsUrlHostReplace.ReplaceAllTheOccurences(newsDetail.VideoUrl);
            }
            return newsDetail;
        }

        public async Task<ItemVideoDetails> GetNewsVideoDetail(string id)
        {
            var iframeSrcUrl = await _httpClient.HttpGet(_urlNewsDetail + id, async (string html) => 
            {
                return await _iframeSrcParser.Parse(html);
            });

            if(string.IsNullOrEmpty(iframeSrcUrl))
            {
                return null;
            }

            var videoDetails = await _httpClient.HttpGet(iframeSrcUrl, async (string html) => 
            {
                return await _itemVideoParser.Parse(html);
            });

            if(videoDetails != null)
            {
                videoDetails.Id = id;
                videoDetails.VideoType = ItemVideoType.NewsVideo;
                videoDetails.VideoUrl = _urlReplaceSettings.CurrentValue.HlsUrlHostReplace.ReplaceAllTheOccurences(videoDetails.VideoUrl);
            }

            return videoDetails;
        }
    }
}
