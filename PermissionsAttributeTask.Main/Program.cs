using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PermissionsAttributeTask.DataAccess.DbContext;
using PermissionsAttributeTask.DataAccess.Repositories;
using PermissionsAttributeTask.DataAccess.Repositories.Abstractions;
using PermissionsAttributeTask.DataAccess.Seeding;
using PermissionsAttributeTask.Main.Configurations;
using PermissionsAttributeTask.Main.Services;

var builder = WebApplication.CreateBuilder(args);

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
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
