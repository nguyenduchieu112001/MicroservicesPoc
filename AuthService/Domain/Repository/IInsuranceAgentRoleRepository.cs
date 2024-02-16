using System.Threading.Tasks;

namespace AuthService.Domain.Repository
{
    public interface IInsuranceAgentRoleRepository
    {
        Task Add(InsuranceAgentRoles agentRoles);

        Task<InsuranceAgentRoles> FindByLogin(InsuranceAgentRoles agentRoles);
    }
}
