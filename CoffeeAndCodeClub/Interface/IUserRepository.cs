using WeCodeCoffee.Models;

namespace WeCodeCoffee.Interface
{
    public interface IUserRepository
    {
        Task<IList<AppUser>> SearchUsersAsync(string searchTerm);
        Task<IList<AppUser>> GetAllUsersAsync();
        Task<AppUser> GetUserByIdAsync(string id);
        bool AddUser(AppUser user);
        bool Update(AppUser user);
        bool Delete(AppUser user);
        bool Save();

    }
}
