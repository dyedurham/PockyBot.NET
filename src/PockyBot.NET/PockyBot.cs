using System.Collections.Generic;
using System.Linq;
using GlobalX.ChatBots.Core.Messages;
using GlobalX.ChatBots.Core.Rooms;
using PockyBot.NET.Services;
using PockyBot.NET.Services.Triggers;

namespace PockyBot.NET
{
    internal class PockyBot : IPockyBot
    {
        private readonly IEnumerable<ITrigger> _triggers;
        private readonly ITriggerResponseTester _triggerResponseTester;

        public PockyBot(IEnumerable<ITrigger> triggers, ITriggerResponseTester triggerResponseTester)
        {
            _triggers = triggers;
            _triggerResponseTester = triggerResponseTester;
        }

        public void Respond(Message message)
        {
            ITrigger responder = null;

            if (message.RoomType == RoomType.Group)
            {
                responder = _triggers.FirstOrDefault(x => _triggerResponseTester.ShouldTriggerInRoom(message, x));
            }
            else if (message.RoomType == RoomType.Direct)
            {
                responder = _triggers.FirstOrDefault(x => _triggerResponseTester.ShouldTriggerInDirectMessage(message, x));
            }

            if (responder != null)
            {
                responder.Respond(message);
            }
        }
    }
}
