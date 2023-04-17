using System.Threading.Tasks;
using GlobalX.ChatBots.Core.Messages;
using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Services.Triggers
{
    internal interface ITrigger
    {
        string Command { get; }
        bool DirectMessageAllowed { get; }
        bool CanHaveArgs { get; }
        Role[] Permissions { get; }
        Task<Message> Respond(Message message);
        string GetHelpMessage(string botName, PockyUser user);
    }
}
