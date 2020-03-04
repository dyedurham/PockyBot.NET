using System.Collections.Generic;
using Shouldly;
using TestStack.BDDfy;
using Xunit;
using NSubstitute;
using PockyBot.NET.Persistence.Models;
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
        internal void NoCommandsShouldReturnAnError()
        {
            this.Given(x => GivenNoCommands())
                .When(x => WhenGetUserLocationIsCalled())
                .Then(x => ThenItShouldReturn(
                    "Please specify a user or group of users. Possible arguments are me, unset, all, or a list of mentioned users."))
                .BDDfy();
        }

        [Fact]
        internal void MeCommandShouldReturnUsersLocation()
        {
            this.Given(x => GivenCommand("me"))
                .And(x => GivenUserHasLocation("myLocation"))
                .When(x => WhenGetUserLocationIsCalled())
                .Then(x => ThenItShouldReturn("Your location is myLocation"))
                .BDDfy();
        }

        [Fact]
        internal void AllCommandShouldReturnAllUserLocations()
        {
            this.Given(x => GivenCommand("all"))
                .And(x => GivenSomeUsersHaveLocations())
                .When(x => WhenGetUserLocationIsCalled())
                .Then(x => ThenItShouldReturnAllLocations())
                .BDDfy();
        }

        [Fact]
        internal void UnsetCommandShouldReturnUnsetUsers()
        {
            this.Given(x => GivenCommand("unset"))
                .And(x => GivenSomeUsersHaveLocations())
                .When(x => WhenGetUserLocationIsCalled())
                .Then(x => ThenItShouldReturnUnsetUsers())
                .BDDfy();
        }

        [Fact]
        internal void MentionedUsersCommandShouldReturnMentionedUsers()
        {
            this.Given(x => GivenMentionCommand("User2", "User3", "User4"))
                .And(x => GivenSomeMentionedUsersHaveLocations())
                .When(x => WhenGetUserLocationIsCalled())
                .Then(x => ThenItShouldReturnMentionedLocations())
                .BDDfy();
        }

        private void GivenNoCommands()
        {
            _commands = new string[] { };
            _mentionedUsers = new string[] { };
            _meId = "";
        }

        private void GivenCommand(string command)
        {
            _commands = new[] { command };
            _mentionedUsers = new string[] { };
            _meId = "me";
        }
        private void GivenMentionCommand(params string[] userIds)
        {
            _commands = userIds;
            _mentionedUsers = userIds;
            _meId = "me";
        }

        private void GivenUserHasLocation(string location)
        {
            _pockyUserRepository.GetUser(Arg.Any<string>()).Returns(GetUser("me", location));
        }

        private void GivenSomeUsersHaveLocations()
        {
            _pockyUserRepository.GetAllUsersLocations().Returns(new List<PockyUser>
            {
                GetUser("User1", "Location1"),
                GetUser("User2", null),
                GetUser("User3", "Location3"),
                GetUser("User4", "")
            });
        }

        private void GivenSomeMentionedUsersHaveLocations()
        {
            _pockyUserRepository.GetUser(Arg.Is("User2")).Returns(GetUser("User2", null));
            _pockyUserRepository.GetUser(Arg.Is("User3")).Returns(GetUser("User3", "Location3"));
            _pockyUserRepository.GetUser(Arg.Is("User4")).Returns(GetUser("User4", ""));
        }

        private void WhenGetUserLocationIsCalled()
        {
            _result = _subject.GetUserLocation(_commands, _mentionedUsers, _meId);
        }

        private void ThenItShouldReturn(string expected)
        {
            _result.ShouldBe(expected);
        }

        private void ThenItShouldReturnAllLocations()
        {
            _result.ShouldContain("Here are all users' locations:");
            _result.ShouldContain("* **User1**: Location1");
            _result.ShouldContain("* **User3**: Location3");
            _result.ShouldNotContain("User2");
            _result.ShouldNotContain("User4");
        }

        private void ThenItShouldReturnUnsetUsers()
        {
            _result.ShouldContain("Here are the users without a location set:");
            _result.ShouldContain("* User2");
            _result.ShouldContain("* User4");
            _result.ShouldNotContain("User1");
            _result.ShouldNotContain("User3");
        }

        private void ThenItShouldReturnMentionedLocations()
        {
            _result.ShouldContain("Here are the users' locations:");
            _result.ShouldContain("* **User3**: Location3");
            _result.ShouldContain("* **User2**: No location set");
            _result.ShouldContain("* **User4**: No location set");
        }

        private PockyUser GetUser(string username, string location)
        {
            return new PockyUser
            {
                Username = username,
                UserId = username,
                Location = new UserLocation
                {
                    Location = location
                }
            };
        }
    }
}
