using System.ComponentModel.DataAnnotations;

namespace TasteFinder.Models
{
    public class Contribution
    {
        [Key]
        public int ContributionId { get; set; }
        public virtual Review Review { get; set; }
        public virtual User Author { get; set; }
    }
}
