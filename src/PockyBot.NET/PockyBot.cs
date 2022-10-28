using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core;
using GlobalX.ChatBots.Core.Messages;
using GlobalX.ChatBots.Core.People;
using GlobalX.ChatBots.Core.Rooms;
using Microsoft.Extensions.Logging;
using PockyBot.NET.Services;
using PockyBot.NET.Services.Triggers;

namespace PockyBot.NET
{
    internal class PockyBot : IPockyBot
    {
        private readonly IEnumerable<ITrigger> _triggers;
        private readonly ITriggerResponseTester _triggerResponseTester;
        private readonly IChatHelper _chatHelper;
        private readonly ILogger<PockyBot> _logger;

        public PockyBot(IEnumerable<ITrigger> triggers, ITriggerResponseTester triggerResponseTester, IChatHelper chatHelper, ILogger<PockyBot> logger)
        {
            _triggers = triggers;
            _triggerResponseTester = triggerResponseTester;
            _chatHelper = chatHelper;
            _logger = logger;
        }

        public async Task Respond(Message message)
        {
            _logger.LogInformation("Received message: {@message}", message);
            ITrigger responder = null;

            if (message.Sender.Type == PersonType.Bot)
            {
                _logger.LogDebug("Message was sent by a bot, ignoring");
                return;
            }

            if (message.RoomType == RoomType.Group)
            {
                responder = _triggers.FirstOrDefault(x => _triggerResponseTester.ShouldTriggerInRoom(message, x));
            }
            else if (message.RoomType == RoomType.Direct)
            {
                responder = _triggers.FirstOrDefault(x => _triggerResponseTester.ShouldTriggerInDirectMessage(message, x));
            }

            Message response = null;
            if (responder != null)
            {
                if (message.RoomType == RoomType.Direct && !responder.DirectMessageAllowed)
                {
                    _logger.LogInformation("Responder does not allow direct messages. Responding with error.");
                    response = new Message
                    {
                        Text =
                            $"Command {responder.Command} is not allowed to be called in a direct message. Please try again in a room."
                    };
                }
                else
                {
                    _logger.LogInformation("Responding to message with responder {responderName}",
                        responder.GetType().Name);
                    response = await responder.Respond(message).ConfigureAwait(false);
                }
            }
            else
            {
                _logger.LogInformation("No responder found");
            }

            if (response != null)
            {
                _logger.LogInformation("Sending response message {@response}", response);
                if (response.RoomId == null)
                {
                    response.RoomId = message.RoomId;
                    response.ParentId = message.Id;
                }

                await _chatHelper.Messages.SendMessageAsync(response).ConfigureAwait(false);
            }
        }
    }
}
