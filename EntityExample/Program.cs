using EntityExample.Common;
using EntityExample.Data;
using EntityExample.Models;
using EntityExample.Services.AuthPolicies;
using EntityExample.Services.AuthPolicies.HandlerRequirements;
using EntityExample.Services.AuthPolicies.Requirements;
using EntityExample.Services.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection") ?? 
		throw new Exception("Database not found"));
});

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
	options.SignIn.RequireConfirmedAccount = false;
	options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789·ÈÌÛ˙¡…Õ”⁄¸‹Ò—-._@+ ";

}).AddRoles<IdentityRole>()
	.AddEntityFrameworkStores<ApplicationContext>()
	.AddDefaultTokenProviders();

builder.Services.AddAuthentication();

builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("EditingAndDeletion", options =>
	options.Requirements.Add(new UpdatingAndDeletionRequirement("Edit", "Delete")));
});

builder.Services.AddTransient<IAuthorizationHandler, UpdatingAndDeletionReqHandler>();
builder.Services.AddSingleton<IGetLastParameter, GetLastParameter>();


builder.Services.AddHttpContextAccessor();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var serviceProvider = scope.ServiceProvider;
	var context = serviceProvider.GetRequiredService<ApplicationContext>();
	context.Database.Migrate();
}

using (var scope = app.Services.CreateScope())
{
	var serviceProvider = scope.ServiceProvider;
	var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
	var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
	RoleService.Initial(roleManager, userManager);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
