using EntityExample.Common;
using EntityExample.Data;
using EntityExample.Models;
using EntityExample.Services.AuthPolicies.Requirements;
using Microsoft.AspNetCore.Authorization;
using NuGet.Versioning;
using System.Security.Claims;

namespace EntityExample.Services.AuthPolicies.HandlerRequirements
{
	public class UpdatingAndDeletionReqHandler : AuthorizationHandler<UpdatingAndDeletionRequirement>
	{

		private readonly IGetLastParameter _getLastParameter;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly ApplicationContext _context;

		public UpdatingAndDeletionReqHandler(
			IGetLastParameter getLastParameter,
			IHttpContextAccessor httpContextAccessor,
			ApplicationContext context)
		{
			_getLastParameter = getLastParameter;
			_httpContextAccessor = httpContextAccessor;
			_context = context;
		}


		protected override Task HandleRequirementAsync(
			AuthorizationHandlerContext context,
			UpdatingAndDeletionRequirement requirement)
		{


			try
			{
				HttpContext httpContext = _httpContextAccessor.HttpContext;

				//EXCEPTION
				if (httpContext is null) throw new ArgumentNullException(nameof(httpContext));

				
				string lastParameter = _getLastParameter.GetParameter(httpContext);

				string currentAction = httpContext.Request.RouteValues["action"].ToString();

				//EXCEPTION
				if (currentAction is null) throw new ArgumentNullException(nameof(currentAction));

				//EXCEPTION
				if (!(currentAction == requirement.ActionOne || currentAction == requirement.ActionTwo)) throw new Exception("Action not allowed");

				var currentUserEmail = context.User.FindFirst(ClaimTypes.Email);
		
				//EXCEPTION
				if (currentUserEmail is null) throw new ArgumentNullException(nameof(currentUserEmail));

				var idOfUserContact = _context.Contacts
					.Where(c => c.EmailAddress == currentUserEmail.Value)
					.Select(c => c.Id)
					.FirstOrDefault().ToString();

				if (context.User.IsInRole(UserRoles.Admin.ToString()))
				{
					context.Succeed(requirement);
					return Task.CompletedTask;
				}else if (lastParameter == idOfUserContact)
				{
					context.Succeed(requirement);
					return Task.CompletedTask;
				}
				else
				{
					context.Fail();
					return Task.CompletedTask;
				}
		

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return Task.CompletedTask;
			}

		}
	}
}
