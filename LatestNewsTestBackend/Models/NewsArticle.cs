using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LatestNewsTestBackend.Models
{
    public class NewsArticle
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [DataType(DataType.Text)][MaxLength(255)]
        public string? title { get; set; }

        [DataType(DataType.Text)][MaxLength(255)]
        public string? author { get; set; }

        [DataType(DataType.Text)]
        public string? description { get; set; }


        [DataType(DataType.Url)]
        public string? url { get; set; }
        [DataType(DataType.ImageUrl)]
        public string? urlToImage { get; set; }


        [DataType(DataType.DateTime)]
        public DateTime? publishedAt { get; set; }

        [DataType(DataType.Text)]
        public string? content { get; set; }

        public virtual NewsSource source { get; set; }
        [MaxLength(255)]
        [ForeignKey(nameof(source))]
        public string? sourceId { get; set; }
    }

    public class NewsResponse
    {
        public int latestID { get; set; }
        public NewsArticle[] articles { get; set; }
    }
}
