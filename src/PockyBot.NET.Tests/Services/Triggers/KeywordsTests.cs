using System.Collections.Generic;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core.Messages;
using GlobalX.ChatBots.Core.People;
using NSubstitute;
using PockyBot.NET.Persistence.Repositories;
using PockyBot.NET.Services.Triggers;
using PockyBot.NET.Tests.TestData.Triggers;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace PockyBot.NET.Tests.Services.Triggers
{
    public class KeywordsTests
    {
        private readonly Keywords _subject;

        private readonly IConfigRepository _configRepository;

        private Message _message;
        private Message _result;

        public KeywordsTests()
        {
            _configRepository = Substitute.For<IConfigRepository>();
            _subject = new Keywords(_configRepository);
        }

        [Theory]
        [MemberData(nameof(KeywordsTestData.RespondTestData), MemberType = typeof(KeywordsTestData))]
        public void TestRespond(IList<string> keywords, IList<string> penaltyKeywords, Message response)
        {
            this.Given(x => GivenAMessage(GetKeywordsMessage()))
                .And(x => GivenAStringConfig("keyword", keywords))
                .And(x => GivenAStringConfig("penaltyKeyword", penaltyKeywords))
                .When(x => WhenRespondingToAMessage())
                .Then(x => ThenItShouldReturnAResponse(response))
                .BDDfy();
        }

        private void GivenAMessage(Message message)
        {
            _message = message;
        }

        private void GivenAStringConfig(string configName, IList<string> config)
        {
            _configRepository.GetStringConfig(configName).Returns(config);
        }

        private async Task WhenRespondingToAMessage()
        {
            _result = await _subject.Respond(_message);
        }

        private void ThenItShouldReturnAResponse(Message response)
        {
            if (response != null)
            {
                _result.ShouldNotBeNull();
                _result.Text.ShouldBe(response.Text);
            }
            else
            {
                _result.ShouldBeNull();
            }
        }

        private static Message GetKeywordsMessage()
        {
            return new Message
            {
                Text = "TestBot keywords",
                MessageParts = new[]
                {
                    new MessagePart
                    {
                        Text = "TestBot",
                        MessageType = MessageType.PersonMention,
                        UserId = "testBotId"
                    },
                    new MessagePart
                    {
                        Text = " status",
                        MessageType = MessageType.Text
                    }
                },
                Sender = new Person
                {
                    UserId = "testUserId",
                    Username = "Test User",
                    Type = PersonType.Person
                }
            };
        }
    }
}
