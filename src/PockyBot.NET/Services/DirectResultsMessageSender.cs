using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core.Messages;
using PockyBot.NET.Models;

namespace PockyBot.NET.Services
{
    internal class DirectResultsMessageSender : IDirectResultsMessageSender
    {
        private readonly IMessageHandler _messageHandler;

        public DirectResultsMessageSender(IMessageHandler messageHandler)
        {
            _messageHandler = messageHandler;
        }

        public async Task SendDirectMessagesAsync(List<PegRecipient> recipients)
        {
            foreach (var recipient in recipients)
            {
                await SendDirectMessageAsync(recipient);
            }
        }

        private async Task SendDirectMessageAsync(PegRecipient recipient)
        {
            var message =
                $"You have received {recipient.PegCount} pegs and {recipient.PenaltyCount} penalties this cycle, for a total of {recipient.TotalPoints} points:\n\n";
            message += string.Join("\n", recipient.Pegs.Select(FormatPeg));

            await _messageHandler.SendMessageAsync(new Message
            {
                Text = message,
                RoomId = recipient.UserId
            });
        }

        private static string FormatPeg(PegDetails peg)
        {
            var location = peg.SenderLocation != null ? peg.SenderLocation : "No Location";
            return $"* **{peg.SenderName}** ({location}, {peg.Weight} point(s)) — \"_{peg.Comment}_\"";
        }
    }
}
