using AuthService.Domain;
using AuthService.Domain.Repository;
using System;
using System.Threading.Tasks;

namespace AuthService.DataAccess.EF
{
    public class InsuranceAgentRoleRepository : IInsuranceAgentRoleRepository
    {
        private readonly InsuranceAgentDbContext dbContext;
        public InsuranceAgentRoleRepository(InsuranceAgentDbContext context)
        {
            dbContext = context ?? throw new ArgumentNullException(nameof(context));
        }
        public Task Add(InsuranceAgentRoles agentRoles)
        {
            throw new System.NotImplementedException();
        }

        public async Task<InsuranceAgentRoles> FindByLogin(InsuranceAgentRoles agentRoles)
        {
            return await dbContext.InsuranceAgentRoles.FindAsync(agentRoles);
        }
    }
}
