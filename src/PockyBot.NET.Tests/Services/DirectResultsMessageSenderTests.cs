using System.Collections.Generic;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core.Messages;
using NSubstitute;
using PockyBot.NET.Models;
using PockyBot.NET.Services;
using PockyBot.NET.Tests.TestData;
using TestStack.BDDfy;
using Xunit;

namespace PockyBot.NET.Tests.Services
{
    public class DirectResultsMessageSenderTests
    {
        private readonly IMessageHandler _messageHandler;

        private readonly DirectResultsMessageSender _subject;

        private List<PegRecipient> _pegRecipients;

        public DirectResultsMessageSenderTests()
        {
            _messageHandler = Substitute.For<IMessageHandler>();
            _subject = new DirectResultsMessageSender(_messageHandler);
        }

        [Theory]
        [MemberData(nameof(DirectResultsMessageSenderTestData.SendDirectMessagesTestData), MemberType = typeof(DirectResultsMessageSenderTestData))]
        internal void TestSendDirectMessages(List<PegRecipient> pegRecipients, List<Message> messages)
        {
            this.Given(x => GivenAListOfPegRecipients(pegRecipients))
                .When(x => WhenSendingDirectMessages())
                .Then(x => ThenItShouldSendDirectMessages(messages))
                .BDDfy();
        }

        private void GivenAListOfPegRecipients(List<PegRecipient> pegRecipients)
        {
            _pegRecipients = pegRecipients;
        }

        private async Task WhenSendingDirectMessages()
        {
            await _subject.SendDirectMessagesAsync(_pegRecipients);
        }

        private void ThenItShouldSendDirectMessages(List<Message> messages)
        {
            _messageHandler.Received(messages.Count).SendMessageAsync(Arg.Any<Message>());
            foreach (var message in messages)
            {
                _messageHandler.Received(1)
                    .SendMessageAsync(Arg.Is<Message>(x => x.Text == message.Text && x.RoomId == message.RoomId));
            }
        }
    }
}
