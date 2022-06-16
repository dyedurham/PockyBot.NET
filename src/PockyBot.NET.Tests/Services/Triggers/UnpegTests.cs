using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core;
using GlobalX.ChatBots.Core.Messages;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PockyBot.NET.Services.Helpers;
using PockyBot.NET.Services.Triggers;
using PockyBot.NET.Tests.TestData.Triggers;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace PockyBot.NET.Tests.Services.Triggers
{
    public class UnpegTests
    {
        private readonly Unpeg _subject;
        private readonly IRandomnessHandler _randomnessHandler;
        private readonly IChatHelper _chatHelper;
        private readonly IAsyncDelayer _asyncDelayer;

        private Message _message;
        private Message _result;

        public UnpegTests()
        {
            _randomnessHandler = Substitute.For<IRandomnessHandler>();
            _randomnessHandler.Random.Returns(Substitute.For<Random>());
            _chatHelper = Substitute.For<IChatHelper>();
            _chatHelper.Messages.Returns(Substitute.For<IMessageHandler>());
            _asyncDelayer = Substitute.For<IAsyncDelayer>();
            _asyncDelayer.Delay(Arg.Any<TimeSpan>(), Arg.Any<CancellationToken>())
                .ReturnsForAnyArgs(Task.CompletedTask);
            var backgroundTaskQueue = Substitute.For<IBackgroundTaskQueue>();
            backgroundTaskQueue
                .WhenForAnyArgs(x => x.QueueBackgroundWorkItem(Arg.Any<Func<CancellationToken, Task>>()))
                .Do(async x => await ((Func<CancellationToken, Task>) x[0])(new CancellationToken()));
            _subject = new Unpeg(_randomnessHandler, _chatHelper, backgroundTaskQueue, _asyncDelayer,
                Substitute.For<ILogger<Unpeg>>());
        }

        [Theory]
        [MemberData(nameof(UnpegTestData.SingleUnpegMessageTestData), MemberType = typeof(UnpegTestData))]
        public void TestUnpegSingleMessage(Message message, int number, Message response)
        {
            this.Given(x => GivenAMessage(message))
                .And(x => GivenTheNextRandomIs(number))
                .When(x => WhenRespondingToAMessage())
                .Then(x => ThenItShouldReturnAMessage(response))
                .BDDfy();
        }

        [Theory]
        [MemberData(nameof(UnpegTestData.DelayedUnpegMessageTestData), MemberType = typeof(UnpegTestData))]
        public void TestUnpegDelayedMessage(Message message, int number, Message response, Message delayedResponse)
        {
            this.Given(x => GivenAMessage(message))
                .And(x => GivenTheNextRandomIs(number))
                .When(x => WhenRespondingToAMessage())
                .Then(x => ThenItShouldReturnAMessage(response))
                .And(x => ThenItShouldReturnADelayedResponse(delayedResponse))
                .BDDfy();
        }

        private void GivenAMessage(Message message)
        {
            _message = message;
        }

        private void GivenTheNextRandomIs(int number)
        {
            _randomnessHandler.Random.Next(Arg.Any<int>()).ReturnsForAnyArgs(number);
        }

        private async Task WhenRespondingToAMessage()
        {
            _result = await _subject.Respond(_message);
        }

        private void ThenItShouldReturnAMessage(Message result)
        {
            CompareMessages(result, _result);
        }

        private void ThenItShouldReturnADelayedResponse(Message delayedResponse)
        {
            _asyncDelayer.ReceivedWithAnyArgs(1).Delay(Arg.Any<TimeSpan>(), Arg.Any<CancellationToken>());
            _chatHelper.Messages.Received(1).SendMessageAsync(Arg.Any<Message>());
            var response = _chatHelper.Messages.ReceivedCalls().First().GetArguments()[0];
            CompareMessages(delayedResponse, (Message)response);
        }

        private static void CompareMessages(Message expected, Message actual)
        {
            actual.Text.ShouldBe(expected.Text);
            actual.RoomId.ShouldBe(expected.RoomId);

            if (expected.MessageParts == null)
            {
                return;
            }

            actual.MessageParts.ShouldNotBeNull();
            actual.MessageParts.Length.ShouldBe(expected.MessageParts.Length);
            for (var i = 0; i < actual.MessageParts.Length; i++)
            {
                var actualPart = actual.MessageParts[i];
                var expectedPart = expected.MessageParts[i];
                actualPart.Text.ShouldBe(expectedPart.Text);
                actualPart.MessageType.ShouldBe(expectedPart.MessageType);
                actualPart.UserId.ShouldBe(expectedPart.UserId);
            }
        }
    }
}
