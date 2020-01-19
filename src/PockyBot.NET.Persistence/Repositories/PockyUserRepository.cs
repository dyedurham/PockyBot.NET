using System.Collections.Generic;
using System.Linq;
using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Persistence.Repositories
{
    public class PockyUserRepository : IPockyUserRepository
    {
        private readonly DatabaseContext _context;

        public PockyUserRepository(DatabaseContext context)
        {
            _context = context;
        }

        public PockyUser GetUser(string userId)
        {
            return _context.PockyUsers.SingleOrDefault(x => x.UserId == userId);
        }

        public PockyUser AddOrUpdateUser(string userId, string username)
        {
            var existingUser = _context.PockyUsers.SingleOrDefault(x => x.UserId == userId);

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
            return _context.PockyUsers.Where(x => x.PegsGiven.Any() || x.PegsReceived.Any()).ToList();
        }
    }
}
