using System.Threading.Tasks;
using GlobalX.ChatBots.Core.Messages;
using GlobalX.ChatBots.Core.People;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PockyBot.NET.Persistence.Repositories;
using PockyBot.NET.Services;
using PockyBot.NET.Services.Pegs;
using PockyBot.NET.Services.Triggers;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace PockyBot.NET.Tests.Services.Triggers
{
    public class ResultsTests
    {
        private readonly Results _subject;
        private readonly IResultsFileGenerator _resultsFileGenerator;

        private Message _message;
        private const string ResultsUrl = "https://someurl";
        private Message _response;

        public ResultsTests()
        {
            var pockyUserRepository = Substitute.For<IPockyUserRepository>();
            var pegResultsHelper = Substitute.For<IPegResultsHelper>();
            _resultsFileGenerator = Substitute.For<IResultsFileGenerator>();
            _subject = new Results(pockyUserRepository, pegResultsHelper,
                _resultsFileGenerator, Substitute.For<ILogger<Results>>());
        }

        [Fact]
        public void ItShouldReturnTheResultsOfTheCycle()
        {
            this.Given(x => GivenAResultsMessage())
                .And(x => GivenTheResultsFileGeneratorReturnsTheUrl())
                .When(x => WhenRespondIsCalled())
                .Then(x => ThenItShouldReturnTheResultsMessage())
                .BDDfy();
        }

        private void GivenAResultsMessage()
        {
            _message = new Message
            {
                Sender = new Person
                {
                    UserId = "someSender"
                },
                MessageParts = new []
                {
                    new MessagePart
                    {
                        MessageType = MessageType.PersonMention,
                        Text = "PockyBot",
                        UserId = "PockyBot"
                    },
                    new MessagePart
                    {
                        MessageType = MessageType.Text,
                        Text = "results"
                    }
                }
            };
        }

        private void GivenTheResultsFileGeneratorReturnsTheUrl()
        {
            _resultsFileGenerator.GenerateResultsFileAndReturnLink(null)
                .ReturnsForAnyArgs(Task.FromResult<string>(ResultsUrl));
        }

        private async Task WhenRespondIsCalled()
        {
            _response = await _subject.Respond(_message);
        }

        private void ThenItShouldReturnTheResultsMessage()
        {
            _response.Text.ShouldBe($"[Here are all pegs given this cycle]({ResultsUrl})");
        }
    }
}
