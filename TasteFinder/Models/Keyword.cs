using System.ComponentModel.DataAnnotations;

namespace TasteFinder.Models
{
    public class Keyword
    {
        [Key]
        public string Text { get; set; }
        public virtual ICollection<KeywordPossession> Restaurants { get; set; }
    }
}
