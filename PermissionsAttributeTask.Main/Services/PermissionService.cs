using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using PermissionsAttributeTask.Main.Configurations;
using PermissionsAttributeTask.Main.Models;

namespace PermissionsAttributeTask.Main.Services;

public class PermissionService : IPermissionService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly PermissionRolesSettings _permissionRolesOptions;
    
    public PermissionService(UserManager<IdentityUser> userManager, IOptions<PermissionRolesSettings> permissionRolesOptions)
    {
        _userManager = userManager;
        _permissionRolesOptions = permissionRolesOptions.Value;
    }
    
    public async Task<bool> HasPermissionAsync(IdentityUser user, Permissions permission)
    {
        var userRoles = await _userManager.GetRolesAsync(user);
        if (!_permissionRolesOptions.PermissionRoles.TryGetValue(permission.ToString(), out var requiredRoles))
        {
            return false;
        }

        return userRoles.Any(requiredRoles.Contains);
    }
}