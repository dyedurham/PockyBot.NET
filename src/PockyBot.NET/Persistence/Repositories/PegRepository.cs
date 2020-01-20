using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Persistence.Repositories
{
    internal class PegRepository : IPegRepository
    {
        private readonly DatabaseContext _context;

        public PegRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Peg> CreatePeg(Peg peg)
        {
            var added = _context.Add(peg);
            await  _context.SaveChangesAsync().ConfigureAwait(false);
            return added.Entity;
        }

        public async Task ClearPegs()
        {
            await _context.Database.ExecuteSqlCommandAsync("DELETE FROM pegs;");
        }
    }
}
