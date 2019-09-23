using GlobalX.ChatBots.Core.Messages;
using PockyBot.NET.Services.Triggers;

namespace PockyBot.NET.Services
{
    internal interface ITriggerResponseTester
    {
        bool ShouldTriggerInRoom(Message message, ITrigger trigger);
        bool ShouldTriggerInDirectMessage(Message message, ITrigger trigger);
    }
}
