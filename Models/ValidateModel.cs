using System.ComponentModel.DataAnnotations;

namespace TasteFinder.Models
{
	public class ValidateModel
	{
		[Required(ErrorMessage = "name is required.")]
		public string name { get; set; }

		[Required]
		[EmailAddress(ErrorMessage = "Invalid email format.")]
		public string email { get; set; }


		[Required(ErrorMessage = "Old password is required.")]
		public string OldPassword { get; set; }

		[Required(ErrorMessage = "New password is required.")]
		[MinLength(8, ErrorMessage = "Password must be at least 8 characters long."),
			RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$", ErrorMessage = "Password must contain at least one letter and one number")]
		public string NewPassword { get; set; }

		[Required(ErrorMessage = "Confirm password is required.")]
		[Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
		public string ConfirmPassword { get; set; }

		[Required(ErrorMessage = "phone number is required.")]
		public string phonenumber { get; set; }

        [Required(ErrorMessage = "Profile Picture is required")]
		[RegularExpression(@"^image.*$", ErrorMessage = "File must be an image")]
        public string FileType { get; set; }

	}
}
