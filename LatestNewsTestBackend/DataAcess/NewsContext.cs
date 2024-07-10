using LatestNewsTestBackend.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace LatestNewsTestBackend.DataAcess
{
    public class NewsContext: DbContext
    {
        public NewsContext(DbContextOptions options) : base(options) { }

        public DbSet<NewsArticle> Articles { get; set; }
        public DbSet<NewsSource> Sources { get; set; }
    }
}
