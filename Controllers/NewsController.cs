using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using rustavi2WebApi.Models.Services;
using rustavi2WebApi.Services;

namespace rustavi2WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {   
            _newsService = newsService;
        }

        // GET api/news/latest
        [HttpGet("latest")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<NewsItem>))]
        public async Task<IActionResult> GetLatestAsync()
        {
            var news = await _newsService.GetLatestNews();
            return CreatedAtAction(nameof(GetLatestAsync), news);
        }

        // GET api/news/5
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(NewsItemDetail))]
        public async Task<IActionResult> GetDetail(string id)
        {
            var newsDetail = await _newsService.GetNewsDetail(id);
            return CreatedAtAction(nameof(GetDetail), new { id = id }, newsDetail);
        }
    }
}
