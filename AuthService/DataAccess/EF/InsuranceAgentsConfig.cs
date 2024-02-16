using AuthService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.DataAccess.EF;

internal class InsuranceAgentsConfig : IEntityTypeConfiguration<InsuranceAgent>
{
    public void Configure(EntityTypeBuilder<InsuranceAgent> builder)
    {
        builder.ToTable(nameof(InsuranceAgent));
        builder.HasKey(q => q.Id);
        builder.Property(q => q.Id)
            .UseIdentityColumn();

        builder.Property(q => q.Login).IsRequired();
        builder.Property(q => q.Password).IsRequired();
        builder.Property(q => q.Avatar).IsRequired();

        //builder.HasOne(q => q.UserType).WithMany(q => q.InsuranceAgents).HasForeignKey(q => q.UserTypeId);
    }
}

internal class UserTypeConfig : IEntityTypeConfiguration<UserType>
{
    public void Configure(EntityTypeBuilder<UserType> builder)
    {
        builder.ToTable(nameof(UserType));
        builder.HasKey(q => q.Id);
        builder.Property(q => q.Id)
            .UseIdentityColumn();

        builder.Property(q => q.Name).IsRequired();
    }
}

internal class InsuranceAgentUserTypesConfig : IEntityTypeConfiguration<InsuranceAgentUserTypes>
{
    public void Configure(EntityTypeBuilder<InsuranceAgentUserTypes> builder)
    {
        builder.ToTable(nameof(InsuranceAgentUserTypes));
        builder.HasKey(q => q.Id);
        builder.Property(q => q.Id).UseIdentityColumn();

        builder.HasOne(q => q.InsuranceAgent)
            .WithMany(q => q.AgentTypes)
            .HasForeignKey(q => q.AgentId);

        builder.HasOne(q => q.UserType)
            .WithMany(q => q.AgentTypes)
            .HasForeignKey(q => q.UserTypeId);
    }
}

internal class InsuranceAgentRolesConfig : IEntityTypeConfiguration<InsuranceAgentRoles>
{
    public void Configure(EntityTypeBuilder<InsuranceAgentRoles> builder)
    {
        builder.ToTable(nameof(InsuranceAgentRoles));
        builder.HasKey(q => q.Id);
        builder.Property(q => q.Id)
            .UseIdentityColumn();

        builder.HasOne(q => q.InsuranceAgent)
            .WithMany(q => q.AgentRoles)
            .HasForeignKey(q => q.AgentId);

        builder.HasOne(q => q.Role)
               .WithMany(q => q.AgentRoles)
               .HasForeignKey(q => q.RoleId);
    }
}

internal class RoleConfig : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable(nameof(Role));
        builder.HasKey(q => q.Id);
        builder.Property(q => q.Id)
            .UseIdentityColumn();

        builder.Property(q => q.Name).IsRequired();
    }
}