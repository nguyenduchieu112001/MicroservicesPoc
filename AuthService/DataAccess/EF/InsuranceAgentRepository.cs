using AuthService.Domain;
using AuthService.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AuthService.DataAccess.EF;

public class InsuranceAgentRepository : IInsuranceAgentRepository
{
    private readonly InsuranceAgentDbContext dbContext;
    public InsuranceAgentRepository(InsuranceAgentDbContext context)
    {
        dbContext = context ?? throw new ArgumentNullException(nameof(context));
    }
    public async Task Add(InsuranceAgent agent)
    {
        await dbContext.InsuranceAgents.AddAsync(agent);
        await dbContext.SaveChangesAsync();
    }

    public async Task<InsuranceAgent> FindByLogin(string login)
    {
        return await dbContext
            .InsuranceAgents
            .Include(x => x.AgentTypes).ThenInclude(at => at.UserType)
            .Include(x => x.AgentRoles).ThenInclude(ar => ar.Role)

            .FirstOrDefaultAsync(a => a.Login.Equals(login));
    }

    public async Task<Role> GetRole(string name)
    {
        return await dbContext.Roles.FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task<UserType> GetUserType(string name)
    {
        return await dbContext.UserTypes.FirstOrDefaultAsync(x => x.Name == name);
    }
}
