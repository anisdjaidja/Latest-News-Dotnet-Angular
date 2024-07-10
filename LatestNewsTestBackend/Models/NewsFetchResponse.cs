namespace LatestNewsTestBackend.Models
{
    public class NewsFetchResponse
    {
        public string status;
        public int totalResults;
        public NewsArticle[] articles;
    }
}
