using HelloJob.Service.Services.Implementations;
using HelloJob.Service.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Service.DependencyResolver
{
    public static class ServiceLayerServiceRegistration
    {
        public static void ServiceLayerServiceRegister(this IServiceCollection services)
        {
            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ISettingService,SettingService>();
            services.AddScoped<ILayoutService,LayoutService>();
            services.AddScoped<ICourseService,CourseService>();
            services.AddScoped<ITagService,TagService>();
            services.AddScoped<IEducationService,EducationService>();
            services.AddScoped<ILanguageService,LanguageService>();
            services.AddScoped<IExperienceService,ExperienceService>();
            services.AddScoped<ICityService,CityService>();
            services.AddHttpContextAccessor();


        }
    }
}
