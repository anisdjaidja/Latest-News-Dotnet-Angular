using LatestNewsTestBackend.Models;
using LatestNewsTestBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LatestNewsTestBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly NewsService _newsService;
        public NewsController(ILogger<NewsController> logger, NewsService newsService)
        {
            _logger = logger;
            _newsService = newsService;
        }


        // GET all Articles
        [HttpGet(template: "getall")]
        public IActionResult Get()
        {
            var news = _newsService.GetAllNews().Result;
            if(news.Count == 0)
            {
                _logger.LogWarning("GetAllArticles Endpoint Responded with 204NoContent");
                return NoContent();
            }
            _logger.LogInformation("GetAllArticles Endpoint Responded with 200OK");
            return Ok(news);
        }
        // GET all Articles
        [HttpGet(template: "getall/sources")]
        public IActionResult GetSources()
        {
            var sources = _newsService.GetAllSources().Result;
            if (sources.Count == 0)
            {
                _logger.LogWarning("GetAllArticles Endpoint Responded with 204NoContent");
                return NoContent();
            }
            _logger.LogInformation("GetAllArticles Endpoint Responded with 200OK");
            return Ok(sources);
        }
    }
}
