using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Persistence.Repositories
{
    public interface IPegRepository
    {
        Peg CreatePeg(Peg peg);
    }
}
