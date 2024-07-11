using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using LatestNewsTestBackend.Models;
using Newtonsoft.Json;
using LatestNewsTestBackend.DataAcess;

namespace LatestNewsTestBackend.Services
{
    public class NewsFetchService : IHostedService
    {
        private readonly ILogger<NewsFetchService> _logger;
        private readonly NewsContext _dbContext;
        private readonly string _apikey;
        private const string _apiurl = "https://newsapi.org/v2/top-headlines";

        public NewsFetchService(ILogger<NewsFetchService> logger, IDbContextFactory<NewsContext> dbContextfactory, IConfiguration config)
        {
            _logger = logger;
            _dbContext = dbContextfactory.CreateDbContext();
            _apikey = config.GetValue("ApiKey", " ").ToString();
        }

        /// <summary>
        /// IHostedService related method, starts with application and keeps running in 
        /// the background to handle periodic api calls and DB updates
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>Void</returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("News Fetcher Service Started in the Background");
            while (!cancellationToken.IsCancellationRequested)
            {
                // TODO: fetch and update
                /// Client configuration adhering to the api docs
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(_apiurl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("User-Agent", "C# App");

                var latestdate = await FindLatestStoredArticleDate();
                /// api call
                var newsresponse = await FetchNews(client, latestdate);
                if (newsresponse != null)
                {
                    /// There is an EF Core quirck, because the articles are being de-serialized from data, 
                    /// the attached SOURCE entity gets created regardless of being duplicate or not, 
                    /// when EF Core then is tracking entities during insertion, it falls into tracking two identical entities.
                    /// The Solution is to use linQ query to pass a single reference of same instance for all indentical SOURCEs
                    foreach (var item in newsresponse.articles)
                    {
                        if (_dbContext.Sources.Contains(item.source))
                            item.source = _dbContext.Sources.FirstOrDefault(s => s == item.source)!;
                    }
                    await _dbContext.Articles.AddRangeAsync(newsresponse.articles);
                    await _dbContext.SaveChangesAsync();

                    _logger.LogInformation($"News Fetcher Service Updated {newsresponse.totalResults} new articles");
                }
                /// This is a delay to not overload the NEWS API Servers, 
                /// this background worker will request new records every X hours, observe "FromHours(5)"
                await Task.Delay(((int)TimeSpan.FromHours(5).TotalMilliseconds), cancellationToken);
            }
        }

        /// <summary>
        /// Makes api call with or without a date query
        /// </summary>
        /// <param name="client"></param>
        /// <param name="latestdate">introduced in the request query to retrieve latest news 
        /// for pre-existing news DB, if null date querying will be ignored</param>
        /// <returns>News Fetch Response Model mapped to provided api schema</returns>
        private async Task<NewsFetchResponse?> FetchNews(HttpClient client, DateTime? latestdate = null)
        {
            HttpResponseMessage response;
            if (latestdate != null)
                response = await client.GetAsync($"?from={latestdate}&language=en&apiKey={_apikey}");
            else
                response = await client.GetAsync($"?language=en&apiKey={_apikey}");

            if (!response.IsSuccessStatusCode)
            {
                var mssg = await response.Content.ReadAsStringAsync();
                _logger.LogError("News Fetcher Service Failed to Call API - message : " + mssg);
                return null;
            }
            else
            {
                string dataset = await response.Content.ReadAsStringAsync();
                var body = JsonConvert.DeserializeObject<NewsFetchResponse>(dataset);
                foreach (var item in body.articles)
                {
                    if (item.source.id == null)
                    {
                        var newID = item.source.name.ToLower().Replace(' ', '-');
                        item.source.id = newID;
                    }

                }
                return body;
            }
        }
        private async Task<DateTime?> FindLatestStoredArticleDate()
        {
            if (_dbContext.Articles.Count() <= 0) { return null; }
            return await _dbContext.Articles.MaxAsync(x => x.publishedAt);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("News Fetcher Service Stopped");
        }


    }
}
