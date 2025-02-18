using CapstoneProject.Attributes;
using System;

public class RoleAuthorizationMiddleware
{
    private readonly RequestDelegate _next;

    public RoleAuthorizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Set a default UserType if not already set in the session
        if (string.IsNullOrEmpty(context.Session.GetString("UserType")))
        {
            context.Session.SetString("UserType", "Client");
        }
        // Get the current route path
        var path = context.Request.Path.ToString().ToLower();
        Console.WriteLine($"Middleware processing path: {path}, method: {context.Request.Method}");

        // Skip authorization check for Login and other public pages or for POST requests to login
        if (path == "/" || (context.Request.Method == "POST") || path == "/secure/index")
        {
            if (path == "/secure/index" || path.Contains("SAML") || path.Contains("duosecurity"))
            {
                context.Session.SetString("UserType", "Client");
            }
            //https://api-b2781b65.duosecurity.com/frame/v4/auth/prompt?sid=frameless-ba85e860-a5c8-477a-9e59-a746700fbf49
            //https://np-fim.temple.edu/idp/profile/SAML2/Redirect/SSO?execution=e1s2&_eventId_proceed=1
            Console.WriteLine("Bypassing authorization for login page.");
            await _next(context);
            return;
        }

        // Check if the user is logged in by confirming the session contains a user role
        var userRole = context.Session.GetString("UserType");

        // If no user role in session, redirect to login
        if (string.IsNullOrEmpty(userRole))
        {
            context.Response.Redirect("/");
            return;
        }

        // Check if the endpoint has the AuthorizeRoles attribute
        var endpoint = context.GetEndpoint();
        var authorizeRoles = endpoint?.Metadata.GetMetadata<AuthorizeRolesAttribute>();

        if (authorizeRoles != null)
        {
            // If the user role is not in the allowed roles, deny access
            if (!authorizeRoles.Roles.Contains(userRole))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden; // Forbidden
                return;
            }
        }

        // Continue to the next middleware if authorized
        await _next(context);
    }
}
