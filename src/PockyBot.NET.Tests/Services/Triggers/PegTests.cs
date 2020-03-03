using System.Collections.Generic;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core;
using GlobalX.ChatBots.Core.Messages;
using GlobalX.ChatBots.Core.People;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PockyBot.NET.Models.Exceptions;
using PockyBot.NET.Persistence.Models;
using PockyBot.NET.Persistence.Repositories;
using PockyBot.NET.Services.Pegs;
using PockyBot.NET.Tests.TestData.Triggers;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace PockyBot.NET.Tests.Services.Triggers
{
    public class PegTests
    {
        private readonly NET.Services.Triggers.Peg _subject;

        private readonly IPegRequestValidator _pegRequestValidator;
        private readonly IPockyUserRepository _pockyUserRepository;
        private readonly IPegHelper _pegHelper;
        private readonly IConfigRepository _configRepository;
        private readonly IChatHelper _chatHelper;
        private readonly IPegGiver _pegGiver;

        private Message _message;
        private Message _result;

        public PegTests()
        {
            _pegRequestValidator = Substitute.For<IPegRequestValidator>();
            _pockyUserRepository = Substitute.For<IPockyUserRepository>();
            _pegHelper = Substitute.For<IPegHelper>();
            _configRepository = Substitute.For<IConfigRepository>();
            _chatHelper = Substitute.For<IChatHelper>();
            _chatHelper.People.Returns(Substitute.For<IPersonHandler>());
            _pegGiver = Substitute.For<IPegGiver>();
            _subject = new NET.Services.Triggers.Peg(_pegRequestValidator, _pockyUserRepository, _pegHelper,
                _configRepository, _chatHelper, _pegGiver, Substitute.For<ILogger<NET.Services.Triggers.Peg>>());
        }

        [Theory]
        [MemberData(nameof(PegTestData.RespondTestData), MemberType = typeof(PegTestData))]
        internal void TestRespond(Message message, string errorMessage, PockyUser senderUser,
            PockyUser receiverUser, int limit, string comment, bool isPegValid, Person receiverChatUser,
            Message response, bool givePeg)
        {
            this.Given(x => GivenAMessage(message))
                .And(x => GivenMessageValidity(errorMessage))
                .And(x => GivenAUser(senderUser))
                .And(x => GivenAUser(receiverUser))
                .And(x => GivenAStringConfig("keyword", new List<string> {"keyword1", "keyword2"}))
                .And(x => GivenAStringConfig("penaltyKeyword", new List<string> {"penaltyKeyword"}))
                .And(x => GivenAGeneralConfig("requireValues", 1))
                .And(x => GivenAGeneralConfig("limit", limit))
                .And(x => GivenPegValidity(comment, isPegValid))
                .And(x => GivenAChatUser(receiverChatUser))
                .When(x => WhenRespondingToAMessage())
                .Then(x => ThenItShouldReturnAResponse(response))
                .And(x => ThenItShouldCallGivePeg(givePeg))
                .BDDfy();
        }

        private void GivenAMessage(Message message)
        {
            _message = message;
        }

        private void GivenMessageValidity(string errorMessage)
        {
            if (errorMessage != null)
            {
                SubstituteExtensions.When(_pegRequestValidator, x => x.ValidatePegRequest(Arg.Any<Message>()))
                    .Do(x => throw new PegValidationException(errorMessage));
            }
        }

        private void GivenAUser(PockyUser user)
        {
            _pockyUserRepository.AddOrUpdateUserAsync(user.UserId, Arg.Any<string>()).Returns(Task.FromResult(user));
        }

        private void GivenAStringConfig(string configName, List<string> config)
        {
            _configRepository.GetStringConfig(configName).Returns(config);
        }

        private void GivenAGeneralConfig(string configName, int config)
        {
            _configRepository.GetGeneralConfig(configName).Returns(config);
        }

        private void GivenPegValidity(string comment, bool isPegValid)
        {
            _pegHelper.IsPegValid(comment, Arg.Any<int?>(), Arg.Any<string[]>(), Arg.Any<string[]>())
                .Returns(isPegValid);
            _pegHelper
                .IsPegValid(Arg.Any<string>(), Arg.Any<int?>(), Arg.Any<string[]>(), Arg.Any<string[]>())
                .Returns(isPegValid);
        }

        private void GivenAChatUser(Person person)
        {
            _chatHelper.People.GetPersonAsync(person.UserId).Returns(Task.FromResult(person));
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

        private void ThenItShouldCallGivePeg(bool givePeg)
        {

            _pegGiver.ReceivedWithAnyArgs(givePeg ? 1 : 0).GivePeg(Arg.Any<string>(), Arg.Any<PockyUser>(), Arg.Any<PockyUser>(),
                Arg.Any<int>());
        }
    }
}
