using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core.Messages;
using NSubstitute;
using PockyBot.NET.Persistence.Models;
using PockyBot.NET.Persistence.Repositories;
using PockyBot.NET.Services;
using PockyBot.NET.Tests.TestData.Triggers;
using Shouldly;
using TestStack.BDDfy;
using Xunit;
using UserLocation = PockyBot.NET.Services.Triggers.UserLocation;

namespace PockyBot.NET.Tests.Services.Triggers
{
    public class UserLocationTests
    {
        private readonly UserLocation _subject;

        private IPockyUserRepository _pockyUserRepository;
        private IUserLocationService _userLocationService;

        private Message _message;
        private Message _response;
        private string[] _mentionedUsers;

        public UserLocationTests()
        {
            _pockyUserRepository = Substitute.For<IPockyUserRepository>();
            _userLocationService = Substitute.For<IUserLocationService>();
            _subject = new UserLocation(_pockyUserRepository, _userLocationService);
        }

        [Theory]
        [MemberData(nameof(UserLocationTestData.GetUserTestData), MemberType = typeof(UserLocationTestData))]
        internal void TestGetUserLocation(Message message, bool hasPermission, string[] commands)
        {
            this.Given(x => GivenAMessage(message))
                .And(x => GivenUserHasPermission(hasPermission))
                .When(x => WhenRespondingToTheMessage())
                .Then(x => ThenItShouldCallGetUserLocation(commands))
                .BDDfy();
        }

        [Theory]
        [MemberData(nameof(UserLocationTestData.SetUserTestData), MemberType = typeof(UserLocationTestData))]
        internal void TestSetUserLocation(Message message, bool hasPermission, string[] commands)
        {
            this.Given(x => GivenAMessage(message))
                .And(x => GivenUserHasPermission(hasPermission))
                .When(x => WhenRespondingToTheMessage())
                .Then(x => ThenItShouldCallSetUserLocation(commands, hasPermission))
                .BDDfy();
        }

        [Theory]
        [MemberData(nameof(UserLocationTestData.DeleteUserTestData), MemberType = typeof(UserLocationTestData))]
        internal void TestDeleteUserLocation(Message message, bool hasPermission, string[] commands)
        {
            this.Given(x => GivenAMessage(message))
                .And(x => GivenUserHasPermission(hasPermission))
                .When(x => WhenRespondingToTheMessage())
                .Then(x => ThenItShouldCallDeleteUserLocation(commands, hasPermission))
                .BDDfy();
        }

        [Theory]
        [MemberData(nameof(UserLocationTestData.BadCommandTestData), MemberType = typeof(UserLocationTestData))]
        internal void TestBadCommand(Message message, string response)
        {
            this.Given(x => GivenAMessage(message))
                .And(x => GivenUserHasPermission(false))
                .When(x => WhenRespondingToTheMessage())
                .Then(x => ThenItShouldReturn(response))
                .BDDfy();
        }

        private void GivenAMessage(Message message)
        {
            _message = message;
            _mentionedUsers = message.MessageParts
                .Skip(1)
                .Where(x => x.MessageType == MessageType.PersonMention)
                .Select(x => x.UserId).ToArray();
        }

        private void GivenUserHasPermission(bool hasPermission)
        {
            _pockyUserRepository.GetUser(Arg.Any<string>())
                .Returns(x => new PockyUser
                {
                    Username = (string) x[0],
                    UserId = (string) x[0],
                    Roles = new List<Role>
                    {
                        new Role
                        {
                            UserRole = hasPermission ? "CONFIG" : "none"
                        }
                    }
                });
        }

        private async Task WhenRespondingToTheMessage()
        {
            _response = await _subject.Respond(_message);
        }

        private void ThenItShouldCallGetUserLocation(string[] commands)
        {
            _userLocationService.Received(1)
                .GetUserLocation(
                    Arg.Is<string[]>(x => x.SequenceEqual(commands)),
                    Arg.Is<string[]>(x => x.SequenceEqual(_mentionedUsers)),
                    _message.Sender.UserId);
        }

        private void ThenItShouldCallSetUserLocation(string[] commands, bool hasPermission)
        {
            _userLocationService.Received(1)
                .SetUserLocation(
                    Arg.Is<string[]>(x => x.SequenceEqual(commands)),
                    Arg.Is<string[]>(x => x.SequenceEqual(_mentionedUsers)),
                    hasPermission,
                    _message.Sender.UserId);
        }

        private void ThenItShouldCallDeleteUserLocation(string[] commands, bool hasPermission)
        {
            _userLocationService.Received(1)
                .SetUserLocation(
                    Arg.Is<string[]>(x => x.SequenceEqual(commands)),
                    Arg.Is<string[]>(x => x.SequenceEqual(_mentionedUsers)),
                    hasPermission,
                    _message.Sender.UserId);
        }

        private void ThenItShouldReturn(string response)
        {
            _response.Text.ShouldBe(response);
        }
    }
}
