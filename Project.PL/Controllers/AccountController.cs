using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.DAL.Models;
using Project.PL.Helpers;
using Project.PL.ViewModels;
using System.Threading.Tasks;

namespace Project.PL.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager )
        {
			_userManager = userManager;
			_signInManager = signInManager;
		}

        #region Sign UP
        public IActionResult SignUp()
        {
            return View();
        }

		[HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user is null)
                {
					user = new ApplicationUser()
					{
						UserName = model.UserName,
						Email = model.Email,
						FName = model.FName,
						LName = model.LName,
						IsAgree = model.IsAgree,
						

					};
					var result = await _userManager.CreateAsync(user, model.Password);
					if(result.Succeeded)
						return RedirectToAction(nameof(SignIn));

					foreach(var error in result.Errors)
						ModelState.AddModelError(string.Empty, error.Description);
				}

				ModelState.AddModelError(string.Empty, "Username is already Exists :) ");
			}
			return View(model);
		}
		#endregion

		#region Sign IN
		public IActionResult SignIn() 
		{ 
			return View(); 
		}
		[HttpPost]
		public async Task<IActionResult> SignIn(SignInViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if(user is not null)
				{
					var flag = await _userManager.CheckPasswordAsync(user, model.Password);
					if (flag)
					{
						var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
						if(result.Succeeded)
							return RedirectToAction(nameof(HomeController.Index), "Home");
					}

				}
				ModelState.AddModelError(string.Empty, "Invalid Lohin");
			}
			return View(model);
		}
		#endregion

		#region Sign Out
		public new async Task<IActionResult> SignOut()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction(nameof(SignIn));
		}
		#endregion

		#region Forget Password

		public IActionResult ForgetPassword()
		{
			return View();
		}

		public async Task<IActionResult> SendResetPasswordUrl(ForgetPasswordViewModel model)
		{
			if(ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user is not null)
				{
					var token = await _userManager.GeneratePasswordResetTokenAsync(user); // Unique for this user for One-Time
					var resetPassword = Url.Action("ResetPassword", "Account", new {email = model.Email, token }, Request.Scheme);

					var email = new Email()
					{
						Subject = "Reset You Password",
						Recipients = model.Email,
						Body = resetPassword
					};
					EmailSettings.SendEmail(email);
					return RedirectToAction(nameof(CheckYourInbox));
				}
				ModelState.AddModelError(string.Empty, "Invalid Email");
			}
			return View(model);
		}

		public IActionResult CheckYourInbox()
		{
			return View();	
		}

		#endregion

		#region Reset Password
		public IActionResult ResetPassword(string email, string token)
		{
			TempData["Email"] = email;
			TempData["Token"] = token;
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				string email = TempData["Email"] as string;
				string token = TempData["Token"] as string;

				var user = await _userManager.FindByEmailAsync(email);

				var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

				if(result.Succeeded)
					return RedirectToAction(nameof(SignIn));

				foreach (var error in result.Errors)
					ModelState.AddModelError(string.Empty, error.Description);
			}
			return View(model);
		}
		#endregion
	}
}
