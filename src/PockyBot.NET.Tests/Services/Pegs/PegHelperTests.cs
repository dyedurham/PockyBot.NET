using System.Collections.Generic;
using NSubstitute;
using PockyBot.NET.Persistence.Models;
using PockyBot.NET.Persistence.Repositories;
using PockyBot.NET.Services.Pegs;
using PockyBot.NET.Tests.TestData.Pegs;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace PockyBot.NET.Tests.Services.Pegs
{
    public class PegHelperTests
    {
        private readonly PegHelper _subject;

        private readonly IConfigRepository _configRepository;

        private string _comment;
        private int? _requireKeywords;
        private string[] _keywords;
        private string[] _penaltyKeywords;

        private bool _isPegValid;

        private string _senderLocation;
        private string _receiverLocation;

        private int _pegWeighting;

        public PegHelperTests()
        {
            _configRepository = Substitute.For<IConfigRepository>();
            _subject = new PegHelper(_configRepository);
        }

        [Theory]
        [MemberData(nameof(PegHelperTestData.IsPegValidTestData), MemberType = typeof(PegHelperTestData))]
        public void IsPegValidTest(string comment, int? requireKeywords, string[] keywords, string[] penaltyKeywords, bool isPegValid)
        {
            this.Given(x => GivenAComment(comment))
                .And(x => GivenRequireKeywords(requireKeywords))
                .And(x => GivenKeywords(keywords))
                .And(x => GivenPenaltyKeywords(penaltyKeywords))
                .When(x => WhenIsPegValidIsCalled())
                .Then(x => ThenItShouldReturnIsPegValid(isPegValid))
                .BDDfy();
        }

        [Theory]
        [MemberData(nameof(PegHelperTestData.GetPegWeightingTestData), MemberType = typeof(PegHelperTestData))]
        internal void GetPegWeightingTest(string senderLocation, string receiverLocation, List<GeneralConfig> allGeneralConfig, int? defaultRemoteLocationWeighting, int pegWeighting)
        {
            this.Given(x => GivenASenderLocation(senderLocation))
                .And(x => GivenAReceiverLocation(receiverLocation))
                .And(x => GivenAllGeneralConfig(allGeneralConfig))
                .And(x => GivenDefaultRemoteLocationWeighting(defaultRemoteLocationWeighting))
                .When(x => WhenGettingAPegWeighting())
                .Then(x => ThenItShouldReturnAPegWeighting(pegWeighting))
                .BDDfy();
        }

        private void GivenAComment(string comment)
        {
            _comment = comment;
        }

        private void GivenRequireKeywords(int? requireKeywords)
        {
            _requireKeywords = requireKeywords;
        }

        private void GivenKeywords(string[] keywords)
        {
            _keywords = keywords;
        }

        private void GivenPenaltyKeywords(string[] penaltyKeywords)
        {
            _penaltyKeywords = penaltyKeywords;
        }

        private void GivenASenderLocation(string senderLocation)
        {
            _senderLocation = senderLocation;
        }

        private void GivenAReceiverLocation(string receiverLocation)
        {
            _receiverLocation = receiverLocation;
        }

        private void GivenAllGeneralConfig(List<GeneralConfig> allGeneralConfig)
        {
            _configRepository.GetAllGeneralConfig().Returns(allGeneralConfig);
        }

        private void GivenDefaultRemoteLocationWeighting(int? defaultRemoteLocationWeighting)
        {
            _configRepository.GetGeneralConfig("remoteLocationWeightingDefault").Returns(defaultRemoteLocationWeighting);
        }

        private void WhenIsPegValidIsCalled()
        {
            _isPegValid = _subject.IsPegValid(_comment, _requireKeywords, _keywords, _penaltyKeywords);
        }

        private void WhenGettingAPegWeighting()
        {
            _pegWeighting = _subject.GetPegWeighting(_senderLocation, _receiverLocation);
        }

        private void ThenItShouldReturnIsPegValid(bool expected)
        {
            _isPegValid.ShouldBe(expected);
        }

        private void ThenItShouldReturnAPegWeighting(int expected)
        {
            _pegWeighting.ShouldBe(expected);
        }
    }
}
