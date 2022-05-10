using Scheduler.Web;
using Scheduler.Data.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// ** Add Cookie Authentication via extension method **
builder.Services.AddCookieAuthentication();

// Add UserService to DI           
builder.Services.AddTransient<IBookingService,BookingService>();

// ** Required to enable asp-authorize Taghelper **            
builder.Services.AddHttpContextAccessor(); 

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else 
{
    // seed users in development mode - using service provider to get BookingService from DI
    Seeder.Seed(app.Services.GetService<IBookingService>());
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ** turn on authentication/authorisation **
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();