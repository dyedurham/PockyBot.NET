using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core.Messages;
using NSubstitute;
using PockyBot.NET.Persistence.Models;
using PockyBot.NET.Persistence.Repositories;
using PockyBot.NET.Services.Pegs;
using PockyBot.NET.Services.Triggers;
using PockyBot.NET.Tests.TestData.Triggers;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace PockyBot.NET.Tests.Services.Triggers
{
    public class StatusTests
    {
        private readonly Status _subject;

        private readonly IPockyUserRepository _pockyUserRepository;
        private readonly IConfigRepository _configRepository;
        private readonly IPegCommentValidator _pegCommentValidator;

        private Message _message;
        private Message _result;

        public StatusTests()
        {
            _pockyUserRepository = Substitute.For<IPockyUserRepository>();
            _configRepository = Substitute.For<IConfigRepository>();
            _pegCommentValidator = Substitute.For<IPegCommentValidator>();
            _subject = new Status(_pockyUserRepository, _configRepository, _pegCommentValidator);
        }

        [Theory]
        [MemberData(nameof(StatusTestData.RespondTestData), MemberType = typeof(StatusTestData))]
        public void TestRespond(Message message, PockyUser sender, int limit, List<Tuple<string, bool>> pegValidity, Message expectedResponse)
        {
            this.Given(x => GivenAMessage(message))
                .And(x => GivenAPockyUser(sender))
                .And(x => GivenAStringConfig("keyword", new List<string> { "keyword1", "keyword2" }))
                .And(x => GivenAStringConfig("penaltyKeyword", new List<string> { "penaltyKeyword" }))
                .And(x => GivenAGeneralConfig("requireValues", 1))
                .And(x => GivenAGeneralConfig("limit", limit))
                .And(x => GivenPegValidity(pegValidity))
                .When(x => WhenRespondingToAMessage())
                .Then(x => ThenItShouldReturnAResponse(expectedResponse))
                .BDDfy();
        }

        private void GivenAMessage(Message message)
        {
            _message = message;
        }

        private void GivenAPockyUser(PockyUser user)
        {
            if (user != null)
            {
                _pockyUserRepository.GetUser(user.UserId).Returns(user);
            }
            else
            {
                _pockyUserRepository.GetUser(Arg.Any<string>()).ReturnsForAnyArgs((PockyUser)null);
            }
        }

        private void GivenAStringConfig(string configName, List<string> config)
        {
            _configRepository.GetStringConfig(configName).Returns(config);
        }

        private void GivenAGeneralConfig(string configName, int config)
        {
            _configRepository.GetGeneralConfig(configName).Returns(config);
        }

        private void GivenPegValidity(List<Tuple<string, bool>> pegValidity)
        {
            foreach (var peg in pegValidity)
            {
                _pegCommentValidator.IsPegValid(peg.Item1, Arg.Any<int>(), Arg.Any<string[]>(), Arg.Any<string[]>())
                    .Returns(peg.Item2);
            }
        }

        private async Task WhenRespondingToAMessage()
        {
            _result = await _subject.Respond(_message);
        }

        private void ThenItShouldReturnAResponse(Message expectedResponse)
        {
            _result.ShouldNotBeNull();
            _result.Text.ShouldBe(expectedResponse.Text);
            _result.RoomId.ShouldBe(expectedResponse.RoomId);
        }
    }
}
