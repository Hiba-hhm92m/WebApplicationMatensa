using Azure.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net.Sockets;

namespace WebApplicationMatensa.Attributes
{
    public sealed class AdminAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context != null)
            {
                string authorizationHeader = context.HttpContext.Request.Headers["Authorization"]!;
                if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
                {
                    context.Result = new JsonResult(new { message = "Token is required" }) { StatusCode = StatusCodes.Status401Unauthorized };
                    return;
                }
                // Remove "Bearer" to get pure token data
                var token = authorizationHeader.Substring("Bearer ".Length);

                if (token != "123456")
                    context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
