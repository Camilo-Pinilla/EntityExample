using EntityExample.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace EntityExample.Services.Roles
{
	public class RoleService
	{
		public static void Initial(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
		{
			// Set roles by default
			SetRoles(roleManager);
			// set users to database by default
			SetUsers(userManager);
		}


		private static Dictionary<UserRoles, IdentityResult> SetRoles(RoleManager<IdentityRole> roleManager)
		{
			Dictionary<UserRoles, IdentityResult> rolesList = []; 

			if (!roleManager.RoleExistsAsync(UserRoles.Admin.ToString()).Result)
			{
				IdentityResult roleResult = roleManager.CreateAsync(new IdentityRole(UserRoles.Admin.ToString())).Result;
				if (roleResult.Succeeded)
				{
					rolesList.Add(UserRoles.Admin, roleResult);
				}
			}

			if (!roleManager.RoleExistsAsync(UserRoles.Manager.ToString()).Result)
			{
				IdentityResult roleResult = roleManager.CreateAsync(new IdentityRole(UserRoles.Manager.ToString())).Result;
				if (roleResult.Succeeded)
				{
					rolesList.Add(UserRoles.Manager, roleResult);
				}
			}

			if (!roleManager.RoleExistsAsync(UserRoles.User.ToString()).Result)
			{
				IdentityResult roleResult = roleManager.CreateAsync(new IdentityRole(UserRoles.User.ToString())).Result;
				if (roleResult.Succeeded)
				{
					rolesList.Add(UserRoles.User, roleResult);
				}
			}

			return rolesList;
		}

		private static void SetUsers(UserManager<User> userManager)
		{
			// set users to database by default

			if (userManager.FindByEmailAsync("administrator@gmail.com").Result == null)
			{
				var newUser = Activator.CreateInstance<User>();
				newUser.UserName = "administrator";
				newUser.Email = "administrator@gmail.com";

				IdentityResult userResult = userManager.CreateAsync(newUser, "$PasswordSegura$_20").Result;

				if (userResult.Succeeded)
				{
					userManager.AddToRoleAsync(newUser, UserRoles.Admin.ToString()).Wait();
				}
				else
				{
					Console.WriteLine("Error: " + userResult.Errors);
				}
			}

			if (userManager.FindByEmailAsync("manager@gmail.com").Result == null)
			{
				var newUser = Activator.CreateInstance<User>();
				newUser.UserName = "manager";
				newUser.Email = "manager@gmail.com";

				IdentityResult userResult = userManager.CreateAsync(newUser, "$PasswordSegura$_21").Result;

				if (userResult.Succeeded)
				{
					userManager.AddToRoleAsync(newUser, UserRoles.Manager.ToString()).Wait();
				}
				else
				{
					Console.WriteLine("Error: " + userResult.Errors);
				}
			}
		}	
	}
}
