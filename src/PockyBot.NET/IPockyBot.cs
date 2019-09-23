using GlobalX.ChatBots.Core.Messages;

namespace PockyBot.NET
{
    public interface IPockyBot
    {
        void Respond(Message message);
    }
}
