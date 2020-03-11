using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Persistence.Repositories
{
    internal class UserLocationRepository : IUserLocationRepository
    {
        private readonly DatabaseContext _context;

        public UserLocationRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task DeleteUserLocation(string userId)
        {
            var userLocationToDelete = await _context.UserLocations.FirstOrDefaultAsync(x => x.UserId == userId);

            if (userLocationToDelete != null)
            {
                _context.UserLocations.Remove(userLocationToDelete);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpsertUserLocation(PockyUser user, string location)
        {
            var userLocation = await _context.UserLocations.FirstOrDefaultAsync(x =>
                x.UserId == (user != null ? user.UserId : null));

            if (userLocation == null)
            {
                userLocation = new UserLocation
                {
                    Location = location,
                    UserId = user.UserId
                };
                await _context.UserLocations.AddAsync(userLocation);
            }
            else
            {
                userLocation.Location = location;
                _context.UserLocations.Update(userLocation);
            }

            await _context.SaveChangesAsync();
        }
    }
}
