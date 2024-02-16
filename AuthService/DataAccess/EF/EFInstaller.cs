using AuthService.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.DataAccess.EF;

public static class EFInstaller
{
    public static IServiceCollection AddEFConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<InsuranceAgentDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("Products"));
        });
        services.AddScoped<IInsuranceAgentRepository, InsuranceAgentRepository>();
        services.AddScoped<IUserTypeRepository, UserTypeRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IInsuranceAgentRoleRepository, InsuranceAgentRoleRepository>();
        services.AddScoped<IInsuranceAgentUserTypeRepository, InsuranceAgentUserTypeRepository>();
        return services;
    }
}
