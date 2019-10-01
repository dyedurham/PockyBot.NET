using System.Threading.Tasks;
using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Persistence.Repositories
{
    public class PegRepository : IPegRepository
    {
        private readonly DatabaseContext _context;

        public PegRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Peg> CreatePeg(Peg peg)
        {
            var added = _context.Add(peg);
            await  _context.SaveChangesAsync();
            return added.Entity;
        }
    }
}
