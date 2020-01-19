using System.Threading.Tasks;
using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Services.Pegs
{
    internal interface IPegGiver
    {
        Task GivePeg(string comment, PockyUser sender, PockyUser receiver, int numPegsGiven);
    }
}
