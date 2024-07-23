using Microsoft.EntityFrameworkCore;
using WeCodeCoffee.Data;
using WeCodeCoffee.Interface;
using WeCodeCoffee.Models;

namespace WeCodeCoffee.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        { 
            _context = context;
        }
        public async Task<IList<AppUser>> SearchUsersAsync(string searchTerm)
        {
            return await _context.Users
                                 .Where(u => u.UserName.Contains(searchTerm.ToLower()))
                                 .Include(u => u.Address)
                                 .ToListAsync();
        }
        public async Task<IList<AppUser>> GetAllUsersAsync()
        {
            return await _context.Users.Include(u => u.Address).ToListAsync();
        }

        public async Task<AppUser> GetUserByIdAsync(string id)
        {
            return await _context.Users
                                 .Include(u => u.Address)
                                 .FirstOrDefaultAsync(u => u.Id == id);
        }

        public bool AddUser(AppUser user)
        {
            _context.Add(user);
            return Save();
            
        }
        public bool Update(AppUser user)
        {
            _context.Update(user);
            return Save();
        }

        public bool Delete(AppUser user)
        {
            _context.Remove(user);
            return Save();
        }
        public bool Save()
        {
           return _context.SaveChanges() >= 0 ? true : false;
        }

    
    }
}
