using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TasteFinder.Models
{
    public class Restaurant
    {

        [Key]
        public string Email { get; set; }
        public string Name { get; set; }
		[Required(ErrorMessage = "Password is required")]
		[DataType(DataType.Password)]
		[MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
		public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public double TotalStars { get; set; }
        public double WeightedReviewScore { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime OpenTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CloseTime { get; set; }
        public bool OpenAir { get; set; }
        public bool KidsArea { get; set; }
        public bool Seats { get; set; }
        public bool Food { get; set; }
        public bool Drinks { get; set; }
        public bool Desserts { get; set; }
        public bool Delivery { get; set; }
        public bool Buffet { get; set; }
        public bool Vegan { get; set; }
        public bool Healthy { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
        public virtual ICollection<Keyword> Keywords { get; set; }
        
    }
}
