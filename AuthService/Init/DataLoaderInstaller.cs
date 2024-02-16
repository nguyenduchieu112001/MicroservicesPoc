using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Init;

public static class DataLoaderInstaller
{
    public static IServiceCollection AddInsuranceAgentDemoInitializer(this IServiceCollection services)
    {
        services.AddScoped<DataLoader>();
        return services;
    }
}
