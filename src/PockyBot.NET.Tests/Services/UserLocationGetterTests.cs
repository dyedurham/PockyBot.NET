using Shouldly;
using TestStack.BDDfy;
using Xunit;
using NSubstitute;
using PockyBot.NET.Persistence.Repositories;
using PockyBot.NET.Services;

namespace PockyBot.NET.Tests.Services
{
    public class UserLocationGetterTests
    {
        private IPockyUserRepository _pockyUserRepository;

        private readonly IUserLocationGetter _subject;
        private string _result;

        private string[] _commands;
        private string[] _mentionedUsers;
        private string _meId;

        public UserLocationGetterTests()
        {
            _pockyUserRepository = Substitute.For<IPockyUserRepository>();
            _subject = new UserLocationGetter(_pockyUserRepository);
        }

        [Fact]
        void NoCommandsShouldReturnAnError()
        {
            this.Given(x => GivenNoCommands())
                .When(x => WhenGetUserLocationIsCalled())
                .Then(x => ThenItShouldReturn(
                    "Please specify a user or group of users. Possible arguments are me, unset, all, or a list of mentioned users."))
                .BDDfy();
        }

        private void GivenNoCommands()
        {
            _commands = new string[] { };
            _mentionedUsers = new string[] { };
            _meId = "";
        }

        private void WhenGetUserLocationIsCalled()
        {
            _result = _subject.GetUserLocation(_commands, _mentionedUsers, _meId);
        }

        private void ThenItShouldReturn(string expected)
        {
            _result.ShouldBe(expected);
        }
    }
}
