using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using LatestNewsTestBackend.Models;
using Newtonsoft.Json;
using LatestNewsTestBackend.DataAcess;
using Microsoft.EntityFrameworkCore.Internal;

namespace LatestNewsTestBackend.Services
{
    public class NewsFetchService : IHostedService
    {
        private readonly ILogger<NewsFetchService> _logger;
        private readonly NewsContext _dbContext;
        private readonly string _apikey;
        private const string _apiurl = @"https://newsapi.org/v2/everything";
        public NewsFetchService(ILogger<NewsFetchService> logger, IDbContextFactory<NewsContext> dbContextfactory, IConfiguration config)
        {
            _logger = logger;
            _dbContext = dbContextfactory.CreateDbContext();
            _apikey = config.GetValue("ApiKey", " ").ToString();
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("News Fetcher Service Started in the Background");
            while (!cancellationToken.IsCancellationRequested)
            {
                // TODO: fetch and update
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(_apiurl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("User-Agent", "C# App");
                HttpResponseMessage response = await client.GetAsync($"?q=e&apiKey={_apikey}");
                
                if (!response.IsSuccessStatusCode)
                {
                    var mssg = await response.Content.ReadAsStringAsync();
                    _logger.LogError("News Fetcher Service Failed to Call API - message : " + mssg);
                }
                else
                {
                    string dataset = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrWhiteSpace(dataset))
                    {
                        _logger.LogInformation("News Fetcher Service Found no new articles");
                    }
                    else
                    {
                        var body = JsonConvert.DeserializeObject<NewsFetchResponse>(dataset);

                        await _dbContext.AddRangeAsync(body.articles);
                        await _dbContext.SaveChangesAsync();
                        _logger.LogInformation("News Fetcher Service Consumed NEWS API, DB Updated");
                    }
                }
                await Task.Delay(((int)TimeSpan.FromHours(5).TotalMilliseconds), cancellationToken);
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("News Fetcher Service Stopped");
        }

        private async Task<DateTime?> FindLatestStoredArticleDate()
        {
            return await _dbContext.Articles.MaxAsync(x => x.publishedAt);
        }
    }
}
