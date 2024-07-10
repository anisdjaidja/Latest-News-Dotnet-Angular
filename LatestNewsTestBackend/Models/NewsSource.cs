using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LatestNewsTestBackend.Models
{
    public class NewsSource
    {
        [Key][Required][DatabaseGenerated(DatabaseGeneratedOption.None)][MaxLength(255)]
        public string id { get; set; }

        [DataType(DataType.Text)][MaxLength(255)]
        public string name { get; set; }
    }
}
