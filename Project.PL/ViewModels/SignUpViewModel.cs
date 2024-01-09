using System.ComponentModel.DataAnnotations;

namespace Project.PL.ViewModels
{
	public class SignUpViewModel
	{
		[Required(ErrorMessage = "Username is Required")]
        public string UserName { get; set; }

		[Required(ErrorMessage = "Firstname is Required")]
		public string FName { get; set; }

		[Required(ErrorMessage = "Lastname is Required")]
		public string LName { get; set; }

        [Required(ErrorMessage = "Email is Required!")]
		[EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

		[Required(ErrorMessage = "Password is Required")]
		[MinLength(5, ErrorMessage = "Minimum Password Length is 5")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required(ErrorMessage = "Confirm Password is Required")]
		[Compare(nameof(Password), ErrorMessage = "Confirm Password dose not match Password")]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }
        public bool IsAgree { get; set; }
    }
}
