using System.Threading.Tasks;

namespace AuthService.Domain.Repository;

public interface IInsuranceAgentRepository
{
    Task Add(InsuranceAgent agent);

    Task<Role> GetRole(string name);
    Task<UserType> GetUserType(string name);

    Task<InsuranceAgent> FindByLogin(string login);
}
