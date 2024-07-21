using System.Diagnostics.CodeAnalysis;
using UMS.Application;
using UMS.Application.Repositories;
using UMS.Infrastructure;
using UMS.Infrastructure.Data;
using UMS.Infrastructure.Interface;

namespace UMS.Infrastracture
{
    [ExcludeFromCodeCoverage]
    public static class InfrastructureService
    {
        public static IServiceCollection AddInfrastructureServices
            (
            this IServiceCollection services,
            ConfigurationManager configuration,
            ILogger logger)
        {
            // register services
            services.AddDbContext<AppDbContext>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            logger.LogInformation("{Project} service registered", "Infrastructure");
            return services;
        }
    }
}
