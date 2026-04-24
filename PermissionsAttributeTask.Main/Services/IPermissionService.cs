using Microsoft.AspNetCore.Identity;
using PermissionsAttributeTask.DataAccess.Entities;
using PermissionsAttributeTask.Main.Models;

namespace PermissionsAttributeTask.Main.Services
{
    public interface IPermissionService
    {
        Task<bool> HasPermissionAsync(IdentityUser user, Permissions permission);
    }
}