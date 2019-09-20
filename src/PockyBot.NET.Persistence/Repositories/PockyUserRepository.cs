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
    }
}
