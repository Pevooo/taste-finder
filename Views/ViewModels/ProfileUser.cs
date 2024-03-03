using TasteFinder.Models;

namespace TasteFinder.ViewModels
{
    public class ProfileUser
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public byte[] ProfilePicture { get; set; }
        public int Trust {  get; set; }
        public string photo {  get; set; }
        public IEnumerable<Review>reviews { get; set; }
       
      
    }
}
