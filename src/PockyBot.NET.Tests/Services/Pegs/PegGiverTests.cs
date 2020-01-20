using System.Threading.Tasks;
using GlobalX.ChatBots.Core;
using GlobalX.ChatBots.Core.Messages;
using NSubstitute;
using PockyBot.NET.Persistence.Models;
using PockyBot.NET.Persistence.Repositories;
using PockyBot.NET.Services.Pegs;
using PockyBot.NET.Tests.TestData.Pegs;
using TestStack.BDDfy;
using Xunit;

namespace PockyBot.NET.Tests.Services.Pegs
{
    public class PegGiverTests
    {
        private readonly PegGiver _subject;

        private readonly IPegRepository _pegRepository;
        private readonly IChatHelper _chatHelper;

        private string _comment;
        private PockyUser _sender;
        private PockyUser _receiver;
        private int _numPegsGiven;

        public PegGiverTests()
        {
            _pegRepository = Substitute.For<IPegRepository>();
            _chatHelper = Substitute.For<IChatHelper>();
            _chatHelper.Messages.Returns(Substitute.For<IMessageHandler>());
            _subject = new PegGiver(_pegRepository, _chatHelper);
        }

        [Theory]
        [MemberData(nameof(PegGiverTestData.ValidTestData), MemberType = typeof(PegGiverTestData))]
        internal void TestGivePeg(string comment, PockyUser sender, PockyUser receiver, int numPegsGiven,
            Peg peg, Message senderPm, Message receiverPm)
        {
            this.Given(x => GivenAComment(comment))
                .And(x => GivenASender(sender))
                .And(x => GivenAReceiver(receiver))
                .And(x => GivenNumPegsGiven(numPegsGiven))
                .When(x => WhenGivingAPeg())
                .Then(x => ThenItShouldCallCreatePeg(peg))
                .And(x => ThenItShouldPmTheSender(senderPm))
                .And(x => ThenItShouldPmTheReceiver(receiverPm))
                .BDDfy();
        }

        private void GivenAComment(string comment)
        {
            _comment = comment;
        }

        private void GivenASender(PockyUser sender)
        {
            _sender = sender;
        }

        private void GivenAReceiver(PockyUser receiver)
        {
            _receiver = receiver;
        }

        private void GivenNumPegsGiven(int numPegsGiven)
        {
            _numPegsGiven = numPegsGiven;
        }

        private async Task WhenGivingAPeg()
        {
            await _subject.GivePeg(_comment, _sender, _receiver, _numPegsGiven);
        }

        private void ThenItShouldCallCreatePeg(Peg peg)
        {
            _pegRepository.Received(1).CreatePeg(Arg.Is<Peg>(x =>
                x.SenderId == peg.SenderId && x.ReceiverId == peg.ReceiverId && x.Comment == peg.Comment));
        }

        private void ThenItShouldPmTheSender(Message message)
        {
            _chatHelper.Messages.Received(1).SendMessageAsync(Arg.Is<Message>(x =>
                x.Text == message.Text && x.RoomId == message.RoomId));
        }

        private void ThenItShouldPmTheReceiver(Message message)
        {
            _chatHelper.Messages.Received(1).SendMessageAsync(Arg.Is<Message>(x =>
                x.Text == message.Text && x.RoomId == message.RoomId));
        }
    }
}
