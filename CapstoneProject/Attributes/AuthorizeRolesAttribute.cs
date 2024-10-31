using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace CapstoneProject.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class AuthorizeRolesAttribute : Attribute, IAuthorizationFilter
    {
        public string[] Roles { get; }

        public AuthorizeRolesAttribute(params string[] roles)
        {
            Roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userRole = context.HttpContext.Session.GetString("UserType");

            if (string.IsNullOrEmpty(userRole) || !Array.Exists(Roles, role => role == userRole))
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
