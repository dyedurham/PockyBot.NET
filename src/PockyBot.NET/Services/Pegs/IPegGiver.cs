using System.Threading.Tasks;
using GlobalX.ChatBots.Core.Messages;
using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Services.Pegs
{
    internal interface IPegGiver
    {
        Task<Message> GivePeg(string comment, PockyUser sender, PockyUser receiver, int numPegsGiven);
    }
}
