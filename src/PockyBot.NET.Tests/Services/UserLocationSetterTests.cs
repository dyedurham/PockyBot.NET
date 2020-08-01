using System.Threading.Tasks;
using GlobalX.ChatBots.Core;
using NSubstitute;
using PockyBot.NET.Persistence.Models;
using PockyBot.NET.Persistence.Repositories;
using PockyBot.NET.Services.UserLocation;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace PockyBot.NET.Tests.Services
{
    public class UserLocationSetterTests
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IUserLocationRepository _userLocationRepository;

        private string[] _commands;
        private string[] _mentionedUsers;
        private bool _userIsAdmin;
        private string _meId;

        private readonly IUserLocationSetter _subject;
        private string _result;

        public UserLocationSetterTests()
        {
            var pockyUserRepository = Substitute.For<IPockyUserRepository>();
            pockyUserRepository.GetUser(Arg.Any<string>()).Returns(x => new PockyUser
            {
                UserId = (string) x[0]
            });

            _locationRepository = Substitute.For<ILocationRepository>();
            _userLocationRepository = Substitute.For<IUserLocationRepository>();
            var chatHelper = Substitute.For<IChatHelper>();
            _subject = new UserLocationSetter(pockyUserRepository, _locationRepository, _userLocationRepository, chatHelper);
        }

        [Theory]
        [InlineData(true, "User or location was not specified. Possible arguments are \"<location> me\" or \"<location> <list of users>\"")]
        [InlineData(false, "User or location was not specified. Argument must be in the form of \"<location> me\"")]
        internal void SetNoUserOrLocationShouldReturnError(bool isAdmin, string response)
        {
            this.Given(x => GivenCommands("me"))
                .And(x => GivenUserIsAdmin(isAdmin))
                .When(x => WhenSetUserLocationIsCalled())
                .Then(x => ThenItShouldReturn(response))
                .And(x => ThenItShouldNotCallSet())
                .BDDfy();
        }

        [Fact]
        internal void SetNonexistentLocationShouldReturnError()
        {
            this.Given(x => GivenCommands("location1", "me"))
                .And(x => GivenLocations2And3AreValid())
                .When(x => WhenSetUserLocationIsCalled())
                .Then(x => ThenItShouldReturn(
                    "Location location1 does not exist. Valid values are: location2, location3."))
                .And(x => ThenItShouldNotCallSet())
                .BDDfy();
        }

        [Fact]
        internal void SetOthersForNonAdminShouldReturnError()
        {
            this.Given(x => GivenCommands("location2", "user1", "me"))
                .And(x => GivenLocations2And3AreValid())
                .And(x => GivenMentionedUsers("user1"))
                .And(x => GivenUserIsAdmin(false))
                .When(x => WhenSetUserLocationIsCalled())
                .Then(x => ThenItShouldReturn("Permission denied. You may only set location for yourself."))
                .And(x => ThenItShouldNotCallSet())
                .BDDfy();
        }

        [Fact]
        internal void SetLocationForMeShouldWork()
        {
            this.Given(x => GivenCommands("location2", "me"))
                .And(x => GivenLocations2And3AreValid())
                .And(x => GivenUserId("meId"))
                .And(x => GivenUserIsAdmin(false))
                .When(x => WhenSetUserLocationIsCalled())
                .Then(x => ThenItShouldReturn("User locations added."))
                .And(x => ThenItShouldCallSet("location2", "meId"))
                .BDDfy();
        }

        [Fact]
        internal void SetOthersByAdminShouldWork()
        {
            this.Given(x => GivenCommands("location2", "user1", "user2"))
                .And(x => GivenLocations2And3AreValid())
                .And(x => GivenUserId("meId"))
                .And(x => GivenMentionedUsers("user1", "user2"))
                .And(x => GivenUserIsAdmin(true))
                .When(x => WhenSetUserLocationIsCalled())
                .Then(x => ThenItShouldReturn("User locations added."))
                .And(x => ThenItShouldCallSet("location2", "user1", "user2"))
                .BDDfy();
        }

        [Fact]
        internal void SetOthersAndMeByAdminShouldWork()
        {
            this.Given(x => GivenCommands("location2", "user1", "user2", "me"))
                .And(x => GivenLocations2And3AreValid())
                .And(x => GivenUserId("meId"))
                .And(x => GivenMentionedUsers("user1", "user2"))
                .And(x => GivenUserIsAdmin(true))
                .When(x => WhenSetUserLocationIsCalled())
                .Then(x => ThenItShouldReturn("User locations added."))
                .And(x => ThenItShouldCallSet("location2", "user1", "user2", "meId"))
                .BDDfy();
        }

        private void GivenCommands(params string[] commands)
        {
            _commands = commands;
        }

        private void GivenMentionedUsers(params string[] mentionedUsers)
        {
            _mentionedUsers = mentionedUsers;
        }

        private void GivenUserIsAdmin(bool isAdmin)
        {
            _userIsAdmin = isAdmin;
        }

        private void GivenUserId(string userId)
        {
            _meId = userId;
        }

        private void GivenLocations2And3AreValid()
        {
            _locationRepository.GetAllLocations().Returns(new[] { "location2", "location3" });
        }

        private async Task WhenSetUserLocationIsCalled()
        {
            _result = await _subject.SetUserLocation(_commands, _mentionedUsers, _userIsAdmin, _meId);
        }

        private void ThenItShouldNotCallSet()
        {
            _userLocationRepository.DidNotReceiveWithAnyArgs().UpsertUserLocation(Arg.Any<PockyUser>(), "");
        }

        private void ThenItShouldReturn(string response)
        {
            _result.ShouldBe(response);
        }

        private void ThenItShouldCallSet(string location, params string[] userIds)
        {
            foreach (var userId in userIds)
            {
                _userLocationRepository.Received(1)
                    .UpsertUserLocation(Arg.Is<PockyUser>(user => user.UserId == userId), location);
            }
        }
    }
}
