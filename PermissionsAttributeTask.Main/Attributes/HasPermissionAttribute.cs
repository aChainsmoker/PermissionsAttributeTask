using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Identity;
using PermissionsAttributeTask.DataAccess.Entities;
using PermissionsAttributeTask.Main.Models;
using PermissionsAttributeTask.Main.Services;

namespace PermissionsAttributeTask.Main.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class HasPermissionAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private Permissions Permission { get; }

        public HasPermissionAttribute(Permissions permission)
        {
            Permission = permission;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity != null && !context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new RedirectToActionResult("Login", "Account", new { returnUrl = context.HttpContext.Request.Path });
                
                return;
            }

            var permissionService = context.HttpContext.RequestServices.GetService<IPermissionService>();
            if (permissionService == null)
            {
                context.Result = new ObjectResult("Permission service is not available") { StatusCode = 500 };
                
                return;
            }

            var userManager = context.HttpContext.RequestServices.GetService<UserManager<IdentityUser>>();
            if (userManager == null)
            {
                context.Result = new ObjectResult("User manager is not available") { StatusCode = 500 };
                
                return;
            }
            
            var userId = context.HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var user = await userManager.FindByIdAsync(userId!);
            if (user == null)
            {
                context.Result = new UnauthorizedResult();
                
                return;
            }

            bool hasPermission = await permissionService.HasPermissionAsync(user, Permission);
            if (!hasPermission)
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Account", new { returnUrl = context.HttpContext.Request.Path });
            }
        }
    }
}