using System.Collections.Generic;
using System.Linq;
using GlobalX.ChatBots.Core.Messages;
using GlobalX.ChatBots.Core.Rooms;
using PockyBot.NET.Services.Triggers;

namespace PockyBot.NET
{
    internal class PockyBot : IPockyBot
    {
        private readonly IEnumerable<ITrigger> _triggers;

        public PockyBot(IEnumerable<ITrigger> triggers)
        {
            _triggers = triggers;
        }

        public void Respond(Message message)
        {
            ITrigger responder = null;

            if (message.RoomType == RoomType.Group)
            {
                responder = _triggers.FirstOrDefault(x => x.ShouldTriggerInRoom(message));
            }
            else if (message.RoomType == RoomType.Direct)
            {
                responder = _triggers.FirstOrDefault(x => x.ShouldTriggerInDirectMessage(message));
            }

            if (responder != null)
            {
                responder.Respond(message);
            }
        }
    }
}
