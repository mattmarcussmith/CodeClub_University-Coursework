using Microsoft.AspNetCore.Mvc;
using WeCodeCoffee.Models;

namespace WeCodeCoffee.Interface
{
    public interface IDashboardRepository
    {
        Task<IList<Club>> GetAllUserClubsAsync();
        Task<IList<Event>> GetAllUserEventsAsync();
        Task<AppUser> GetUserByIdAsync(string userId);
        Task<AppUser> GetUserByIdNoTrackingAsync(string userId);
        bool Update(AppUser user);
        bool Save();
    }
}
