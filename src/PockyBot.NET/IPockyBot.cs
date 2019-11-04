using GlobalX.ChatBots.Core.Messages;
using System.Threading.Tasks;

namespace PockyBot.NET
{
    public interface IPockyBot
    {
        Task Respond(Message message);
    }
}
