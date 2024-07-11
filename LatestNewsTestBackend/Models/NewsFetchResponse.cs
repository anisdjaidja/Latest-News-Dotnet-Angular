namespace LatestNewsTestBackend.Models
{
    public class NewsFetchResponse
    {
        public string status { get; set; }
        public int totalResults { get; set; }
        public NewsArticle[] articles { get; set; }
    }
    public class SourcesFetchResponse
    {
        public string status { get; set; }
        public NewsSource[] sources { get; set; }
    }
}
