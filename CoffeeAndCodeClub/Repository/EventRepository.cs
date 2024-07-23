using Microsoft.EntityFrameworkCore;
using WeCodeCoffee.Data;
using WeCodeCoffee.Interface;
using WeCodeCoffee.Models;

namespace WeCodeCoffee.Repository
{


    public class EventRepository : IEventRepository
    {
        private readonly ApplicationDbContext _context;
        public EventRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(Event techEvent)
        {
            _context.Events.Add(techEvent);
            return Save();
        }
        public bool Update(Event techEvent)
        {
            _context.Update(techEvent);
            return Save();
        }

        public bool Delete(Event techEvent)
        {
            _context.Remove(techEvent);
            return Save();
        }

        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            return await _context.Events.ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetEventsByCityAsync(string city)
        {
            return await _context.Events
                           .Where(c => c.Address.City == city)
                           .ToListAsync();
        }

        public async Task<Event> GetEventByIdAsync(int id)
        {
            return await _context.Events
                                 .Include(a => a.Address)
                                 .FirstOrDefaultAsync(c => c.Id == id);
        }
      
        public async Task<Event> GetEventByIdAsyncNoTracking(int id)
        {
            return await _context.Events
                                 .Include(a => a.Address)
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(c => c.Id == id);
        }
        public bool Save()
        {
            return _context.SaveChanges() > 0 ? true : false;
        }
      
    }
}
