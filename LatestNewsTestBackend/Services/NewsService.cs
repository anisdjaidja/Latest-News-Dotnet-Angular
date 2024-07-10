using LatestNewsTestBackend.DataAcess;
using LatestNewsTestBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace LatestNewsTestBackend.Services
{
    public class NewsService
    {
        private readonly NewsContext dbContext;
        public NewsService(NewsContext newsContext)
        {
            dbContext = newsContext;
        }
        #region QUERIES
        public async Task<List<NewsArticle>> GetAllNews()
        {
            return await dbContext.Articles.ToListAsync();
        }
        #endregion
    }
}
