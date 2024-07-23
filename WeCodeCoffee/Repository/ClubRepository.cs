using Microsoft.EntityFrameworkCore;
using WeCodeCoffee.Data;
using WeCodeCoffee.Interface;
using WeCodeCoffee.Models;

namespace WeCodeCoffee.Repository
{
    public class ClubRepository : IClubRepository
    {

        private readonly ApplicationDbContext _context;

        public ClubRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Add(Club club)
        {
            _context.Clubs.Add(club);
            return Save();
        }
        public bool Update(Club club)
        {
            _context.Clubs.Update(club);
            return Save();
        }

        public bool Delete(Club club)
        {
            _context.Remove(club);
            return Save();
        }

        public async Task<IEnumerable<Club>> GetAllClubsAsync()
        {
           return await _context.Clubs.ToListAsync();
           
        }

        public async Task<IEnumerable<Club>> GetClubsByCityAsync(string city)
        {
            return await _context
                                 .Clubs
                                 .Where(c => c.Address.City == city)
                                 .ToListAsync();
        }

        public async Task<Club> GetClubByIdAsync(int id)
        {
            return await _context.Clubs
                                 .Include(a => a.Address)
                                 .FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<Club> GetClubByIdAsyncNoTracking(int id)
        {
            return await _context.Clubs
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
