using System;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core;
using GlobalX.ChatBots.Core.Messages;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PockyBot.NET.Services;
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
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;

        private Message _message;
        private Message _result;

        public UnpegTests()
        {
            _randomnessHandler = Substitute.For<IRandomnessHandler>();
            _randomnessHandler.Random.Returns(Substitute.For<Random>());
            _chatHelper = Substitute.For<IChatHelper>();
            _backgroundTaskQueue = Substitute.For<IBackgroundTaskQueue>();
            _subject = new Unpeg(_randomnessHandler, _chatHelper, _backgroundTaskQueue, Substitute.For<ILogger<Unpeg>>());
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
            _result.Text.ShouldBe(result.Text);
            _result.RoomId.ShouldBe(result.RoomId);
            if (result.MessageParts != null)
            {
                _result.MessageParts.ShouldNotBeNull();
                _result.MessageParts.Length.ShouldBe(result.MessageParts.Length);
                for (var i = 0; i < _result.MessageParts.Length; i++)
                {
                    var actualPart = _result.MessageParts[i];
                    var expectedPart = result.MessageParts[i];
                    actualPart.Text.ShouldBe(expectedPart.Text);
                    actualPart.MessageType.ShouldBe(expectedPart.MessageType);
                    actualPart.UserId.ShouldBe(expectedPart.UserId);
                }
            }
        }
    }
}
