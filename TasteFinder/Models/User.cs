using System.ComponentModel.DataAnnotations;

namespace TasteFinder.Models
{
    public class User
    {
        [Key]
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public int Contribution { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public byte[] ProfilePicture { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
