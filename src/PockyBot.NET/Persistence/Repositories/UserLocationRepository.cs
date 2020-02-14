using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Persistence.Repositories
{
    class UserLocationRepository : IUserLocationRepository
    {
        private readonly DatabaseContext _context;

        public UserLocationRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task DeleteUserLocation(PockyUser user)
        {
            await DeleteUserLocation(user.UserId);
        }

        public async Task DeleteUserLocation(string userId)
        {
            var userLocationToDelete = await _context.UserLocations.FirstOrDefaultAsync(x => x.UserId == userId);

            // unclear what this does if userLocationToDelete is null
            _context.UserLocations.Remove(userLocationToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task AddUserLocation(PockyUser user, Location location)
        {
            // TODO: check if all three of these are needed?
            var userLocation = new UserLocation
            {
                Location = location.Name,
                User = user,
                UserId = user.UserId
            };

            _context.UserLocations.Add(userLocation);
            await _context.SaveChangesAsync();
        }
    }
}
