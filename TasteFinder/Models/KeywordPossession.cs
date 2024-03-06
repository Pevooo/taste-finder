using System.ComponentModel.DataAnnotations;

namespace TasteFinder.Models
{
    public class KeywordPossession
    {
        [Key]
        public int PossessionId { get; set; }
        public virtual Restaurant Restaurant { get; set; }
        public virtual Keyword Key { get; set; }
    }
}
