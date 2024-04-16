
using Microsoft.AspNetCore.Http.Extensions;

namespace EntityExample.Common
{
	public class GetLastParameter : IGetLastParameter
	{
		public string GetParameter(HttpContext httpContext)
		{
			string parameter = httpContext.Request.GetDisplayUrl().Split("/").Last();
			return parameter;
		}
	}
}
