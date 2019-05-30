using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rustavi2WebApi.Models.Services;

namespace rustavi2WebApi.Services
{
    public interface INewsService
    {
        Task<IEnumerable<NewsItem>> GetLatestNews();
        Task<NewsItemDetail> GetNewsDetail(string id);
        Task<ItemVideoDetails> GetNewsVideoDetail(string id);
    }
}
