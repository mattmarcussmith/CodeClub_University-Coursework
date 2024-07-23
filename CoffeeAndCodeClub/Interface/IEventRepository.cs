using WeCodeCoffee.Models;

namespace WeCodeCoffee.Interface
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAllEventsAsync();
        Task<Event> GetEventByIdAsync(int id);
        Task<Event> GetEventByIdAsyncNoTracking(int id);
        Task<IEnumerable<Event>> GetEventsByCityAsync(string city);
        bool Add(Event techEvent);  
        bool Update(Event techEvent);
        bool Delete(Event techEvent);
        bool Save();
    }
}
