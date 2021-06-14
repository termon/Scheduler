using System;
using Scheduler.Data.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
namespace Scheduler.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {   
            
            // ** Add Cookie and Jwt Authentication using extension method **
            //services.AddCookieAndJwtAuthentication(Configuration);

            // ** Add Cookie Authentication via extension method **
            services.AddCookieAuthentication();

            // ** Enable Cors for and webapi endpoints provided **
            //services.AddCors();
            
            // Add Service to DI Container     
            services.AddTransient<IBookingService,BookingService>();

            // ** Required to enable asp-authorize Taghelper **            
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider provider)
        {
            if (env.IsDevelopment())
            {
                // AMC seed users - using service provider to get UserService from DI
                Seeder.Seed(provider.GetService<IBookingService>());
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

            // ** configure cors to allow full cross origin access to any webapi end points **
            //app.UseCors(c => c.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            // ** turn on authentication/authorisation **
            app.UseAuthentication();
            app.UseAuthorization();
 
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
