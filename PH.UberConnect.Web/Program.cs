using PH.UberConnect.Api;
using PH.UberConnect.Core.Service;
using PH.UberConnect.Web.AppSettings;
using PH.UberConnect.Web.Extensions;

// Add Selirog for use Logs
SerilogExtension.AddSerilog();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add a Singleton to use the signing keys
var signingKeys = builder.Configuration.GetSection("SigningKeys").Get<SigningKeys>();
builder.Services.AddSingleton(signingKeys);

// Add a Singleton to use the PH services
var services = new Services(builder.Configuration.GetConnectionString("PHCRSRVMCARIIS0"));
builder.Services.AddSingleton(services);

// Add a singleton to use the uber API
var apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>();
builder.Services.AddSingleton(apiSettings);

Apis api = new(apiSettings.Url, apiSettings.AuthUrl, apiSettings.CustomerId);
builder.Services.AddSingleton(api);

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = "PHUberConnect.Web";
    options.IdleTimeout = TimeSpan.FromHours(8);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

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

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
