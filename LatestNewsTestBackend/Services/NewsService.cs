using BenchmarkDotNet.Attributes;
using LatestNewsTestBackend.DataAcess;
using LatestNewsTestBackend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LatestNewsTestBackend.Services
{
    /// <summary>
    /// Has the concern of querying the database on behalf of the controller endpoints
    /// </summary>
    public class NewsService
    {
        private readonly NewsContext dbContext;
        public NewsService(IDbContextFactory<NewsContext> dbContextfactory)
        {
            dbContext = dbContextfactory.CreateDbContext();
        }
        #region QUERIES
        [Benchmark]
        public async Task<IEnumerable<NewsArticle>> GetAllNews()
        {
            /// The Include() operator is introduced in EF Core 8 with lazy load, 
            /// explicitly sets an attached entity to be loaded
            return await dbContext.Articles.Include(a => a.source).ToListAsync();
        }
        public async Task<List<NewsArticle>> GetAllNews(int startingfromID)
        {
            /// The Include() operator is introduced in EF Core 8 with lazy load, 
            /// explicitly sets an attached entity to be loaded
            return await dbContext.Articles.Where(x => x.id > startingfromID).Include(x => x.source).ToListAsync();
        }
        public async Task<List<NewsSource>> GetAllSources()
        {
            /// The Include() operator is introduced in EF Core 8 with lazy load, 
            /// explicitly sets an attached entity to be loaded
            return await dbContext.Sources.ToListAsync();
        }
        #endregion
    }
}
