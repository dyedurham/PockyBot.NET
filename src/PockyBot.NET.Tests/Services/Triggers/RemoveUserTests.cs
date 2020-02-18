using System.Collections.Generic;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core.Messages;
using NSubstitute;
using PockyBot.NET.Persistence.Models;
using PockyBot.NET.Persistence.Repositories;
using PockyBot.NET.Services.Triggers;
using Shouldly;
using TestStack.BDDfy;
using Xunit;
using Peg = PockyBot.NET.Persistence.Models.Peg;

namespace PockyBot.NET.Tests.Services.Triggers
{
    public class RemoveUserTests
    {
        private readonly RemoveUser _subject;
        private readonly IPockyUserRepository _pockyUserRepository;

        private Message _message;
        private Message _response;

        private const string UserToRemoveUsername = "RemoveMe";
        private const string UserToRemoveId = "RemoveMeId";

        public RemoveUserTests()
        {
            _pockyUserRepository = Substitute.For<IPockyUserRepository>();
            _subject = new RemoveUser(_pockyUserRepository);
        }

        [Fact]
        public void ItShouldRemoveAMentionedUser()
        {
            this.Given(x => GivenARemoveUserByMentionMessage())
                .And(x => GivenThePockyUserRepositoryReturnsTheUserBasedOnUserId(false))
                .When(x => WhenRespondIsCalled())
                .Then(x => ThenItShouldRemoveTheUser())
                .And(x => ThenItShouldRespondWithMessage($"User '{UserToRemoveUsername}' has been removed."))
                .BDDfy();
        }

        [Fact]
        public void ItShouldRemoveAUserByUsername()
        {
            this.Given(x => GivenARemoveUserByUsernameMessage(UserToRemoveUsername))
                .And(x => GivenThePockyUserRepositoryReturnsUsersByUsername(1, false))
                .When(x => WhenRespondIsCalled())
                .Then(x => ThenItShouldRemoveTheUser())
                .And(x => ThenItShouldRespondWithMessage($"User '{UserToRemoveUsername}' has been removed."))
                .BDDfy();
        }

        [Fact]
        public void ItShouldDisplayAMessageWhenMentioningMoreThanOneUser()
        {
            this.Given(x => GivenARemoveUserByMentionMessageWithMoreThanOneMention())
                .When(x => WhenRespondIsCalled())
                .And(x => ThenItShouldRespondWithMessage("Can only remove one user at a time."))
                .BDDfy();
        }

        [Fact]
        public void ItShouldDisplayAMessageWhenAUserCannotBeFoundByUsername()
        {
            this.Given(x => GivenARemoveUserByUsernameMessage(UserToRemoveUsername))
                .And(x => GivenThePockyUserRepositoryReturnsUsersByUsername(0, false))
                .When(x => WhenRespondIsCalled())
                .And(x => ThenItShouldRespondWithMessage($"User with username '{UserToRemoveUsername}', not found, cannot be removed"))
                .BDDfy();
        }

        [Fact]
        public void ItShouldDisplayAMessageWhenMoreThanOneUserWithTheUsernameIsFound()
        {
            this.Given(x => GivenARemoveUserByUsernameMessage(UserToRemoveUsername))
                .And(x => GivenThePockyUserRepositoryReturnsUsersByUsername(2, false))
                .When(x => WhenRespondIsCalled())
                .And(x => ThenItShouldRespondWithMessage($"More than one user with the username '{UserToRemoveUsername}' found, cannot remove them."))
                .BDDfy();
        }

        [Fact]
        public void ItShouldDisplayAMessageWhenAttemptingToRemoveAGroupMention()
        {
            this.Given(x => GivenARemoveUserByMentionMessageWithAGroupMention())
                .When(x => WhenRespondIsCalled())
                .And(x => ThenItShouldRespondWithMessage("Invalid message type: cannot remove a group mention."))
                .BDDfy();
        }

        [Fact]
        public void ItShouldDisplayAMessageWhenAttemptingToRemoveAUserWithOutstandingPegsByUsername()
        {
            this.Given(x => GivenARemoveUserByMentionMessage())
                .And(x => GivenThePockyUserRepositoryReturnsTheUserBasedOnUserId(true))
                .When(x => WhenRespondIsCalled())
                .And(x => ThenItShouldRespondWithMessage($"Cannot remove user '{UserToRemoveUsername}', they still have outstanding pegs"))
                .BDDfy();
        }

