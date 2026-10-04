using GoogleSearchApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace GoogleSearchApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly SearchService _searchService;

        public SearchController(SearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet]
        public async Task<IActionResult> Search(string query)
        {
            var results = await _searchService.SearchAsync(query);

            return Ok(results);
        }
    }
}