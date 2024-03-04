using Microsoft.AspNetCore.Identity;

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

            //var scopFactory = applicationBuilder.Services.GetRequiredService<IServiceScopeFactory>();
            //using (var scope = scopFactory.CreateScope())
            //{
            //    var userManager = scope.ServiceProvider.GetService<UserManager<User>>();
            //    var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
            //    DataSeeder.SeedRoles(roleManager).Wait();
            //    DataSeeder.SeedUsers(userManager).Wait();
            //}

            //         applicationBuilder.UseEndpoints(endpoints =>
            //         {
            //             endpoints.MapControllerRoute(
            //             name: "areas",
            //            pattern: "{area:admin}/{controller=Dashboard}/{action=index}/{id?}"
            // );
            //             endpoints.MapControllerRoute(
            //            name: "areas1",
            //           pattern: "{area:user}/{controller=Profilim}/{action=index}/{id?}"
            //);
            //             endpoints.MapControllerRoute(
            //             name: "default",
            //             pattern: "{controller=Home}/{action=Index}/{id?}");

            //         });
            applicationBuilder.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
            );
                endpoints.MapControllerRoute(
                    name: "admin",
                    pattern: "admin/{controller=Dashboard}/{action=index}/{id?}",
                    defaults: new { area = "admin" }
                );
                endpoints.MapControllerRoute(
                    name: "user",
                    pattern: "user/{controller=Profilim}/{action=index}/{id?}",
                    defaults: new { area = "user" }
                );
            });

        }
    }
}
