using System.Threading.Tasks;

namespace AuthService.Domain.Repository
{
    public interface IInsuranceAgentUserTypeRepository
    {
        Task Add(InsuranceAgentUserTypes agentTypes);

        Task<InsuranceAgentUserTypes> FindByLogin(InsuranceAgentUserTypes agentTypes);
    }
}
