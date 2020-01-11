using System.Collections.Generic;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core.Messages;
using NSubstitute;
using PockyBot.NET.Models;
using PockyBot.NET.Persistence.Models;
using PockyBot.NET.Persistence.Repositories;
using PockyBot.NET.Services;
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
        private readonly IPegResultsHelper _pegResultsHelper;
        private readonly IResultsUploader _resultsUploader;
        private readonly IDirectResultsMessageSender _directResultsMessageSender;

        private Message _message;
        private Message _result;

        public FinishTests()
        {
            _pockyUserRepository = Substitute.For<IPockyUserRepository>();
            _resultsUploader = Substitute.For<IResultsUploader>();
            _pegResultsHelper = Substitute.For<IPegResultsHelper>();
            _directResultsMessageSender = Substitute.For<IDirectResultsMessageSender>();
            _subject = new Finish(_pockyUserRepository, _pegResultsHelper, _resultsUploader, _directResultsMessageSender);
        }

        [Theory]
        [MemberData(nameof(FinishTestData.RespondTestData), MemberType = typeof(FinishTestData))]
        internal void TestRespond(Message message, List<PockyUser> allUsersWithPegs, List<PegRecipient> mappedUsers,
            List<PegRecipient> winners, List<PegCategory> categories, string uploadLocation, Message response)
        {
            this.Given(x => GivenAMessage(message))
                .And(x => GivenAllUsersWithPegs(allUsersWithPegs))
                .And(x => GivenMappedUsers(mappedUsers))
                .And(x => GivenWinners(winners))
                .And(x => GivenPegCategories(categories))
                .And(x => GivenUploadedResultsLocation(uploadLocation))
                .When(x => WhenRespondingToAMessage())
                .Then(x => ThenItShouldReturnAResponse(response))
                .And(x => ThenItShouldPmAllUsers(mappedUsers))
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

        private void GivenMappedUsers(List<PegRecipient> mappedUsers)
        {
            _pegResultsHelper.MapUsersToPegRecipients(Arg.Any<List<PockyUser>>()).Returns(mappedUsers);
        }

        private void GivenWinners(List<PegRecipient> winners)
        {
            _pegResultsHelper.GetWinners(Arg.Any<List<PegRecipient>>()).Returns(winners);
        }

        private void GivenPegCategories(List<PegCategory> categories)
        {
            _pegResultsHelper.GetCategories(Arg.Any<List<PegRecipient>>()).Returns(categories);
        }

        private void GivenUploadedResultsLocation(string uploadLocation)
        {
            _resultsUploader.UploadResults(Arg.Any<string>()).Returns(Task.FromResult(uploadLocation));
        }

        private async Task WhenRespondingToAMessage()
        {
            _result = await _subject.Respond(_message);
        }

        private void ThenItShouldReturnAResponse(Message response)
        {
            _result.ShouldNotBeNull();
            _result.Text.ShouldBe(response.Text);
        }

        private void ThenItShouldPmAllUsers(List<PegRecipient> mappedUsers)
        {
            _directResultsMessageSender.Received(1).SendDirectMessagesAsync(mappedUsers);
        }
    }
}
