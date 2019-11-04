using System.Threading.Tasks;
using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Persistence.Repositories
{
    public interface IPegRepository
    {
        Task<Peg> CreatePeg(Peg peg);
    }
}
