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
            services.AddHttpContextAccessor();


        }
    }
}
