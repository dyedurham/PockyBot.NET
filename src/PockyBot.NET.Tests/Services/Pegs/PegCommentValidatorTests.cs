using NSubstitute;
using PockyBot.NET.Persistence.Repositories;
using PockyBot.NET.Services.Pegs;
using PockyBot.NET.Tests.TestData.Pegs;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace PockyBot.NET.Tests.Services.Pegs
{
    public class PegCommentValidatorTests
    {
        private readonly PegHelper _subject;

        private IConfigRepository _configRepository;

        private string _comment;
        private int? _requireKeywords;
        private string[] _keywords;
        private string[] _penaltyKeywords;

        private bool _result;

        public PegCommentValidatorTests()
        {
            _configRepository = Substitute.For<IConfigRepository>();
            _subject = new PegHelper(_configRepository);
        }

        [Theory]
        [MemberData(nameof(PegCommentValidatorTestData.IsPegValid), MemberType = typeof(PegCommentValidatorTestData))]
        public void TestIsPegValid(string comment, int? requireKeywords, string[] keywords, string[] penaltyKeywords, bool isValid)
        {
            this.Given(x => GivenAComment(comment))
                .And(x => GivenRequireKeywords(requireKeywords))
                .And(x => GivenKeywords(keywords))
                .And(x => GivenPenaltyKeywords(penaltyKeywords))
                .When(x => WhenValidatingAPeg())
                .Then(x => ThenItShouldReturn(isValid))
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

        private void WhenValidatingAPeg()
        {
            _result = _subject.IsPegValid(_comment, _requireKeywords, _keywords, _penaltyKeywords);
        }

        private void ThenItShouldReturn(bool isValid)
        {
            _result.ShouldBe(isValid);
        }
    }
}
