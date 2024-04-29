using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PL.Helper;
using PL.ViewModels;
using System.Threading.Tasks;

namespace PL.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser> userManager , SignInManager<ApplicationUser> signInManager)
        {
			_userManager = userManager;
			_signInManager = signInManager;
		}

		#region Sign Up
		[HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
		[HttpPost]
		public async Task<IActionResult> SignUp(SignUpViewModel model)
		{
            if (ModelState.IsValid) //Server Side Validation
            {
				var User = await _userManager.FindByNameAsync(model.UserName);
				if (User == null)
				{
					User= await _userManager.FindByEmailAsync(model.Email);
					if (User == null)
					{
						//Manual Mapping
						User = new ApplicationUser
						{
							UserName = model.UserName,
							Email = model.Email,
							FirstName = model.FirstName,
							LastName = model.LastName,
							IsAgree = model.IsAgree
						};
						var Result = await _userManager.CreateAsync(User, model.Password);
						if (Result.Succeeded)
						{
							return RedirectToAction(nameof(SignIn));
						}
                        foreach (var error in Result.Errors)
                        {
							ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
					
				}
				ModelState.AddModelError(string.Empty, "User is Already Exist (:");
            }

			return View(model);
		}

		#endregion


		#region Sign In

		[HttpGet]
		public IActionResult SignIn()
		{
			return View();
		}
		[HttpPost]
		public async Task< IActionResult> SignIn(SignInViewModel model)
		{ 
			if (ModelState.IsValid)
			{
				var User = await _userManager.FindByEmailAsync(model.Email);
				if (User != null)
				{
					var Flag =	await _userManager.CheckPasswordAsync(User, model.Password);
					if (Flag)
					{
						var Result= await _signInManager.PasswordSignInAsync(User, model.Password, model.RememberMe, false);
						if (Result.Succeeded)
						{
							return RedirectToAction(nameof(HomeController.Index),"Home");
						}
					}
				}
				ModelState.AddModelError(string.Empty, "Invalid Login!!");
			}

			return View(model);
		}

		#endregion


		#region Sign Out

		public new async Task< IActionResult> SignOut()
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
			if (ModelState.IsValid)
			{
				var User = await _userManager.FindByEmailAsync(model.Email);
				if (User != null)
				{
					//Generate Token

				    var Token = await _userManager.GeneratePasswordResetTokenAsync(User);
					
					//Create URL Which Send In Body Of Email

					var url = Url.Action("ResetPassword", "Account", new {email = model.Email , token = Token},Request.Scheme);
					
					//https://localhost:44323/Account/ResetPassword?email=ahmedsayed@gmail.com?token=

					//Create Email

					var Email = new Email()
					{
						Subject = "Reset Your Password",
						Reciept = model.Email,
						Body = url

					};

					//Send Email

					EmailSetting.SendEmail(Email);

					//Redirect To Action
					return RedirectToAction(nameof(CheckYourInbox));
				}
				ModelState.AddModelError(string.Empty, "Invalid Email");
			}
			return View(nameof(ForgetPassword),model);
		}

		public IActionResult CheckYourInbox()
		{
			return View();
		}
		#endregion


		#region Reset Password
		[HttpGet]
		public IActionResult ResetPassword(string email,string token)
		{
			TempData["email"]=email;
			TempData["token"]=token;

			return View();
		}
		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var email = TempData["email"] as string;
				var token = TempData["token"] as string;
				var User = await _userManager.FindByEmailAsync(email);
				if(User is not null)
				{
					
					var Result = await _userManager.ResetPasswordAsync(User,token,model.NewPassword);
					if (Result.Succeeded)
					{
						return RedirectToAction(nameof(SignIn));
					}
                    foreach (var error in Result.Errors)
                    {
						ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
				ModelState.AddModelError(string.Empty, "Invalid Reset Password!!");

			}
			return View(model);
		}

		#endregion

		public IActionResult AccessDenied()
		{
			return View();
		}

	}
}
