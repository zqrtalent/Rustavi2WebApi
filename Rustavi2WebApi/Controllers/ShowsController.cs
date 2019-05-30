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
    public class ShowsController : ControllerBase
    {
        private readonly IShowsService _showsService;

        public ShowsController(IShowsService showsService)
        {   
            _showsService = showsService;
        }

        // GET api/v1/shows
        /// <summary>
        /// Retrieves all shows information.
        /// </summary>
        [HttpGet()]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ShowItem>))]
        [ProducesAttribute("application/json")]
        public async Task<IActionResult> GetAsync()
        {
            var shows = await _showsService.GetShows();
            return Ok(shows);
        }

        // GET api/v1/shows/{showId}
        /// <summary>
        /// Retrieves show detail by show id.
        /// </summary>
        /// <param name="id">Show identifier</param>
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(ShowItemDetail))]
        [ProducesAttribute("application/json")]
        public async Task<IActionResult> GetDetail(string id)
        {
            var showDetail = await _showsService.GetShowDetail(id);
            return Ok(showDetail);
        }

        // GET api/shows/{id}/video/{videoId}
        /// <summary>
        /// Retrieves video detail of the show by id.
        /// </summary>
        /// <param name="id">Show identifier</param>
        /// <param name="videoId">Video identifier</param>
        [HttpGet("{id}/video/{videoId}")]
        [ProducesResponseType(200, Type = typeof(ItemVideoDetails))]
        [ProducesAttribute("application/json")]
        public async Task<IActionResult> GetShowVideoDetail(string id, string videoId)
        {
            var showVideoDetail = await _showsService.GetShowVideoDetail(id, videoId);
            return Ok(showVideoDetail);
        }
    }
}
