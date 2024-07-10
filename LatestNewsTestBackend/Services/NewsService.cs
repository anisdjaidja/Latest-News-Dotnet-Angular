using LatestNewsTestBackend.DataAcess;
using LatestNewsTestBackend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace LatestNewsTestBackend.Services
{
    public class NewsService
    {
        private readonly NewsContext dbContext;
        public NewsService(IDbContextFactory<NewsContext> dbContextfactory)
        {
            dbContext = dbContextfactory.CreateDbContext();
        }
        #region QUERIES
        public async Task<List<NewsArticle>> GetAllNews()
        {
            return await dbContext.Articles.ToListAsync();
        }
        #endregion
    }
}
