using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace rustavi2WebApi.Controllers
{
    using rustavi2WebApi.Models.Services;
    using rustavi2WebApi.Services;

    [Route("api/v1/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {   
            _newsService = newsService;
        }

        // GET api/news/latest
        /// <summary>
        /// Retrievs latest news items.
        /// </summary>
        [HttpGet("latest")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<NewsItem>))]
        [ProducesAttribute("application/json")]
        public async Task<IActionResult> GetLatestAsync()
        {
            var news = await _newsService.GetLatestNews();
            return CreatedAtAction(nameof(GetLatestAsync), news);
        }

        // GET api/news/{id}
        /// <summary>
        /// Retrieves news detail by id.
        /// </summary>
        /// <param name="id">News identifier</param>     
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(NewsItemDetail))]
        [ProducesAttribute("application/json")]
        public async Task<IActionResult> GetDetail(string id)
        {
            var newsDetail = await _newsService.GetNewsDetail(id);
            return CreatedAtAction(nameof(GetDetail), new { id = id }, newsDetail);
        }
    }
}
