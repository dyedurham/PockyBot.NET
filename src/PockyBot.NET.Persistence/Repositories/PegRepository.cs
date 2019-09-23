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

        public Peg CreatePeg(Peg peg)
        {
            var added = _context.Add(peg);
            _context.SaveChanges();
            return added.Entity;
        }
    }
}
