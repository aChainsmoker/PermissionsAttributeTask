using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using PermissionsAttributeTask.DataAccess.DbContext;
using PermissionsAttributeTask.DataAccess.Repositories;
using PermissionsAttributeTask.DataAccess.Repositories.Abstractions;
using PermissionsAttributeTask.DataAccess.Seeding;
using PermissionsAttributeTask.Main.Configurations;
using PermissionsAttributeTask.Main.Extensions.Middleware;
using PermissionsAttributeTask.Main.Middleware;
using PermissionsAttributeTask.Main.Services;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();


    builder.Services.AddControllersWithViews();

    builder.Services.AddDbContext<PermissionsAttributeTaskDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnectionString"))
    );

    builder.Services.AddIdentity<IdentityUser, IdentityRole>()
        .AddEntityFrameworkStores<PermissionsAttributeTaskDbContext>()
        .AddDefaultTokenProviders();

    builder.Services.AddScoped<IPermissionService, PermissionService>();
    builder.Services.AddScoped<IProfilesRepository, ProfileRepository>();
    builder.Services.AddScoped<DbSeeder>();

    builder.Services.Configure<PermissionRolesSettings>(builder.Configuration.GetSection("PermissionRolesSettings"));

    var app = builder.Build();

    using (var scope = app.Services.CreateScope())
    {
        var dbSeeder = scope.ServiceProvider.GetRequiredService<DbSeeder>();
        try
        {
            await dbSeeder.Seed();
        }
        catch (Exception ex)
        {
            logger.Error(ex, "An error occurred while seeding the database.");
        }
    }

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseHttpMethodOverride();

    app.UseRouting();
    
    app.UseRequestsTiming();
    
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception exception)
{
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}


