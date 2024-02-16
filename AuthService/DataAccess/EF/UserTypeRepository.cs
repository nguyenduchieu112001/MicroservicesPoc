using AuthService.Domain;
using AuthService.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AuthService.DataAccess.EF
{
    public class UserTypeRepository : IUserTypeRepository
    {
        private readonly InsuranceAgentDbContext dbContext;
        public UserTypeRepository(InsuranceAgentDbContext context)
        {
            dbContext = context ?? throw new ArgumentNullException(nameof(context));
        }
        public Task Add(UserType userType)
        {
            throw new System.NotImplementedException();
        }

        public async Task<UserType> FindByLogin(string name)
        {
            return await dbContext.UserTypes.FirstOrDefaultAsync(p => p.Name.Equals(name.ToUpper()));
        }
    }
}
