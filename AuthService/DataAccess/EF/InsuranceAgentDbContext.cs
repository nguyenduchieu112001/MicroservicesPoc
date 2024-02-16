using AuthService.Domain;
using Microsoft.EntityFrameworkCore;

namespace AuthService.DataAccess.EF;

public class InsuranceAgentDbContext : DbContext
{
    public InsuranceAgentDbContext(DbContextOptions<InsuranceAgentDbContext> options) : base(options)
    {

    }

    public DbSet<InsuranceAgent> InsuranceAgents { get; set; }
    public DbSet<InsuranceAgentRoles> InsuranceAgentRoles { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserType> UserTypes { get; set; }
    public DbSet<InsuranceAgentUserTypes> InsuranceAgentUserTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new InsuranceAgentsConfig());
        modelBuilder.ApplyConfiguration(new InsuranceAgentRolesConfig());
        modelBuilder.ApplyConfiguration(new RoleConfig());
        modelBuilder.ApplyConfiguration(new UserTypeConfig());
        modelBuilder.ApplyConfiguration(new InsuranceAgentUserTypesConfig());
    }
}
