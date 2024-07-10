using LatestNewsTestBackend.Models;
using LatestNewsTestBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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


        // GET all
        [HttpGet(template: "GetAll")]
        public IEnumerable<NewsArticle>? Get()
        {
            return _newsService.GetAllNews().Result;
        }
    }
}
