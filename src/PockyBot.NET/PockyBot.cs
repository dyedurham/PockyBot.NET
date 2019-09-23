using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core;
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
        private readonly IChatHelper _chatHelper;

        public PockyBot(IEnumerable<ITrigger> triggers, ITriggerResponseTester triggerResponseTester, IChatHelper chatHelper)
        {
            _triggers = triggers;
            _triggerResponseTester = triggerResponseTester;
            _chatHelper = chatHelper;
        }

        public async Task Respond(Message message)
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

            var response = await responder?.Respond(message) ?? null;
            if (response != null)
            {
                response.RoomId = message.RoomId;
                await _chatHelper.Messages.SendMessageAsync(response);
            }
        }
    }
}
