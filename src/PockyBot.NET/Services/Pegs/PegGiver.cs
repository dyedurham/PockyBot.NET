using System;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core;
using GlobalX.ChatBots.Core.Messages;
using PockyBot.NET.Persistence.Models;
using PockyBot.NET.Persistence.Repositories;

namespace PockyBot.NET.Services.Pegs
{
    internal class PegGiver : IPegGiver
    {
        private readonly IPegRepository _pegRepository;
        private readonly IChatHelper _chatHelper;

        public PegGiver(IPegRepository pegRepository, IChatHelper chatHelper)
        {
            _pegRepository = pegRepository;
            _chatHelper = chatHelper;
        }

        public async Task<Message> GivePeg(string comment, PockyUser sender, PockyUser receiver, int numPegsGiven)
        {
            var peg = new Peg
            {
                Sender = sender.UserId,
                Receiver = receiver.UserId,
                Comment = comment
            };
            _pegRepository.CreatePeg(peg);

            await PmSender(sender, receiver, numPegsGiven);
            await PmReceiver(comment, sender, receiver);

            return null;
        }

        private async Task PmSender(PockyUser sender, PockyUser receiver, int numPegsGiven)
        {
            var pegs = numPegsGiven == 1 ? "peg" : "pegs";
            await _chatHelper.Messages.SendMessageAsync(new Message
            {
                Text = $"Peg given to {receiver.Username}. You have given {numPegsGiven} {pegs} this fortnight.",
                RoomId = sender.UserId
            });
        }

        private async Task PmReceiver(string comment, PockyUser sender, PockyUser receiver)
        {
            var forString = comment.StartsWith("for ", StringComparison.InvariantCultureIgnoreCase)
                ? string.Empty
                : " for";
            await _chatHelper.Messages.SendMessageAsync(new Message
            {
                Text = $"You have received a new peg from {sender.Username}{forString}: \"{comment}\".",
                RoomId = receiver.UserId
            });
        }
    }
}