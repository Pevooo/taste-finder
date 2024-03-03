using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TasteFinder.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }
        public int Stars { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Date { get; set; }
        public string Text { get; set; }
        public virtual User Author { get; set; }
        public virtual Restaurant Restaurant { get; set; }
        public virtual ICollection<Contribution> Contributions { get; set; }
    }
}
