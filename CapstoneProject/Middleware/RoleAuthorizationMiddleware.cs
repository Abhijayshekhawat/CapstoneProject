using CapstoneProject.Attributes;

public class RoleAuthorizationMiddleware
{
    private readonly RequestDelegate _next;

    public RoleAuthorizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Check if the user is logged in by confirming the session contains a user role
        var userRole = context.Session.GetString("UserRole");

        // If no user role in session, redirect to login
        if (string.IsNullOrEmpty(userRole))
        {
            context.Response.Redirect("/Login");
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
