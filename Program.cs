using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddAuthentication()
    .AddGoogle(googleOption =>
    {
        googleOption.ClientId = builder.Configuration.GetSection("GoogleAuthSettings").GetValue<string>("ClientId");
        googleOption.ClientSecret = builder.Configuration.GetSection("GoogleAuthSettings").GetValue<string>("ClientSecret");
    })
    .AddFacebook(facebookOptions =>
    {
        facebookOptions.AppId = builder.Configuration.GetSection("FacebookAuthSettings").GetValue<string>("AppId");
        facebookOptions.AppSecret = builder.Configuration.GetSection("FacebookAuthSettings").GetValue<string>("AppSecret");
    })
    .AddMicrosoftAccount(mapOptions =>
    {
        mapOptions.ClientId = builder.Configuration.GetSection("MicrosoftAuthSettings").GetValue<string>("ClientId");
        mapOptions.ClientSecret = builder.Configuration.GetSection("MicrosoftAuthSettings").GetValue<string>("ClientSecret");
    });
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});

// See: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/app-state#configure-session-state
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    // options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

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
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();


app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}"
    );
});
app.MapRazorPages();

app.Run();
