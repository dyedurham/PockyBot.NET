using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Persistence.Repositories
{
    internal class PockyUserRepository : IPockyUserRepository
    {
        private readonly DatabaseContext _context;

        public PockyUserRepository(DatabaseContext context)
        {
            _context = context;
        }

        public PockyUser GetUser(string userId)
        {
            return _context.PockyUsers
                .Where(x => x.UserId == userId)
                .Include(x => x.PegsGiven)
                    .ThenInclude(x => x.Receiver)
                .Include(x => x.PegsReceived)
                .Include(x => x.Location)
                .Include(x => x.Roles)
                .SingleOrDefault();
        }

        public List<PockyUser> GetUsersByUsername(string username)
        {
            return _context.PockyUsers
                .Where(x => x.Username == username)
                .Include(x => x.PegsGiven)
                    .ThenInclude(x => x.Receiver)
                .Include(x => x.PegsReceived)
                .Include(x => x.Location)
                .Include(x => x.Roles)
                .ToList();
        }

        public PockyUser AddOrUpdateUser(string userId, string username)
        {
            var existingUser = _context.PockyUsers
                .Include(x => x.PegsGiven)
                .Include(x => x.Roles)
                .SingleOrDefault(x => x.UserId == userId);

            if (existingUser != null)
            {
                existingUser.Username = username;
                _context.SaveChanges();
                return existingUser;
            }

            PockyUser newPockyUser = new PockyUser
            {
                UserId = userId,
                Username = username
            };
            _context.Add(newPockyUser);
            _context.SaveChanges();

            return newPockyUser;
        }

        public List<PockyUser> GetAllUsersWithPegs()
        {
            return _context.PockyUsers
                .Include(x => x.PegsGiven)
                    .ThenInclude(x => x.Receiver)
                        .ThenInclude(x => x.Location)
                .Include(x => x.PegsReceived)
                    .ThenInclude(x => x.Sender)
                        .ThenInclude(x => x.Location)
                .Include(x => x.Location)
                .Include(x => x.Roles)
                .Where(x => x.PegsGiven.Any() || x.PegsReceived.Any()).ToList();
        }

        public async Task RemoveUser(PockyUser user)
        {
            var userLocations = _context.UserLocations.Where(x => x.UserId == user.UserId);
            _context.UserLocations.RemoveRange(userLocations);

            _context.PockyUsers.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
