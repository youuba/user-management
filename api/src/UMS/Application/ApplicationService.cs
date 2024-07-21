using System.Diagnostics.CodeAnalysis;
using UMS.Application.Common.Interfaces;
using UMS.Application.Services;

namespace UMS.Application
{
    [ExcludeFromCodeCoverage]
    public static class ApplicationService
    {
        public static IServiceCollection AddApplicationServices
       (
       this IServiceCollection services,
       ConfigurationManager configuration,
       ILogger logger)
        {
            // register services
            services.AddScoped<IUserService, UserService>();
            logger.LogInformation("{Project} service registered", "Application");
            return services;
        }
    }
}
