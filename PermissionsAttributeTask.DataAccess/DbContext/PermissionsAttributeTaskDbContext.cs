using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PermissionsAttributeTask.DataAccess.Configurations;
using PermissionsAttributeTask.DataAccess.Entities;

namespace PermissionsAttributeTask.DataAccess.DbContext;

public class PermissionsAttributeTaskDbContext : IdentityDbContext<IdentityUser>
{
    public PermissionsAttributeTaskDbContext(DbContextOptions<PermissionsAttributeTaskDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new UserProfileConfigurations());
        base.OnModelCreating(builder);
    }

    public DbSet<UserProfile> UserProfiles { get; set; }
}