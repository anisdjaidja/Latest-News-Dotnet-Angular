
namespace LatestNewsTestBackend.Services
{
    public class NewsFetchService : IHostedService
    {
        private readonly ILogger<NewsFetchService> _logger;
        public NewsFetchService(ILogger<NewsFetchService> logger)
        {
            _logger = logger;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("News Fetcher Service Started in the Background");
            while (!cancellationToken.IsCancellationRequested)
            {
                // TODO: fetch and update
                _logger.LogInformation("News Fetcher Service Consumed NEWS API, DB Updated");
                await Task.Delay(((int)TimeSpan.FromHours(5).TotalMilliseconds), cancellationToken);
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("News Fetcher Service Stopped");
        }
    }
}
