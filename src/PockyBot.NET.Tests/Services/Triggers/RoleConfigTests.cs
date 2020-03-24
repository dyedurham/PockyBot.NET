using System.Collections.Generic;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core;
using GlobalX.ChatBots.Core.Messages;
using GlobalX.ChatBots.Core.People;
using NSubstitute;
using PockyBot.NET.Persistence.Models;
using PockyBot.NET.Persistence.Repositories;
using PockyBot.NET.Services.Triggers;
using PockyBot.NET.Tests.TestData.Triggers;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace PockyBot.NET.Tests.Services.Triggers
{
    public class RoleConfigTests
    {
        private readonly RoleConfig _subject;

        private readonly IPockyUserRepository _pockyUserRepository;
        private readonly IChatHelper _chatHelper;

        private Message _message;
        private Message _response;

        public RoleConfigTests()
        {
            _pockyUserRepository = Substitute.For<IPockyUserRepository>();
            _chatHelper = Substitute.For<IChatHelper>();
            _chatHelper.People.Returns(Substitute.For<IPersonHandler>());

            _subject = new RoleConfig(_pockyUserRepository, _chatHelper);
        }

        [Theory]
        [MemberData(nameof(RoleConfigTestData.GetRoleConfigTestData), MemberType = typeof(RoleConfigTestData))]
        internal void TestGetRoleConfig(Message message, List<PockyUser> users, string[] responseLines)
        {
            this.Given(x => GivenAMessage(message))
                .And(x => GivenAListOfUserRoles(users))
                .When(x => WhenRespondingToAMessage())
                .Then(x => ThenItShouldReturnAResponse(responseLines))
                .BDDfy();
        }

        [Theory]
        [MemberData(nameof(RoleConfigTestData.UnsuccessfulSetRoleConfigTestData), MemberType = typeof(RoleConfigTestData))]
        internal void TestUnsuccessfulSetRoleConfig(Message message, Person person, PockyUser pockyUser,
            Message response)
        {
            this.Given(x => GivenAMessage(message))
                .And(x => GivenAPerson(person))
                .And(x => GivenAPockyUser(pockyUser))
                .When(x => WhenRespondingToAMessage())
                .Then(x => ThenItShouldReturnAResponse(response))
                .And(x => ThenItShouldNotCallAddRole())
                .BDDfy();
        }

        [Theory]
        [MemberData(nameof(RoleConfigTestData.UnsuccessfulDeleteRoleConfigTestData), MemberType = typeof(RoleConfigTestData))]
        internal void TestUnsuccessfulDeleteRoleConfig(Message message, Person person, PockyUser pockyUser,
            Message response)
        {
            this.Given(x => GivenAMessage(message))
                .And(x => GivenAPerson(person))
                .And(x => GivenAPockyUser(pockyUser))
                .When(x => WhenRespondingToAMessage())
                .Then(x => ThenItShouldReturnAResponse(response))
                .And(x => ThenItShouldNotCallRemoveRole())
                .BDDfy();
        }

        [Theory]
        [MemberData(nameof(RoleConfigTestData.SuccessfulSetRoleConfigTestData), MemberType = typeof(RoleConfigTestData))]
        internal void TestSuccessfulSetRoleConfig(Message message, Person person, PockyUser pockyUser,
            Message response, string userId, string role)
        {
            this.Given(x => GivenAMessage(message))
                .And(x => GivenAPerson(person))
                .And(x => GivenAPockyUser(pockyUser))
                .When(x => WhenRespondingToAMessage())
                .Then(x => ThenItShouldReturnAResponse(response))
                .And(x => ThenItShouldCallAddRole(userId, role))
                .BDDfy();
        }

        [Theory]
        [MemberData(nameof(RoleConfigTestData.SuccessfulDeleteRoleConfigTestData), MemberType = typeof(RoleConfigTestData))]
        internal void TestSuccessfulDeleteRoleConfig(Message message, Person person, PockyUser pockyUser,
            Message response, string userId, string role)
        {
            this.Given(x => GivenAMessage(message))
                .And(x => GivenAPerson(person))
                .And(x => GivenAPockyUser(pockyUser))
                .When(x => WhenRespondingToAMessage())
                .Then(x => ThenItShouldReturnAResponse(response))
                .And(x => ThenItShouldCallRemoveRole(userId, role))
                .BDDfy();
        }

        private void GivenAMessage(Message message)
        {
            _message = message;
        }

        private void GivenAPerson(Person person)
        {
            _chatHelper.People.GetPersonAsync(person.UserId).Returns(Task.FromResult(person));
        }

        private void GivenAPockyUser(PockyUser pockyUser)
        {
            _pockyUserRepository.AddOrUpdateUserAsync(pockyUser.UserId, pockyUser.Username)
                .Returns(Task.FromResult(pockyUser));
        }

        private void GivenAListOfUserRoles(List<PockyUser> users)
        {
            _pockyUserRepository.GetAllUserRolesAsync().Returns(Task.FromResult(users));
        }

        private async Task WhenRespondingToAMessage()
        {
            _response = await _subject.Respond(_message);
        }

        private void ThenItShouldReturnAResponse(Message response)
        {
            _response.ShouldNotBeNull();
            _response.Text.ShouldBe(response.Text);
        }

        private void ThenItShouldReturnAResponse(string[] responseLines)
        {
            _response.ShouldNotBeNull();
            foreach (var line in responseLines)
            {
                _response.Text.ShouldContain(line);
            }
        }

        private void ThenItShouldNotCallAddRole()
        {
            _pockyUserRepository.DidNotReceiveWithAnyArgs().AddRoleAsync(Arg.Any<string>(), Arg.Any<string>());
        }

        private void ThenItShouldNotCallRemoveRole()
        {
            _pockyUserRepository.DidNotReceiveWithAnyArgs().RemoveRoleAsync(Arg.Any<string>(), Arg.Any<string>());
        }

        private void ThenItShouldCallAddRole(string userId, string role)
        {
            _pockyUserRepository.Received(1).AddRoleAsync(userId, role);
        }

        private void ThenItShouldCallRemoveRole(string userId, string role)
        {
            _pockyUserRepository.Received(1).RemoveRoleAsync(userId, role);
        }
    }
}
