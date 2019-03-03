using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rustavi2WebApi.Models.Services;

namespace rustavi2WebApi.Services
{
    public class NewsService : INewsService
    {
        private readonly IHtmlParser<IEnumerable<NewsItem>> _newsParser;
        private readonly IHtmlParser<NewsItemDetail> _newsDetailParser;
        private readonly string _urlNewsArchive = "http://rustavi2.ge/ka/news/page-";
        private readonly string _urlNewsDetail = "http://rustavi2.ge/ka/news/";

        public NewsService(IHtmlParser<IEnumerable<NewsItem>> newsParser, IHtmlParser<NewsItemDetail> newsDetailParser)
        {
            _newsParser = newsParser;
            _newsDetailParser = newsDetailParser;
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
    }
}
