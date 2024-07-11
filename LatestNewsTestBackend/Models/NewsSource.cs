using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LatestNewsTestBackend.Models
{
    public class NewsSource
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [MaxLength(255)]
        public string? id { get; set; }

        [DataType(DataType.Text)][MaxLength(255)]
        public string? name { get; set; }
        [DataType(DataType.Text)]
        [MaxLength(255)]
        public string? description { get; set; }
        [DataType(DataType.Url)]
        public string? url { get; set; }
        [DataType(DataType.Text)]
        [MaxLength(255)]
        public string? category { get; set; }
        [DataType(DataType.Text)]
        [MaxLength(255)]
        public string? language { get; set; }
        [DataType(DataType.Text)]
        [MaxLength(255)]
        public string? country { get; set; }



    }
}
