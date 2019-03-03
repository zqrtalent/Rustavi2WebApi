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
    public class ShowsController : ControllerBase
    {
        private readonly IShowsService _showsService;

        public ShowsController(IShowsService showsService)
        {   
            _showsService = showsService;
        }

        // GET api/shows
        [HttpGet()]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ShowItem>))]
        public async Task<IActionResult> GetAsync()
        {
            var shows = await _showsService.GetShows();
            return CreatedAtAction(nameof(GetAsync), shows);
        }

        // GET api/shows/kirieri
        [HttpGet("{name}")]
        [ProducesResponseType(200, Type = typeof(ShowItemDetail))]
        public async Task<IActionResult> GetDetail(string name)
        {
            var showDetail = await _showsService.GetShowDetail(name);
            return CreatedAtAction(nameof(GetDetail), new { name = name }, showDetail);
        }
    }
}
