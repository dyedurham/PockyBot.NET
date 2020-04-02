using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PockyBot.NET.Models;
using PockyBot.NET.Services;
using PockyBot.NET.Services.Pegs;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace PockyBot.NET.Tests.Services
{
    public class ResultsFileGeneratorTests
    {
        private readonly IResultsFileGenerator _subject;
        private readonly IPegResultsHelper _pegResultsHelper;
        private readonly IResultsUploader _resultsUploader;

        private List<PegRecipient> _mappedUsers;
        private const string ResultsUrl = "https://someurl";
        private string _result;

        public ResultsFileGeneratorTests()
        {
            _pegResultsHelper = Substitute.For<IPegResultsHelper>();
            _resultsUploader = Substitute.For<IResultsUploader>();
            var logger = Substitute.For<ILogger<ResultsFileGenerator>>();
            _subject = new ResultsFileGenerator(_pegResultsHelper, logger, _resultsUploader);
        }

        [Fact]
        public void ItShouldGenerateTheResultFile()
        {
            this.Given(x => GivenASetOfMappedUser())
                .And(x => GivenThePegResultsHelperReturnsTheWinners())
                .And(x => GivenTheResultsUrlIsReturnedByTheUploader())
                .When(x => WhenGenerateResultsFileAndReturnLinkIsCalled())
                .Then(x => ThenItShouldReturnTheResultsUrl())
                .BDDfy();
        }

        private void GivenASetOfMappedUser()
        {
            _mappedUsers = new List<PegRecipient>
            {
                new PegRecipient
                {
                    Location = "Brisbane",
                    Name = "User",
                    UserId = "SomeUser",
                    PegCount = 5,
                    PegsGivenCount = 3,
                    PenaltyCount = 0,
                    TotalPoints = 6
                }
            };
        }

        private void GivenThePegResultsHelperReturnsTheWinners()
        {
            _pegResultsHelper.GetWinners(_mappedUsers).Returns(new List<PegRecipient>
            {
                new PegRecipient
                {
                    UserId = "SomeWinner"
                }
            });
        }

        private void GivenTheResultsUrlIsReturnedByTheUploader()
        {
            _resultsUploader.UploadResults(Arg.Any<string>()).Returns(Task.FromResult<string>(ResultsUrl));
        }

        private async Task WhenGenerateResultsFileAndReturnLinkIsCalled()
        {
            _result = await _subject.GenerateResultsFileAndReturnLink(_mappedUsers);
        }

        private void ThenItShouldReturnTheResultsUrl()
        {
            _result.ShouldBe(ResultsUrl);
        }
    }
}
