using EntityExample.Data;
using EntityExample.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EntityExample.Controllers.Account
{
	public class AccountController : Controller
	{
		private readonly ApplicationContext _context;
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		[BindProperty]
		public InputModel Input { get; set; }
		public AccountController(
			ApplicationContext context,
			UserManager<User> userManager,
			RoleManager<IdentityRole> roleManager,
		SignInManager<User> signInManager)
		{
			_context = context;
			_userManager = userManager;
			_roleManager = roleManager;
			_signInManager = signInManager;
		}

		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SignUp()
		{
			var newUser = Activator.CreateInstance<User>();

			if (ModelState.IsValid && newUser != null)
			{
				try
				{
					newUser.UserName = Input.UserName;
					newUser.Email = Input.Email;

					var result = await _userManager.CreateAsync(newUser, Input.Password);

					if (!result.Succeeded) throw new Exception("Failed to create user");

					var roleResult = await _userManager.AddToRoleAsync(newUser, UserRoles.User.ToString());

					if (!roleResult.Succeeded) throw new Exception("Failed to add user to role");

					return RedirectToAction("Index", "Home");


				}
				catch (Exception ex)
				{
					ModelState.AddModelError("Error", ex.Message);
					Console.WriteLine(ex.Message);
					return RedirectToAction("Register", "Account");
				}
			}
			else
			{
				ModelState.AddModelError("Error", "Invalid model state");
				return RedirectToAction("Register", "Account");
			}
		}

		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SignIn()
		{
			string userName = GetUserByEmailAddress(Input.Email);
			if (ModelState.IsValid && userName != null)
			{
				try
				{
					var result = await _signInManager.PasswordSignInAsync(userName, Input.Password, false, false);
					if(!result.Succeeded) throw new Exception("Failed to sign in user");
					return RedirectToAction("Index", "Home");
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
					return RedirectToAction("Login", "Account");
				}
			}
			else
			{
				ModelState.AddModelError("Error", "Invalid model state");
				Console.WriteLine("Invalid model state or user not found");
				return RedirectToAction("Login", "Account");
			}
		}

		private string GetUserByEmailAddress(string email)
		{
			var user = _context.Users.FirstOrDefault(u => u.Email == email);
			return user?.UserName;
		}


		public IActionResult SignOut()
		{
			_signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}


		public IActionResult AccessDenied()
		{
			return View();
		}
	}
}
