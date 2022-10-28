using System.Collections.Generic;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core;
using GlobalX.ChatBots.Core.Messages;
using GlobalX.ChatBots.Core.People;
using GlobalX.ChatBots.Core.Rooms;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PockyBot.NET.Services;
using PockyBot.NET.Services.Triggers;
using TestStack.BDDfy;
using Xunit;

namespace PockyBot.NET.Tests
{
    public class PockyBotTests
    {
        private readonly PockyBot _subject;

        private readonly List<ITrigger> _triggers = new List<ITrigger>();
        private readonly ITriggerResponseTester _triggerResponseTester;
        private readonly IChatHelper _chatHelper;

        private Message _message;

        public PockyBotTests()
        {
            _triggerResponseTester = Substitute.For<ITriggerResponseTester>();
            _chatHelper = Substitute.For<IChatHelper>();
            var messageHandler = Substitute.For<IMessageHandler>();
            _chatHelper.Messages.Returns(messageHandler);

            _subject = new PockyBot(_triggers, _triggerResponseTester, _chatHelper,
                Substitute.For<ILogger<PockyBot>>());
        }

        [Theory]
        [InlineData(RoomType.Direct)]
        [InlineData(RoomType.Group)]
        public void ItShouldIgnoreMessagesSentByBots(RoomType roomType)
        {
            this.Given(x => GivenAMessage(PersonType.Bot, roomType, null, null))
                .When(x => WhenRespondingToAMessage())
                .Then(x => ThenItShouldNotSendAResponse())
                .BDDfy();
        }

        [Theory]
        [InlineData(0, "This is a test message", "room1", "message1")]
        [InlineData(1, "Another test", "room2", "message2")]
        public void ItShouldTriggerInARoom(int triggerIndex, string response, string roomId, string messageId)
        {
            this.Given(x => GivenAMessage(PersonType.Person, RoomType.Group, roomId, messageId))
                .And(x => GivenAListOfTriggers())
                .And(x => GivenATriggerWillRespondInRoom(triggerIndex, response, null, null))
                .When(x => WhenRespondingToAMessage())
                .Then(x => ThenItShouldSendAResponse(response, roomId, messageId))
                .BDDfy();
        }

        [Theory]
        [InlineData(0, "This is a test message", "room1", "message1")]
        [InlineData(1, "Another test", "room2", "message2")]
        public void ItShouldTriggerInADirectMessage(int triggerIndex, string response, string roomId, string messageId)
        {
            this.Given(x => GivenAMessage(PersonType.Person, RoomType.Direct, roomId, messageId))
                .And(x => GivenAListOfTriggers())
                .And(x => GivenATriggerWillRespondInDirectMessage(triggerIndex, response))
                .And(x => GivenATriggerIsAllowedInDirectMessage(triggerIndex, true, "commandName"))
                .When(x => WhenRespondingToAMessage())
                .Then(x => ThenItShouldSendAResponse(response, roomId, messageId))
                .BDDfy();
        }

        [Theory]
        [InlineData(0, "This is a test message", "room1", "command1")]
        [InlineData(1, "Another test", "room2", "command2")]
        private void ItShouldErrorOnDirectMessageNotAllowed(int triggerIndex, string roomId, string messageId, string commandName)
        {
            this.Given(x => GivenAMessage(PersonType.Person, RoomType.Direct, roomId, messageId))
                .And(x => GivenAListOfTriggers())
                .And(x => GivenATriggerWillRespondInDirectMessage(triggerIndex, null))
                .And(x => GivenATriggerIsAllowedInDirectMessage(triggerIndex, false, commandName))
                .When(x => WhenRespondingToAMessage())
                .Then(x => ThenItShouldSendAResponse(
                    $"Command {commandName} is not allowed to be called in a direct message. Please try again in a room.",
                    roomId, messageId))
                .BDDfy();
        }

        [Theory]
        [InlineData(0, "This is a test message", "room1", "command1")]
        [InlineData(1, "Another test", "room2", "command2")]
        private void ItShouldNotOverrideRoomIdSetByTheTrigger(int triggerIndex, string response, string roomId, string messageId)
        {
            this.Given(x => GivenAMessage(PersonType.Person, RoomType.Group, "testRoom", "testMessage"))
                .And(x => GivenAListOfTriggers())
                .And(x => GivenATriggerWillRespondInRoom(triggerIndex, response, roomId, messageId))
                .When(x => WhenRespondingToAMessage())
                .Then(x => ThenItShouldSendAResponse(response, roomId, messageId))
                .BDDfy();
        }

        [Fact]
        private void ItShouldNotRespondIfNoTriggerFound()
        {
            this.Given(x => GivenAMessage(PersonType.Person, RoomType.Group, "roomId", "messageId"))
                .And(x => GivenAListOfTriggers())
                .When(x => WhenRespondingToAMessage())
                .Then(x => ThenItShouldNotSendAResponse())
                .BDDfy();
        }

        private void GivenAMessage(PersonType senderType, RoomType roomType, string roomId, string messageId)
        {
            _message = new Message
            {
                Sender = new Person
                {
                    Type = senderType
                },
                RoomType = roomType,
                RoomId = roomId,
                Id = messageId
            };
        }

        private void GivenAListOfTriggers()
        {
            _triggers.AddRange(new List<ITrigger>
            {
                Substitute.For<ITrigger>(),
                Substitute.For<ITrigger>(),
                Substitute.For<ITrigger>()
            });
        }

        private void GivenATriggerWillRespondInRoom(int index, string response, string roomId, string messageId)
        {
            _triggerResponseTester.ShouldTriggerInRoom(_message, _triggers[index]).Returns(true);
            _triggers[index].Respond(_message)
                .Returns(Task.FromResult(new Message { Text = response, RoomId = roomId, ParentId = messageId }));
        }

        private void GivenATriggerWillRespondInDirectMessage(int index, string response)
        {
            _triggerResponseTester.ShouldTriggerInDirectMessage(_message, _triggers[index]).Returns(true);
            _triggers[index].Respond(_message)
                .Returns(Task.FromResult(new Message { Text = response }));
        }

        private void GivenATriggerIsAllowedInDirectMessage(int index, bool directMessageAllowed, string commandName)
        {
            _triggers[index].DirectMessageAllowed.Returns(directMessageAllowed);
            _triggers[index].Command.Returns(commandName);
        }

        private async Task WhenRespondingToAMessage()
        {
            await _subject.Respond(_message);
        }

        private void ThenItShouldNotSendAResponse()
        {
            _chatHelper.Messages.DidNotReceive().SendMessageAsync(Arg.Any<Message>());
        }

        private void ThenItShouldSendAResponse(string response, string roomId, string parentId)
        {
            _chatHelper.Messages.Received(1).SendMessageAsync(Arg.Is<Message>(x =>
                x.Text == response && x.RoomId == roomId && x.ParentId == parentId));
        }
    }
}
