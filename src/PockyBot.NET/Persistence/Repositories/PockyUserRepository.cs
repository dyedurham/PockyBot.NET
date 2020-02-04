using System.Collections.Generic;
using System.Linq;
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
                .Include(x => x.PegsGiven)
                    .ThenInclude(x => x.Receiver)
                .Include(x => x.PegsReceived)
                .Include(x => x.Location)
                .Include(x => x.Roles)
                .SingleOrDefault(x => x.UserId == userId);
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

        public List<PockyUser> GetAllUsersLocations()
        {
            return _context.PockyUsers.Include(x => x.Location).ToList();
        }
    }
}
