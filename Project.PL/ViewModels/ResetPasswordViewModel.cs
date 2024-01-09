using System.ComponentModel.DataAnnotations;

namespace Project.PL.ViewModels
{
	public class ResetPasswordViewModel
	{

		[Required(ErrorMessage = "Password is Required")]
		[DataType(DataType.Password)]
		public string NewPassword { get; set; }

		[Required(ErrorMessage = "Confirm Password is Required")]
		[Compare(nameof(NewPassword), ErrorMessage = "Confirm Password dose not match Password")]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }

    }
}