        [Fact]
        public void ItShouldDisplayAMessageWhenAttemptingToRemoveAUserWithOutstandingPegsByMention()
        {
            this.Given(x => GivenARemoveUserByUsernameMessage(UserToRemoveUsername))
                .And(x => GivenThePockyUserRepositoryReturnsUsersByUsername(1, true))
                .When(x => WhenRespondIsCalled())
                .And(x => ThenItShouldRespondWithMessage($"Cannot remove user '{UserToRemoveUsername}', they still have outstanding pegs"))
                .BDDfy();
        }

        private void GivenARemoveUserByMentionMessage()
        {
            _message = new Message
            {
                MessageParts = new []
                {
                    new MessagePart
                    {
                        MessageType = MessageType.PersonMention,
                        Text = "PockyBot",
                        UserId = "TestPockyBot"
                    },
                    new MessagePart
                    {
                        MessageType = MessageType.Text,
                        Text = "removeuser "
                    },
                    new MessagePart
                    {
                        MessageType = MessageType.PersonMention,
                        Text = UserToRemoveUsername,
                        UserId = UserToRemoveId
                    }
                }
            };
        }

        private void GivenARemoveUserByMentionMessageWithAGroupMention()
        {
            _message = new Message
            {
                MessageParts = new []
                {
                    new MessagePart
                    {
                        MessageType = MessageType.PersonMention,
                        Text = "PockyBot",
                        UserId = "TestPockyBot"
                    },
                    new MessagePart
                    {
                        MessageType = MessageType.Text,
                        Text = "removeuser "
                    },
                    new MessagePart
                    {
                        MessageType = MessageType.GroupMention,
                        Text = UserToRemoveUsername,
                        UserId = UserToRemoveId
                    }
                }
            };
        }

        private void GivenARemoveUserByMentionMessageWithMoreThanOneMention()
        {
            _message = new Message
            {
                MessageParts = new []
                {
                    new MessagePart
                    {
                        MessageType = MessageType.PersonMention,
                        Text = "PockyBot",
                        UserId = "TestPockyBot"
                    },
                    new MessagePart
                    {
                        MessageType = MessageType.Text,
                        Text = "removeuser "
                    },
                    new MessagePart
                    {
                        MessageType = MessageType.PersonMention,
                        Text = UserToRemoveUsername,
                        UserId = UserToRemoveId
                    },
                    new MessagePart
                    {
                        MessageType = MessageType.PersonMention,
                        Text = "ExtraUser",
                        UserId = "ExtraUserId"
                    }
                }
            };
        }

        private void GivenARemoveUserByUsernameMessage(string command)
        {
            _message = new Message
            {
                MessageParts = new []
                {
                    new MessagePart
                    {
                        MessageType = MessageType.PersonMention,
                        Text = "PockyBot",
                        UserId = "TestPockyBot"
                    },
                    new MessagePart
                    {
                        MessageType = MessageType.Text,
                        Text = "removeuser " + command
                    }
                }
            };
        }

        private void GivenThePockyUserRepositoryReturnsTheUserBasedOnUserId(bool hasOutstandingPegs)
        {
            _pockyUserRepository.GetUser(UserToRemoveId).Returns(new PockyUser
            {
                UserId = UserToRemoveId,
                Username = UserToRemoveUsername,
                PegsGiven = hasOutstandingPegs ? new List<Peg>{new Peg()} : new List<Peg>(),
                PegsReceived = new List<Peg>()
            });
        }

        private void GivenThePockyUserRepositoryReturnsUsersByUsername(int count, bool hasOutstandingPegs)
        {
            var pockyUsers = new List<PockyUser>();
            for (var i = 0; i < count; i++)
            {
                pockyUsers.Add(new PockyUser
                {
                    UserId = UserToRemoveId,
                    Username = UserToRemoveUsername,
                    PegsGiven = hasOutstandingPegs ? new List<Peg>{new Peg()} : new List<Peg>(),
                    PegsReceived = new List<Peg>()
                });
            }
            _pockyUserRepository.GetUsersByUsername(UserToRemoveUsername).Returns(pockyUsers);
        }

        private async Task WhenRespondIsCalled()
        {
            _response = await _subject.Respond(_message);
        }

        private void ThenItShouldRemoveTheUser()
        {
            _pockyUserRepository.Received().RemoveUser(Arg.Is<PockyUser>(x =>
                x.UserId == UserToRemoveId &&
                x.Username == UserToRemoveUsername
            ));
        }

        private void ThenItShouldRespondWithMessage(string message)
        {
            _response.Text.ShouldBe(message);
        }
    }
}
