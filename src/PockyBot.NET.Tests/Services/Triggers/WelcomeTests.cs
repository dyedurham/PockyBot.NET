using System.Collections.Generic;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core.Messages;
using Microsoft.Extensions.Options;
using NSubstitute;
using PockyBot.NET.Configuration;
using PockyBot.NET.Persistence.Repositories;
using PockyBot.NET.Services.Triggers;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace PockyBot.NET.Tests.Services.Triggers
{
    public class WelcomeTests
    {
        private readonly Welcome _subject;
        private readonly IConfigRepository _configRepository;

        private const string BotName = "Pocky";
        private Message _message;
        private Message _result;

        public WelcomeTests()
        {
            var pockyUserSettings = Substitute.For<IOptions<PockyBotSettings>>();
            pockyUserSettings.Value.Returns(new PockyBotSettings
            {
                BotName = BotName
            });
            _configRepository = Substitute.For<IConfigRepository>();
            _subject = new Welcome(pockyUserSettings, _configRepository);
        }

        [Fact]
        public void ItShouldReturnTheCorrectWelcomeMessageWhenKeywordsAreNotRequired()
        {
            this.Given(x => GivenAWelcomeMessage())
                .And(x => GivenNoKeywordsAreRequired())
                .When(x => WhenRespondIsCalled())
                .Then(x => ThenItShouldReturnTheWelcomeMessageWithoutKeywords())
                .BDDfy();
        }

        [Fact]
        public void ItShouldReturnTheCorrectWelcomeMessageWhenKeywordsAreRequired()
        {
            this.Given(x => GivenAWelcomeMessage())
                .And(x => GivenKeywordsAreRequired())
                .When(x => WhenRespondIsCalled())
                .Then(x => ThenItShouldReturnTheWelcomeMessageWithKeywords())
                .BDDfy();
        }

        private void GivenAWelcomeMessage()
        {
            _message = new Message
            {
                MessageParts = new []
                {
                    new MessagePart
                    {
                        MessageType = MessageType.PersonMention,
                        Text = "PockyBot",
                        UserId = BotName
                    },
                    new MessagePart
                    {
                        MessageType = MessageType.Text,
                        Text = "welcome"
                    }
                }
            };
        }

        private void GivenNoKeywordsAreRequired()
        {
            _configRepository.GetGeneralConfig("requireValues").Returns(0);
        }

        private void GivenKeywordsAreRequired()
        {
            _configRepository.GetGeneralConfig("requireValues").Returns(1);
            _configRepository.GetStringConfig("keyword").Returns(new List<string> {"keyword1", "keyword2"});
        }

        private async Task WhenRespondIsCalled()
        {
            _result = await _subject.Respond(_message);
        }

        private void ThenItShouldReturnTheWelcomeMessageWithoutKeywords()
        {
            _result.Text.ShouldContain("Make sure to tell us why you're giving them a peg.");
            _result.Text.ShouldNotContain("Tell us why you're giving them a peg and include the relevant company values in your description:");
        }

        private void ThenItShouldReturnTheWelcomeMessageWithKeywords()
        {
            _result.Text.ShouldContain("Tell us why you're giving them a peg and include the relevant company values in your description: keyword1, keyword2");
        }
    }
}
