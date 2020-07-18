using System.Collections.Generic;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core.People;
using NSubstitute;
using PockyBot.NET.Persistence.Models;
using PockyBot.NET.Persistence.Repositories;
using PockyBot.NET.Services;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace PockyBot.NET.Tests.Services
{
    public class UsernameUpdaterTests
    {
        private readonly IUsernameUpdater _subject;
        private readonly IPersonHandler _personHandler;
        private readonly IPockyUserRepository _pockyUserRepository;

        private readonly List<PockyUser> _inputPockyUsers;

        private List<PockyUser> _resultPockyUsers;

        public UsernameUpdaterTests()
        {
            _personHandler = Substitute.For<IPersonHandler>();
            _pockyUserRepository = Substitute.For<IPockyUserRepository>();
            _subject = new UsernameUpdater(_personHandler, _pockyUserRepository);

            _inputPockyUsers = new List<PockyUser>();
        }

        [Fact]
        public void ItShouldUpdateAUsersUsername()
        {
            this.Given(x => GivenAUser("someUser", "previous", "current"))
                .When(x => WhenUpdateUsernamesIsCalled())
                .Then(x => ThenItShouldUpdateTheUsersUsername("someUser", "current"))
                .BDDfy();
        }

        [Fact]
        public void ItShouldNotUpdateTheUsernameIfItHasNotChanged()
        {
            this.Given(x => GivenAUser("someUser", "unchanged", "unchanged"))
                .When(x => WhenUpdateUsernamesIsCalled())
                .Then(x => ThenItShouldNotUpdateTheUsersUsername("someUser"))
                .BDDfy();
        }

        [Fact]
        public void ItShouldUpdateMultipleUsers()
        {
            this.Given(x => GivenAUser("user1", "unchanged", "unchanged"))
                .And(x => GivenAUser("user2", "user2p", "user2c"))
                .And(x => GivenAUser("user3", "user3p", "user3c"))
                .When(x => WhenUpdateUsernamesIsCalled())
                .Then(x => ThenItShouldNotUpdateTheUsersUsername("user1"))
                .And(x => ThenItShouldUpdateTheUsersUsername("user2", "user2c"))
                .And(x => ThenItShouldUpdateTheUsersUsername("user3", "user3c"))
                .BDDfy();
        }

        private void GivenAUser(string userId, string previousUsername, string currentUsername)
        {
            _inputPockyUsers.Add(new PockyUser
            {
                UserId = userId,
                Username = previousUsername
            });

            _personHandler.GetPersonAsync(userId).Returns(Task.FromResult(new Person
            {
                UserId = userId,
                Username = currentUsername
            }));
        }

        private async Task WhenUpdateUsernamesIsCalled()
        {
            _resultPockyUsers = await _subject.UpdateUsernames(_inputPockyUsers);
        }

        private void ThenItShouldUpdateTheUsersUsername(string userId, string username)
        {
            _resultPockyUsers.Find(x => x.UserId == userId).Username.ShouldBe(username);
            _pockyUserRepository.Received().UpdateUsernameAsync(userId, username);
        }

        private void ThenItShouldNotUpdateTheUsersUsername(string userId)
        {
            _pockyUserRepository.Received(0).UpdateUsernameAsync(userId, Arg.Any<string>());
        }
    }
}
