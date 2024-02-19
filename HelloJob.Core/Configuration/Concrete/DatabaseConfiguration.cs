using Microsoft.Extensions.Configuration;

namespace HelloJob.Core.Configuration.Concrete
{
    public static class DatabaseConfiguration
    {
        public static string ConnectionString { get
            {
                ConfigurationManager configurationManager = new();
                configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../HelloJob.App"));
                configurationManager.AddJsonFile("appsettings.json");
                return configurationManager.GetConnectionString("Default");
            } 
        }
    }
}
