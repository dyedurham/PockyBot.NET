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
                .And(x => GivenThePegResultsHelperReturnsTheCategoryResults())
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
                    TotalPoints = 6,
                    Pegs = new List<PegDetails>
                    {
                        new PegDetails
                        {
                            Comment = "This is a peg\nwith some newlines\nwoo",
                            Keywords = new List<string>{"customer"},
                            SenderLocation = "Brisbane",
                            SenderName = "Second User",
                            Weight = 1
                        },
                        new PegDetails
                        {
                            Comment = "This is another peg",
                            Keywords = new List<string>{"collaborative", "real"},
                            SenderLocation = "Melbourne",
                            SenderName = "Third User",
                            Weight = 2
                        }
                    }
                },
                new PegRecipient
                {
                    Location = "Brisbane",
                    Name = "Second User",
                    UserId = "SecondUser",
                    PegCount = 1,
                    PegsGivenCount = 3,
                    PenaltyCount = 1,
                    TotalPoints = 0,
                    Pegs = new List<PegDetails>
                    {
                        new PegDetails
                        {
                            Comment = "Moooore pegs",
                            Keywords = new List<string>{"awesome"},
                            SenderLocation = "Brisbane",
                            SenderName = "User",
                            Weight = 1
                        }
                    },
                    Penalties = new List<PegDetails>
                    {
                        new PegDetails
                        {
                            Comment = "shame",
                            Keywords = new List<string>{"shame"},
                            SenderLocation = "Brisbane",
                            SenderName = "Second User",
                            Weight = 1
                        }
                    }
                },
                new PegRecipient
                {
                    Name = "Some Winner",
                    Location = "Melbourne",
                    UserId = "SomeWinner",
                    PegCount = 2,
                    PegsGivenCount = 5,
                    PenaltyCount = 0,
                    TotalPoints = 4,
                    Pegs = new List<PegDetails>
                    {
                        new PegDetails
                        {
                            Comment = "hello peg",
                            Keywords = new List<string>{"customer"},
                            SenderLocation = "Brisbane",
                            SenderName = "User",
                            Weight = 2
                        },
                        new PegDetails
                        {
                            Comment = "hello second peg",
                            Keywords = new List<string>{"collaborative"},
                            SenderLocation = "Brisbane",
                            SenderName = "Second User",
                            Weight = 2
                        }
                    }
                }
            };
        }

        private void GivenThePegResultsHelperReturnsTheWinners()
        {
            _pegResultsHelper.GetWinners(_mappedUsers).Returns(new List<PegRecipient>
            {
                new PegRecipient
                {
                    Name = "Some Winner",
                    Location = "Melbourne",
                    UserId = "SomeWinner",
                    PegCount = 2,
                    PegsGivenCount = 5,
                    PenaltyCount = 0,
                    TotalPoints = 4,
                    Pegs = new List<PegDetails>
                    {
                        new PegDetails
                        {
                            Comment = "hello peg",
                            Keywords = new List<string>{"customer"},
                            SenderLocation = "Brisbane",
                            SenderName = "User",
                            Weight = 2
                        },
                        new PegDetails
                        {
                            Comment = "hello second peg",
                            Keywords = new List<string>{"collaborative"},
                            SenderLocation = "Brisbane",
                            SenderName = "Second User",
                            Weight = 2
                        }
                    }
                }
            });
        }

        private void GivenThePegResultsHelperReturnsTheCategoryResults()
        {
            _pegResultsHelper.GetCategories(_mappedUsers).Returns(new List<PegCategory>
            {
                new PegCategory
                {
                    Name = "Collaborative",
                    Recipients = new List<PegRecipient>
                    {
                        new PegRecipient
                        {
                            Name = "Some Winner",
                            Location = "Melbourne",
                            UserId = "SomeWinner",
                            PegCount = 2,
                            PegsGivenCount = 5,
                            PenaltyCount = 0,
                            TotalPoints = 4,
                            Pegs = new List<PegDetails>
                            {
                                new PegDetails
                                {
                                    Comment = "hello second peg",
                                    Keywords = new List<string> { "collaborative" },
                                    SenderLocation = "Brisbane",
                                    SenderName = "Second User",
                                    Weight = 2
                                }
                            }
                        },
                        new PegRecipient
                        {
                            Name = "Some Winner",
                            Location = "Melbourne",
                            UserId = "SomeWinner",
                            PegCount = 2,
                            PegsGivenCount = 5,
                            PenaltyCount = 0,
                            TotalPoints = 4,
                            Pegs = new List<PegDetails>
                            {
                                new PegDetails
                                {
                                    Comment = "hello second peg",
                                    Keywords = new List<string> { "collaborative" },
                                    SenderLocation = "Brisbane",
                                    SenderName = "Second User",
                                    Weight = 2
                                }
                            }
                        }
                    }
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
