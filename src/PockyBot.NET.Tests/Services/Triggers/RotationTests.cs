using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core.Messages;
using NSubstitute;
using PockyBot.NET.Persistence.Repositories;
using PockyBot.NET.Services.Triggers;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace PockyBot.NET.Tests.Services.Triggers
{
    public class RotationTests
    {
        private readonly Rotation _subject;
        private readonly IConfigRepository _configRepository;

        private const string Rotation = "team1,team2";
        private Message _message;
        private Message _result;

        public RotationTests()
        {
            _configRepository = Substitute.For<IConfigRepository>();
            _subject = new Rotation(_configRepository);
        }

        [Fact]
        public void ItShouldReturnTheRotationMessage()
        {
            this.Given(x => GivenARotationMessage())
                .And(x => GivenTheConfigRepositoryReturnsTheRotation())
                .When(x => WhenRespondIsCalled())
                .Then(x => ThenItShouldReturnTheRotationMessage())
                .BDDfy();
        }

        private void GivenARotationMessage()
        {
            _message = new Message
            {
                MessageParts = new []
                {
                    new MessagePart
                    {
                        MessageType = MessageType.PersonMention,
                        Text = "PockyBot",
                        UserId = "TestPockyBot"
                    },
                    new MessagePart
                    {
                        MessageType = MessageType.Text,
                        Text = "rotation"
                    }
                }
            };
        }

        private void GivenTheConfigRepositoryReturnsTheRotation()
        {
            _configRepository.GetStringConfig("rotation").Returns(new List<string> {Rotation});
        }

        private async Task WhenRespondIsCalled()
        {
            _result = await _subject.Respond(_message);
        }

        private void ThenItShouldReturnTheRotationMessage()
        {
            var nl = Environment.NewLine;
            _result.Text.ShouldBe("## Here's the snack buying rotation:\n\n" +
                                  $"* team1{nl}* team2{nl}");
        }
    }
}
