using Microsoft.AspNetCore.Mvc;
using HackerNewsAPI.Services;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace HackerNewsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoriesController : ControllerBase
    {
        private readonly IHackerNewsService _hackerNewsService;
        private readonly ILogger<StoriesController> _logger;

        public StoriesController(IHackerNewsService hackerNewsService, ILogger<StoriesController> logger)
        {
            _hackerNewsService = hackerNewsService;
            _logger = logger;
        }

        [HttpGet("best")]
        public async Task<ActionResult<IEnumerable<object>>> GetBestStories([FromQuery] int n = 10)
        {
            if (n <= 0 || n > 200)
            {
                return BadRequest("The value of n must be between 1 and 200");
                //return BadRequest("El valor de n debe estar entre 1 y 200");
            }

            try
            {
                var stories = await _hackerNewsService.GetBestStoriesAsync(n);
                return Ok(stories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving best stories with n={N}", n);
                //we can return multi-language
                return StatusCode(500, "Internal server error");
            }
        }
    }
}