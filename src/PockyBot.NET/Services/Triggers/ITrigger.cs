using System.Threading.Tasks;
using GlobalX.ChatBots.Core.Messages;

namespace PockyBot.NET.Services.Triggers
{
    internal interface ITrigger
    {
        string Command { get; }
        bool DirectMessageAllowed { get; }
        bool CanHaveArgs { get; }
        string[] Permissions { get; }
        Task<Message> Respond(Message message);
    }
}
