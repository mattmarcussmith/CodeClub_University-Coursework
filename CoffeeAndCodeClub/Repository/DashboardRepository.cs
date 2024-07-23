using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeCodeCoffee.Data;
using WeCodeCoffee.Interface;
using WeCodeCoffee.Models;

namespace WeCodeCoffee.Repository
{
    public class DashboardRepository : IDashboardRepository
    {

        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) 
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;

        }
        public async Task<IList<Event>> GetAllUserEventsAsync()
        {
            var currentUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            var userEvent = _context.Events
                                    .Where(a => a.AppUser.Id == currentUser)
                                    .Include(c => c.AppUser);


            return await userEvent.ToListAsync();                             
        }

       
        public async Task<IList<Club>> GetAllUserClubsAsync()
        {
            var currentUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            var userClubs = _context.Clubs
                                    .Where(a => a.AppUser.Id == currentUser)
                                    .Include(c => c.AppUser);

            return await userClubs.ToListAsync();
        }
        public async Task<AppUser> GetUserByIdAsync(string userId)
        {
            return await _context.Users.FindAsync(userId);
        }
        public async Task<AppUser> GetUserByIdNoTrackingAsync(string userId)
        {
            return await _context.Users
                                 .Where(u => u.Id == userId)
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync();
        }

        public bool Update(AppUser user)
        {
            _context.Users.Update(user);
            return Save();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved >= 0 ? true : false;
        }


    }
}
