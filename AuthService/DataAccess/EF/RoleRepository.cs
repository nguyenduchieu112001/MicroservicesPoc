using AuthService.Domain;
using AuthService.Domain.Repository;
using System;
using System.Threading.Tasks;

namespace AuthService.DataAccess.EF
{
    public class RoleRepository : IRoleRepository
    {
        private readonly InsuranceAgentDbContext dbContext;
        public RoleRepository(InsuranceAgentDbContext context)
        {
            dbContext = context ?? throw new ArgumentNullException(nameof(context));
        }
        public Task Add(Role role)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Role> FindByLogin(string name)
        {
            return await dbContext.Roles.FindAsync(name.ToUpper());
        }
    }
}
