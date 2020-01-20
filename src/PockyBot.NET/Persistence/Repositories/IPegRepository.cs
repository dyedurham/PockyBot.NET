using System.Threading.Tasks;
using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Persistence.Repositories
{
    internal interface IPegRepository
    {
        Task<Peg> CreatePeg(Peg peg);
        Task ClearPegs();
    }
}
