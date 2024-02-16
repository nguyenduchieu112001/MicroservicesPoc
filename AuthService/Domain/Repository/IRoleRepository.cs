using System.Threading.Tasks;

namespace AuthService.Domain.Repository
{
    public interface IRoleRepository
    {
        Task Add(Role role);

        Task<Role> FindByLogin(string name);
    }
}
