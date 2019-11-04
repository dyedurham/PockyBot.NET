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

        public async Task GivePeg(string comment, PockyUser sender, PockyUser receiver, int numPegsGiven)
        {
            var peg = new Peg
            {
                SenderId = sender.UserId,
                ReceiverId = receiver.UserId,
                Comment = comment
            };
            await _pegRepository.CreatePeg(peg).ConfigureAwait(false);

            await PmSender(sender, receiver, numPegsGiven).ConfigureAwait(false);
            await PmReceiver(comment, sender, receiver).ConfigureAwait(false);
        }

        private async Task PmSender(PockyUser sender, PockyUser receiver, int numPegsGiven)
        {
            var pegs = numPegsGiven == 1 ? "peg" : "pegs";
            await _chatHelper.Messages.SendMessageAsync(new Message
            {
                Text = $"Peg given to {receiver.Username}. You have given {numPegsGiven} {pegs} this fortnight.",
                RoomId = sender.UserId
            }).ConfigureAwait(false);
        }

        private async Task PmReceiver(string comment, PockyUser sender, PockyUser receiver)
        {
            await _chatHelper.Messages.SendMessageAsync(new Message
            {
                Text = $"You have received a new peg from {sender.Username} with message: \"{comment}\".",
                RoomId = receiver.UserId
            }).ConfigureAwait(false);
        }
    }
}
