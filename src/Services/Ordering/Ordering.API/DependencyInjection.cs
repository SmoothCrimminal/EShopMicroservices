using BuildingBlocks.Exceptions.Handler;
using BuildingBlocks.Extensions;
using HealthChecks.UI.Client;

namespace Ordering.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCarter(null, cfg =>
            {
                cfg.LoadAllModules();
            });

            services.AddExceptionHandler<CustomExceptionHandler>();
            services.AddHealthChecks()
                .AddSqlServer(configuration.GetConnectionString("Database")!);

            return services;
        }

        public static WebApplication UseApiServices(this WebApplication app)
        {
            app.MapCarter();

            app.UseExceptionHandler(opt => { });
            app.UseHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            return app;
        }
    }
}
