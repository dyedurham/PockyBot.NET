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
            await _subject.Respond(_message);
        }

        private void ThenItShouldCallGetUserLocation(string[] commands)
        {
            _userLocationService.Received(1)
                .GetUserLocation(
                    Arg.Is<string[]>(x => x.SequenceEqual(commands)),
                    Arg.Is<string[]>(x => x.SequenceEqual(_mentionedUsers)),
                    _message.Sender.UserId);
        }

    }
}
