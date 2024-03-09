using System.ComponentModel.DataAnnotations;

namespace TasteFinder.Models
{
    public class Keyword
    {
        [Key]
        public int PossessionId { get; set; }
        public string Text { get; set; }
        public virtual Restaurant Restaurant { get; set; }
        
    }
}
