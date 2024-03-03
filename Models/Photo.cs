using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TasteFinder.Models
{
    public class Photo
    {
        [Key]
        public int PhotoId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Date { get; set; }
        public byte[]? PhotoData { get; set; }
        public virtual Restaurant Owner { get; set; }
    }
}
