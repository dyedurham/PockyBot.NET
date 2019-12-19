using System;
using System.Collections.Generic;
using System.Text;
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
    public class FinishTests
    {
        private readonly Finish _subject;

        private readonly IPockyUserRepository _pockyUserRepository;
        private readonly IResultsUploader _resultsUploader;
        private readonly IConfigRepository _configRepository;
        private readonly IPegHelper _pegHelper;

        private Message _message;
        private Message _result;

        public FinishTests()
        {
            _pockyUserRepository = Substitute.For<IPockyUserRepository>();
            _resultsUploader = Substitute.For<IResultsUploader>();
            _configRepository = Substitute.For<IConfigRepository>();
            _pegHelper = Substitute.For<IPegHelper>();
            _subject = new Finish(_pockyUserRepository, _resultsUploader, _configRepository, _pegHelper);
        }

        [Theory]
        [MemberData(nameof(FinishTestData.RespondTestData), MemberType = typeof(FinishTestData))]
        public void TestRespond(Message message, List<PockyUser> allUsersWithPegs, int minimum, int winners,
            string uploadLocation, Message response)
        {
            this.Given(x => GivenAMessage(message))
                .And(x => GivenAllUsersWithPegs(allUsersWithPegs))
                .And(x => GivenAStringConfig("keyword", new List<string> {"keyword1", "keyword2"}))
                .And(x => GivenAStringConfig("penaltyKeyword", new List<string> {"penaltyKeyword"}))
                .And(x => GivenAGeneralConfig("requireValues", 1))
                .And(x => GivenAGeneralConfig("minimum", minimum))
                .And(x => GivenAGeneralConfig("winners", winners))
                .And(x => GivenPegValidity("penaltyKeyword"))
                .And(x => GivenPegWeighting(1))
                .And(x => GivenUploadedResultsLocation(uploadLocation))
                .When(x => WhenRespondingToAMessage())
                .Then(x => ThenItShouldReturnAResponse(response))
                .And(x => ThenItShouldPmAllUsers())
                .BDDfy();
        }

        private void GivenAMessage(Message message)
        {
            _message = message;
        }

        private void GivenAllUsersWithPegs(List<PockyUser> allUsersWithPegs)
        {
            _pockyUserRepository.GetAllUsersWithPegs().Returns(allUsersWithPegs);
        }

        private void GivenAStringConfig(string configName, List<string> config)
        {
            _configRepository.GetStringConfig(configName).Returns(config);
        }

        private void GivenAGeneralConfig(string configName, int config)
        {
            _configRepository.GetGeneralConfig(configName).Returns(config);
        }

        private void GivenPegValidity(string penaltyKeyword)
        {
            _pegHelper.IsPegValid(Arg.Any<string>(), Arg.Any<int?>(), Arg.Any<string[]>(), Arg.Any<string[]>()).Returns(
                x => !((string) x[0]).Contains(penaltyKeyword));
        }

        private void GivenPegWeighting(int weighting)
        {
            _pegHelper.GetPegWeighting(Arg.Any<string>(), Arg.Any<string>()).Returns(weighting);
        }

        private void GivenUploadedResultsLocation(string uploadLocation)
        {
            _resultsUploader.UploadResults(Arg.Any<string>()).Returns(Task.FromResult(uploadLocation));
        }

        private async void WhenRespondingToAMessage()
        {
            _result = await _subject.Respond(_message);
        }

        private void ThenItShouldReturnAResponse(Message response)
        {
            _result.ShouldNotBeNull();
            _result.Text.ShouldBe(response.Text);
        }

        private void ThenItShouldPmAllUsers()
        {
            // TODO
        }
    }
}
