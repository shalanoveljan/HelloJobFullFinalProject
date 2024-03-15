using HelloJob.Entities.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using System.Web.Mvc;

namespace HelloJob.App.Configurations
{
    public static class AppConfiguration
    {
        public static void AddDefaultConfiguration(this WebApplication applicationBuilder, IWebHostEnvironment webHostEnvironment)
        {

            if (webHostEnvironment.IsDevelopment())
            {
                applicationBuilder.UseDatabaseErrorPage();
            }
            else
            {
                applicationBuilder.UseDeveloperExceptionPage();
                //applicationBuilder.UseExceptionHandler("/Home/Error");
                applicationBuilder.UseHsts();
            }
            applicationBuilder.UseHttpsRedirection();
            applicationBuilder.UseStaticFiles();

            applicationBuilder.UseRouting();

            applicationBuilder.UseAuthentication();
            applicationBuilder.UseAuthorization();

            var scopFactory = applicationBuilder.Services.GetRequiredService<IServiceScopeFactory>();
            using (var scope = scopFactory.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetService<UserManager<AppUser>>();
                var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                DataSeeder.SeedRoles(roleManager).Wait();
                DataSeeder.SeedUsers(userManager).Wait();
            }

            //           applicationBuilder.UseEndpoints(endpoints =>
            //           {


            //               //endpoints.MapControllerRoute(
            //               //    name: "areas",
            //               //    pattern: "{area:exists}/{controller=Account}/{action=Login}/{id?}"
            //               //);

            //               //endpoints.MapControllerRoute(
            //               //    name: "adminArea",
            //               //    pattern: "admin/{controller=Account}/{action=Login}/{id?}",
            //               //    defaults: new { area = "Admin" }
            //               //);

            //               //endpoints.MapControllerRoute(
            //               //    name: "userArea",
            //               //    pattern: "user/{controller=Dashboard}/{action=index}/{id?}",
            //               //    defaults: new { area = "User" }
            //               //);

            //               //// Default yönlendirme
            //               //endpoints.MapControllerRoute(
            //               //    name: "default",
            //               //    pattern: "{controller=Home}/{action=Index}/{id?}"
            //               //);

            //               //             endpoints.MapControllerRoute(
            //               //       name: "areas",
            //               //      pattern: "{area:exists}/{controller=dashboard}/{action=index}/{id?}"
            //               //);
            //               //             endpoints.MapControllerRoute(
            //               //             name: "default",
            //               //             pattern: "{controller=Home}/{action=Index}/{id?}");

            //               endpoints.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
            //    namespaces: new[] { "YourNamespace.Controllers" }
            //);
            //           });


            
                applicationBuilder.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "areaRoute",
                        pattern: "{area:exists}/{controller=dashboard}/{action=index}/{id?}");

                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");
                });

        }
    }
}
