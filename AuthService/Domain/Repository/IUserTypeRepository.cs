using System.Threading.Tasks;

namespace AuthService.Domain.Repository
{
    public interface IUserTypeRepository
    {
        Task Add(UserType userType);

        Task<UserType> FindByLogin(string name);
    }
}
