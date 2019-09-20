using GlobalX.ChatBots.Core.Messages;

namespace PockyBot.NET.Services.Triggers
{
    internal interface ITrigger
    {
        bool ShouldTriggerInRoom(Message message);
        bool ShouldTriggerInDirectMessage(Message message);
        Message Respond(Message message);
    }
}
