using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core.Messages;
using Microsoft.Extensions.Logging;
using PockyBot.NET.Models;

namespace PockyBot.NET.Services
{
    internal class DirectResultsMessageSender : IDirectResultsMessageSender
    {
        private readonly IMessageHandler _messageHandler;
        private readonly ILogger<DirectResultsMessageSender> _logger;

        public DirectResultsMessageSender(IMessageHandler messageHandler, ILogger<DirectResultsMessageSender> logger)
        {
            _messageHandler = messageHandler;
            _logger = logger;
        }

        public async Task SendDirectMessagesAsync(List<PegRecipient> recipients)
        {
            foreach (var recipient in recipients)
            {
                try
                {
                    await SendDirectMessageAsync(recipient).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error sending message to {recipient.Name}", ex);
                }
            }
        }

        private async Task SendDirectMessageAsync(PegRecipient recipient)
        {
            var pegsPlural = recipient.PegCount == 1 ? string.Empty : "s";
            var penaltiesPlural = recipient.PenaltyCount == 1 ? "y" : "ies";
            var pointsPlural = recipient.TotalPoints == 1 ? string.Empty : "s";
            var message =
                $"You have received {recipient.PegCount} peg{pegsPlural} and {recipient.PenaltyCount} penalt{penaltiesPlural} this cycle, for a total of {recipient.TotalPoints} point{pointsPlural}:\n\n";
            message += string.Join("\n", recipient.Pegs.Select(FormatPeg));

            await _messageHandler.SendMessageAsync(new Message
            {
                Text = message,
                RoomId = recipient.UserId
            });
        }

        private static string FormatPeg(PegDetails peg)
        {
            var location = peg.SenderLocation ?? "No Location";
            var pointsPlural = peg.Weight == 1 ? string.Empty : "s";
            return $"* **{peg.SenderName}** ({location}, {peg.Weight} point{pointsPlural}) — \"_{peg.Comment.Replace("\n\n", " // ").Replace("\n", " / ")}_\"";
        }
    }
}
