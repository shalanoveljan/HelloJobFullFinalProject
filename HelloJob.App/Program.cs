using HelloJob.App.Configurations;
using HelloJob.Data.DBContexts.SQLSERVER;
using HelloJob.Data.ServiceRegistrations;
using HelloJob.Service.DependencyResolver;
using HelloJob.Service.Mappers;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
builder.Services.AddAutoMapper(typeof(GlobalMapping));
builder.Services.DataAccessServiceRegister(builder.Configuration);
builder.Services.ServiceLayerServiceRegister();
var app = builder.Build();
app.AddDefaultConfiguration(app.Environment);
app.Run();
