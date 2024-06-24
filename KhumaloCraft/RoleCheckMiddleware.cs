using Microsoft.AspNetCore.Identity;

namespace KhumaloCraft
{
    public class RoleCheckMiddleware
    {
        private readonly RequestDelegate _next;

        public RoleCheckMiddleware(RequestDelegate next) {  _next = next; }

        public async Task InvokeAsync(HttpContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            var path = context.Request.Path;
            if (!path.StartsWithSegments("/Home/SelectRole"))
            {
                if (signInManager.IsSignedIn(context.User))
                {
                    var user = await userManager.GetUserAsync(context.User);
                    var roles = await userManager.GetRolesAsync(user);
                    if (!roles.Any())
                    {
                        context.Response.Redirect("/Home/SelectRole");
                        return;
                    }
                }
            }
            
            await _next(context);
        }
    }
}
