using AuthService.Domain;
using AuthService.Domain.Repository;
using System.Threading.Tasks;

namespace AuthService.DataAccess.EF
{
    public class InsuranceAgentUserTypeRepository : IInsuranceAgentUserTypeRepository
    {
        private readonly InsuranceAgentDbContext _context;
        public InsuranceAgentUserTypeRepository(InsuranceAgentDbContext context)
        {
            _context = context;
        }

        public Task Add(InsuranceAgentUserTypes agentTypes)
        {
            throw new System.NotImplementedException();
        }

        public async Task<InsuranceAgentUserTypes> FindByLogin(InsuranceAgentUserTypes agentTypes)
        {
            return await _context.InsuranceAgentUserTypes.FindAsync(agentTypes);
        }
    }
}
