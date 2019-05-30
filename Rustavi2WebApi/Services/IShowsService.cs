using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rustavi2WebApi.Models.Services;

namespace rustavi2WebApi.Services
{
    public interface IShowsService
    {
        Task<IEnumerable<ShowItem>> GetShows();
        Task<ShowItemDetail> GetShowDetail(string name);
        Task<ItemVideoDetails> GetShowVideoDetail(string name, string videoId);
    }
}
