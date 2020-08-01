using System.Threading.Tasks;
using Shouldly;
using TestStack.BDDfy;
using Xunit;
using NSubstitute;
using PockyBot.NET.Persistence.Repositories;
using PockyBot.NET.Services;
using PockyBot.NET.Services.UserLocation;

namespace PockyBot.NET.Tests.Services
{
    public class UserLocationDeleterTests
    {
        private readonly IUserLocationRepository _userLocationRepository;

        private string[] _commands;
        private string[] _mentionedUsers;
        private bool _userIsAdmin;
        private string _meId;

        private readonly IUserLocationDeleter _subject;
        private string _result;

        public UserLocationDeleterTests()
        {
            _userLocationRepository = Substitute.For<IUserLocationRepository>();
            _subject = new UserLocationDeleter(_userLocationRepository);
        }

        [Fact]
        internal void DeleteNoUserShouldReturnErrorForAdmin()
        {
            this.Given(x => GivenCommands())
                .And(x => GivenMentionedUsers())
                .And(x => GivenUserIsAdmin(true))
                .And(x => GivenUserId("me"))
                .When(x => WhenDeleteUserLocationIsCalled())
                .Then(x => ThenItShouldReturn("No user specified. Possible arguments are me or a list of mentioned users."))
                .And(x => ThenItShouldCallDeleteForUsers())
                .BDDfy();
        }

        [Fact]
        internal void DeleteNoUserShouldReturnErrorForNonAdmin()
        {
            this.Given(x => GivenCommands())
                .And(x => GivenMentionedUsers())
                .And(x => GivenUserIsAdmin(false))
                .And(x => GivenUserId("me"))
                .When(x => WhenDeleteUserLocationIsCalled())
                .Then(x => ThenItShouldReturn("No user specified. Possible arguments are me."))
                .And(x => ThenItShouldCallDeleteForUsers())
                .BDDfy();
        }

        [Fact]
        internal void DeleteOnlySelfShouldWork()
        {
            this.Given(x => GivenCommands("me"))
                .And(x => GivenMentionedUsers())
                .And(x => GivenUserIsAdmin(false))
                .And(x => GivenUserId("meId"))
                .When(x => WhenDeleteUserLocationIsCalled())
                .Then(x => ThenItShouldCallDeleteForUsers("meId"))
                .And(x => ThenItShouldReturn("User locations removed."))
                .BDDfy();
        }

        [Fact]
        internal void DeleteOthersForNonAdminShouldReturnError()
        {
            this.Given(x => GivenCommands("user1"))
                .And(x => GivenMentionedUsers("user1", "user2"))
                .And(x => GivenUserIsAdmin(false))
                .And(x => GivenUserId("meId"))
                .When(x => WhenDeleteUserLocationIsCalled())
                .Then(x => ThenItShouldReturn("Permission denied. You may only delete yourself."))
                .And(x => ThenItShouldCallDeleteForUsers())
                .BDDfy();
        }

        [Fact]
        internal void DeleteMentionedUsersShouldWorkForAdmin()
        {
            this.Given(x => GivenCommands("user1"))
                .And(x => GivenMentionedUsers("user1", "user2"))
                .And(x => GivenUserIsAdmin(true))
                .And(x => GivenUserId("meId"))
                .When(x => WhenDeleteUserLocationIsCalled())
                .Then(x => ThenItShouldReturn("User locations removed."))
                .And(x => ThenItShouldCallDeleteForUsers("user1", "user2"))
                .BDDfy();
        }

        [Fact]
        internal void DeleteSelfAndMentionedUsersShouldWorkForAdmin()
        {
            this.Given(x => GivenCommands("user1", "me", "user2"))
                .And(x => GivenMentionedUsers("user1", "user2"))
                .And(x => GivenUserIsAdmin(true))
                .And(x => GivenUserId("meId"))
                .When(x => WhenDeleteUserLocationIsCalled())
                .Then(x => ThenItShouldReturn("User locations removed."))
                .And(x => ThenItShouldCallDeleteForUsers("user1", "user2", "meId"))
                .BDDfy();        }

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

        private async Task WhenDeleteUserLocationIsCalled()
        {
            _result = await _subject.DeleteUserLocation(_commands, _mentionedUsers, _userIsAdmin, _meId);
        }

        private void ThenItShouldCallDeleteForUsers(params string[] userIds)
        {
            if (userIds == null || userIds.Length == 0)
            {
                _userLocationRepository.DidNotReceiveWithAnyArgs().DeleteUserLocation("");
                return;
            }

            foreach (var userId in userIds)
            {
                _userLocationRepository.Received(1).DeleteUserLocation(userId);
            }
        }

        private void ThenItShouldReturn(string response)
        {
            _result.ShouldBe(response);
        }
    }
}
