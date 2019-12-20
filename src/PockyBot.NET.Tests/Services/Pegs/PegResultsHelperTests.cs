using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using PockyBot.NET.Models;
using PockyBot.NET.Persistence.Models;
using PockyBot.NET.Persistence.Repositories;
using PockyBot.NET.Services.Pegs;
using PockyBot.NET.Tests.TestData.Pegs;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace PockyBot.NET.Tests.Services.Pegs
{
    public class PegResultsHelperTests
    {
        private readonly IPegResultsHelper _subject;

        private readonly IConfigRepository _configRepository;
        private readonly IPegHelper _pegHelper;

        private List<PockyUser> _pockyUsers;
        private List<PegRecipient> _pegRecipients;
        private List<PegRecipient> _returnedUserList;
        private List<PegCategory> _returnedCategories;

        public PegResultsHelperTests()
        {
            _configRepository = Substitute.For<IConfigRepository>();
            _pegHelper = Substitute.For<IPegHelper>();
            _subject = new PegResultsHelper(_configRepository, _pegHelper);
        }

        [Theory]
        [MemberData(nameof(PegResultsHelperTestData.MapUsersToPegRecipientsTestData), MemberType = typeof(PegResultsHelperTestData))]
        internal void TestMapUsersToPegRecipients(List<PockyUser> pockyUsers, List<PegRecipient> expected)
        {
            this.Given(x => GivenAListOfPockyUsers(pockyUsers))
                .And(x => GivenAStringConfig("keyword", new List<string> {"keyword1", "keyword2"}))
                .And(x => GivenAStringConfig("penaltyKeyword", new List<string> {"penaltyKeyword"}))
                .And(x => GivenAGeneralConfig("requireValues", 1))
                .And(x => GivenPegValidity("penaltyKeyword"))
                .And(x => GivenPegWeighting())
                .When(x => WhenMappingUsersToPegRecipients())
                .Then(x => ThenItShouldReturnAListOfPegRecipients(expected))
                .BDDfy();
        }

        [Theory]
        [MemberData(nameof(PegResultsHelperTestData.GetWinnersTestData), MemberType = typeof(PegResultsHelperTestData))]
        internal void TestGetWinners(List<PegRecipient> mappedUsers, int minimum, int winners, List<PegRecipient> expected)
        {
            this.Given(x => GivenAListOfPegRecipients(mappedUsers))
                .And(x => GivenAGeneralConfig("minimum", minimum))
                .And(x => GivenAGeneralConfig("winners", winners))
                .When(x => WhenGettingWinners())
                .Then(x => ThenItShouldReturnAListOfPegRecipients(expected))
                .BDDfy();
        }

        [Theory]
        [MemberData(nameof(PegResultsHelperTestData.GetCategoriesTestData), MemberType = typeof(PegResultsHelperTestData))]
        internal void TestGetCategories(List<PegRecipient> mappedUsers, List<PegCategory> expected)
        {
            this.Given(x => GivenAListOfPegRecipients(mappedUsers))
                .And(x => GivenAStringConfig("keyword", new List<string> {"keyword1", "keyword2"}))
                .When(x => WhenGettingCategories())
                .Then(x => ThenItShouldReturnAListOfCategories(expected))
                .BDDfy();
        }

        private void GivenAListOfPockyUsers(List<PockyUser> pockyUsers)
        {
            _pockyUsers = pockyUsers;
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
                x => !((string)x[0]).Contains(penaltyKeyword));
        }

        private void GivenPegWeighting()
        {
            _pegHelper.GetPegWeighting(Arg.Any<string>(), Arg.Any<string>()).Returns(x =>
                (string)x[0] == (string)x[1] ? 1 : 2);
        }

        private void GivenAListOfPegRecipients(List<PegRecipient> pegRecipients)
        {
            _pegRecipients = pegRecipients;
        }

        private void WhenMappingUsersToPegRecipients()
        {
            _returnedUserList = _subject.MapUsersToPegRecipients(_pockyUsers);
        }

        private void WhenGettingWinners()
        {
            _returnedUserList = _subject.GetWinners(_pegRecipients);
        }

        private void WhenGettingCategories()
        {
            _returnedCategories = _subject.GetCategories(_pegRecipients);
        }

        private void ThenItShouldReturnAListOfPegRecipients(List<PegRecipient> expected)
        {
            ComparePegRecipients(_returnedUserList, expected);
        }

        private void ThenItShouldReturnAListOfCategories(List<PegCategory> expected)
        {
            _returnedCategories.ShouldNotBeNull();
            _returnedCategories.Count.ShouldBe(expected.Count);
            _returnedCategories = _returnedCategories.OrderBy(x => x.Name).ToList();
            expected = expected.OrderBy(x => x.Name).ToList();
            for (int i = 0; i < _returnedCategories.Count; i++)
            {
                _returnedCategories[i].Name.ShouldBe(expected[i].Name);
                ComparePegRecipients(_returnedCategories[i].Recipients, expected[i].Recipients);
            }
        }

        private void ComparePegRecipients(List<PegRecipient> actual, List<PegRecipient> expected)
        {
            actual.ShouldNotBeNull();
            actual.Count.ShouldBe(expected.Count);
            actual = actual.OrderBy(x => x.UserId).ToList();
            expected = expected.OrderBy(x => x.UserId).ToList();
            for (int i = 0; i < actual.Count; i++)
            {
                var currentUser = actual[i];
                var expectedUser = expected[i];
                currentUser.ShouldSatisfyAllConditions(
                    () => currentUser.Name.ShouldBe(expectedUser.Name),
                    () => currentUser.UserId.ShouldBe(expectedUser.UserId),
                    () => currentUser.Location.ShouldBe(expectedUser.Location),
                    () => currentUser.TotalPoints.ShouldBe(expectedUser.TotalPoints),
                    () => currentUser.PegCount.ShouldBe(expectedUser.PegCount),
                    () => currentUser.PenaltyCount.ShouldBe(expectedUser.PenaltyCount),
                    () => currentUser.PegsGivenCount.ShouldBe(expectedUser.PegsGivenCount),
                    () =>
                    {
                        currentUser.Pegs.ShouldNotBeNull();
                        currentUser.Pegs.Count.ShouldBe(expectedUser.Pegs.Count);
                        foreach (var peg in expectedUser.Pegs)
                        {
                            currentUser.Pegs.ShouldContain(x =>
                                x.SenderName == peg.SenderName && x.Weight == peg.Weight && x.Comment == peg.Comment
                                && x.Keywords.OrderBy(y => y).SequenceEqual(peg.Keywords.OrderBy(y => y))
                                && x.SenderLocation == peg.SenderLocation);
                        }
                    },
                    () =>
                    {
                        currentUser.Penalties.ShouldNotBeNull();
                        currentUser.Penalties.Count.ShouldBe(expectedUser.Penalties.Count);
                        foreach (var penalty in expectedUser.Penalties)
                        {
                            currentUser.Penalties.ShouldContain(x =>
                                x.SenderName == penalty.SenderName && x.Weight == penalty.Weight && x.Comment == penalty.Comment
                                && x.Keywords.OrderBy(y => y).SequenceEqual(penalty.Keywords.OrderBy(y => y))
                                && x.SenderLocation == penalty.SenderLocation);
                        }
                    }
                );
            }
        }
    }
}
