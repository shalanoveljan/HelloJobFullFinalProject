using HelloJob.Core.Configuration.Concrete;
using HelloJob.Data.DAL.Implementations;
using HelloJob.Data.DAL.Interfaces;
using HelloJob.Data.DBContexts.SQLSERVER;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Data.ServiceRegistrations
{
    public static class DataAccessServiceRegistration
    {
        public static void DataAccessServiceRegister(this IServiceCollection services)
        {
            //    services.AddDbContext<HelloJobDbContext>(opt =>
            //    {
            //        opt.UseSqlServer(DatabaseConfiguration.ConnectionString);

            //    });


            services.AddScoped<IBlogDAL, BlogDAL>();
            services.AddScoped<ICategoryDAL, CategoryDAL>();
            services.AddScoped<ISettingDAL, SettingDAL>();
            services.AddScoped<ITagDAL, TagDAL>();
            services.AddScoped<ICourseDAL, CourseDAL>();
            services.AddScoped<ILayoutDAL, LayoutDAL>();


            //services.AddIdentity<User, IdentityRole>(opt =>
            //{
            //    opt.User.RequireUniqueEmail = true;
            //    opt.Lockout.MaxFailedAccessAttempts = 5;
            //    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
            //    opt.SignIn.RequireConfirmedEmail = true;
            //})
            //    .AddEntityFrameworkStores<HelloJobDbContext>()
            //    .AddDefaultTokenProviders();



        }
    }
}
