using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Persistence.Repositories
{
    internal class LocationRepository : ILocationRepository
    {
        private readonly DatabaseContext _context;

        public LocationRepository(DatabaseContext context)
        {
            _context = context;
        }

        public string[] GetAllLocations()
        {
            return _context.Locations.Select(x => x.Name).ToArray();
        }

        public async Task AddLocation(string location)
        {
            Location newLocation = new Location
            {
                Name = location
            };

            _context.Locations.Add(newLocation);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLocation(string location)
        {
            var locationToDelete = await _context.Locations.FirstOrDefaultAsync(x => x.Name == location);

            if (locationToDelete != null)
            {
                _context.Locations.Remove(locationToDelete);
                await _context.SaveChangesAsync();
            }
        }
    }
}
