using System.Net;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using OpenAI.GPT3.Extensions;

using Web.Data;
using Web.Models;
using Web.Services;
using Web.Utils;

using Web.Utils;

using Web.API;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR(options =>
{
    options.MaximumReceiveMessageSize = 5000 * 1024; // 5 MB or 5000 KB
});

// In-house Services
builder.Services.AddScoped<FoodItemService>();
builder.Services.AddScoped<FoodCategoryService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IHouseholdService, HouseholdService>();
builder.Services.AddTransient<IGroceryFoodService, GroceryFoodService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<OpenAIApiService>();
builder.Services.AddScoped<RecipeService>();


builder.Services.AddScoped<ChatService>();


builder.Services.AddScoped<ICaptchaService, CaptchaService>();
builder.Services.AddOpenAIService();
builder.Services.AddTransient<UserManager<User>>();
builder.Services.AddScoped<DonationService>();
builder.Services.AddScoped<CustomFoodService>();
builder.Services.AddScoped<DonationRequestService>();
builder.Services.AddScoped<MessageService>();
builder.Services.AddScoped<ReviewService>();

builder.Services.AddScoped<CategoryService>();
builder.Services.AddTransient<OpenAIHub>();
builder.Services.AddScoped<GoogleAIService>();

builder.Services.AddScoped<ReviewService>();

builder.Services.AddDbContext<DataContext>(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        // Setup SQLite Connection
        var devConnectionString = builder.Configuration.GetConnectionString("dev");
        options.UseSqlite(connectionString: devConnectionString);
    }

    else if (builder.Environment.IsProduction())
    {
        // Setup MySQL Connection
        var prodConnectionString = builder.Configuration.GetConnectionString("prod");
        var serverVersion = ServerVersion.AutoDetect(prodConnectionString);
        options.UseMySql(connectionString: prodConnectionString, serverVersion);
    }
    else
    {
        Console.WriteLine("[WARNING]: Environment is neither Development or Production.");
        Console.WriteLine("[WARNING]: System will default to using SQLite database provider.");
        var devConnectionString = builder.Configuration.GetConnectionString("dev");
        options.UseSqlite(connectionString: devConnectionString);
    }
});
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();
// Configure Identity Options
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(30);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+!&*()~|`#%^,";
});

//builder.Services.AddAuthentication().AddGoogle(googleOptions =>
//{
//    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
//    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
//    googleOptions.CallbackPath = "/signin-google";

//});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
// NOTE: Stores to server memory
// TODO: Change to externals stores to allow horizontal scalling
builder.Services.AddDistributedMemoryCache();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

}
 );
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

builder.Services.ConfigureApplicationCookie(options =>
{

    options.AccessDeniedPath = "/login";
    options.LoginPath = "/login";


});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
WebRequest.DefaultWebProxy = null;

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();
app.MapControllers();
app.MapRazorPages();
app.MapHub<OpenAIHub>("/openAIHub");
app.MapHub<ChatHub>("/Chathub");



app.MapHub<HouseholdHub>("/householdHub");
app.MapHub<NotificationHub>("/notificationHub");



app.Run();