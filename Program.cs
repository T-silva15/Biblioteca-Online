using Biblioteca_LAB.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
	.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    //passsword default settings, we can change this to our needs
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // user settings
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvxzwyABCDEFGHIJKLMNOPQRSTUVXZWY0123456789.:-_~/@#!?='\"\\";
    options.User.RequireUniqueEmail = true;
	options.SignIn.RequireConfirmedEmail = true;
});


builder.Services.AddControllersWithViews();

builder.Services.AddScoped<RoleManager<IdentityRole>>();

builder.Services.AddSession();

var app = builder.Build();

using( var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
	SeedRoles.Seed(roleManager);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();


app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();

/*
app.UseEndpoints(endpoints =>
{
    // Rota personalizada para "Requisitas/Entregar/{idLeitor}/{isbn}"
    endpoints.MapControllerRoute(
        name: "entregar",
        pattern: "Requisitas/Entregar/{idLeitor}/{isbn}",
        defaults: new { controller = "Requisitas", action = "Entregar" });
}); */

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.MapRazorPages()
   .WithStaticAssets();

app.Run();
