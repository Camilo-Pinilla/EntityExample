using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EntityExample.Models
{
	public class User : IdentityUser
	{
	}

	public class InputModel
	{
		[DisplayName("User Name")]
		[StringLength(256, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string? UserName { get; set; }
		[Required]
		[EmailAddress]
        public required string Email { get; set; }
		[Required]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
        public required string Password { get; set; }
	}


	public enum UserRoles
	{
		Admin , 
		Manager, 
		User
	}
}
