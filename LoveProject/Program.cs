using loveproject.dataa.Abstract;
using loveproject.dataa.Concrete.EfCore;
using LoveProject.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using LoveProject.EmailService;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI;

var builder = WebApplication.CreateBuilder(args);




builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlite("Data Source=Db"));
//builder.Services.AddIdentity<User, IdentityRole>()
//.AddTokenProvider<DataProtectorTokenProvider<User>>(TokenOptions.DefaultProvider)
//.AddEntityFrameworkStores<ApplicationContext>();

builder.Services.AddDefaultIdentity<User>(
    options => options.SignIn.RequireConfirmedAccount = true)
    .AddTokenProvider<DataProtectorTokenProvider<User>>(TokenOptions.DefaultProvider)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationContext>();


builder.Services.Configure<IdentityOptions>(options =>
{
    //password
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;

    //lockout
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.AllowedForNewUsers = true;

    //username
    options.User.AllowedUserNameCharacters = "";
    options.User.RequireUniqueEmail = false;

    //confirmation
    options.SignIn.RequireConfirmedEmail = true;
    options.SignIn.RequireConfirmedPhoneNumber = false;

});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/account/login";
    options.LogoutPath = "/account/logout";
    options.AccessDeniedPath= "/account/accessdenied";
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.Cookie = new CookieBuilder {
        HttpOnly = true,
        Name = ".HowMatch.Security.Cookie",
        SameSite = SameSiteMode.Strict
    };
});

builder.Configuration.AddJsonFile("appsettings.json");

var configBuilder = new ConfigurationBuilder();
configBuilder.AddJsonFile("appsettings.json");
var config = configBuilder.Build();


builder.Services.AddScoped<IEmailService, HotmailService>(i =>
    new HotmailService(config["Smtp:Username"],int.Parse(config["Smtp:Port"]), config["Smtp:Password"], config["Smtp:Host"],bool.Parse(config["Smtp:EnableSSL"]))
);
builder.Services.AddScoped<ILoverMatchRepository, LoverMatchRepository>();



builder.Services.AddControllersWithViews();



var app = builder.Build();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Calculate}/{id?}"
    );

app.UseAuthentication();
app.UseAuthorization();
app.Run();
