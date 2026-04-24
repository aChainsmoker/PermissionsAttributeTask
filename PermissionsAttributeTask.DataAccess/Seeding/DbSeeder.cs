using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using PermissionsAttributeTask.DataAccess.DbContext;
using PermissionsAttributeTask.DataAccess.Entities;

namespace PermissionsAttributeTask.DataAccess.Seeding;

public class DbSeeder
{
    private readonly PermissionsAttributeTaskDbContext _dbContext;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public DbSeeder(PermissionsAttributeTaskDbContext dbContext, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _roleManager = roleManager;
    }
    public async Task Seed()
    {
        var roles = new[] { "User", "Manager", "Administrator" };
        
        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        var adminUser = new IdentityUser
        {
            UserName = "admin@example.com",
            Email = "admin@example.com",
            EmailConfirmed = true
        };

        var adminPassword = "AdminPass123!";

        if (await _userManager.FindByEmailAsync("admin@example.com") == null)
        {
            var createAdminResult = await _userManager.CreateAsync(adminUser, adminPassword);

            if (createAdminResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(adminUser, "Administrator");
            }
        }

        var managerUser = new IdentityUser
        {
            UserName = "manager@example.com",
            Email = "manager@example.com",
            EmailConfirmed = true
        };

        var managerPassword = "ManagerPass123!";

        if (await _userManager.FindByEmailAsync("manager@example.com") == null)
        {
            var createManagerResult = await _userManager.CreateAsync(managerUser, managerPassword);

            if (createManagerResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(managerUser, "Manager");
            }
        }

        var regularUser = new IdentityUser
        {
            UserName = "user@example.com",
            Email = "user@example.com",
            EmailConfirmed = true
        };

        var userPassword = "UserPass123!";

        if (await _userManager.FindByEmailAsync("user@example.com") == null)
        {
            var createUserResult = await _userManager.CreateAsync(regularUser, userPassword);

            if (createUserResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(regularUser, "User");
            }
        }
        
        if (!_dbContext.UserProfiles.Any())
        {
            var userProfiles = new List<UserProfile>
            {
                new UserProfile
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Admin",
                    LastName = "User",
                    Age = 30,
                    About = "Administrator account"
                },
                new UserProfile
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Manager",
                    LastName = "User",
                    Age = 35,
                    About = "Manager account"
                },
                new UserProfile
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Regular",
                    LastName = "User",
                    Age = 25,
                    About = "Regular user account"
                }
            };

            _dbContext.UserProfiles.AddRange(userProfiles);
            await _dbContext.SaveChangesAsync();
        }
    }
}