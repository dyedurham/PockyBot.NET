using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core.Messages;
using GlobalX.ChatBots.Core.People;
using Microsoft.Extensions.Options;
using NSubstitute;
using PockyBot.NET.Configuration;
using PockyBot.NET.Constants;
using PockyBot.NET.Persistence.Models;
using PockyBot.NET.Persistence.Repositories;
using PockyBot.NET.Services.Triggers;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace PockyBot.NET.Tests.Services.Triggers
{
    public class HelpTests
    {
        private readonly Help _subject;
        private readonly IPockyUserRepository _pockyUserRepository;

        private const string BotName = "Pocky";
        private const string SenderId = "testSender";
        private readonly List<UserRole> noRoles = new List<UserRole>();
        private readonly List<UserRole> adminRole = new List<UserRole>{new UserRole{Role = Role.Admin}};
        private readonly List<IHelpMessageTrigger> triggers = new List<IHelpMessageTrigger>();

        private Message _message;
        private Message _result;

        public HelpTests()
        {
            var pockyUserSettings = Substitute.For<IOptions<PockyBotSettings>>();
            pockyUserSettings.Value.Returns(new PockyBotSettings
            {
                BotName = BotName
            });
            _pockyUserRepository = Substitute.For<IPockyUserRepository>();
            _subject = new Help(_pockyUserRepository, pockyUserSettings, triggers);
        }

        [Fact]
        public void ItShouldShowAListOfCommandsToANonAdminUser()
        {
            this.Given(x => GivenAHelpMessage(""))
                .And(x => GivenAListOfTriggers())
                .And(x => GivenTheUserHasRoles(noRoles))
                .When(x => WhenRespondIsCalled())
                .Then(x => ThenItShouldShowAListOfNonAdminCommands())
                .BDDfy();
        }

        [Fact]
        public void ItShouldShowAListOfCommandsToANonAdminUserInADirectMessage()
        {
            this.Given(x => GivenADirectMessageHelpMessage(""))
                .And(x => GivenAListOfTriggers())
                .And(x => GivenTheUserHasRoles(noRoles))
                .When(x => WhenRespondIsCalled())
                .Then(x => ThenItShouldShowAListOfNonAdminCommands())
                .BDDfy();
        }

        [Fact]
        public void ItShouldShowAListOfAdminCommandsToAnAdminUser()
        {
            this.Given(x => GivenAHelpMessage(""))
                .And(x => GivenAListOfTriggers())
                .And(x => GivenTheUserHasRoles(adminRole))
                .When(x => WhenRespondIsCalled())
                .Then(x => ThenItShouldShowAListOfAdminCommands())
                .BDDfy();
        }

        [Fact]
        public void ItShouldShowTheDefaultHelpMessageWhenTheCommandDoesNotExist()
        {
            this.Given(x => GivenAHelpMessage("bogusCommand"))
                .And(x => GivenAListOfTriggers())
                .And(x => GivenTheUserHasRoles(noRoles))
                .When(x => WhenRespondIsCalled())
                .Then(x => ThenItShouldShowTheDefaultHelpMessage())
                .BDDfy();
        }

        [Fact]
        public void ItShouldShowTheDefaultHelpMessageWhenTheCommandDoesNotExistInADirectMessage()
        {
            this.Given(x => GivenADirectMessageHelpMessage("bogusCommand"))
                .And(x => GivenAListOfTriggers())
                .And(x => GivenTheUserHasRoles(noRoles))
                .When(x => WhenRespondIsCalled())
                .Then(x => ThenItShouldShowTheDefaultHelpMessage())
                .BDDfy();
        }

        [Theory]
        [InlineData(Commands.Peg)]
        [InlineData(Commands.Status)]
        public void ItShouldShowTheHelpMessageForNonAdminCommands(string command)
        {
            this.Given(x => GivenAHelpMessage(command))
                .And(x => GivenAListOfTriggers())
                .And(x => GivenTheUserHasRoles(noRoles))
                .When(x => WhenRespondIsCalled())
                .Then(x => ThenItShouldShowTheCommandHelpMessage(command))
                .BDDfy();
        }

        [Theory]
        [InlineData(Commands.Peg)]
        [InlineData(Commands.Status)]
        public void ItShouldShowTheHelpMessageForNonAdminCommandsInADirectMessage(string command)
        {
            this.Given(x => GivenADirectMessageHelpMessage(command))
                .And(x => GivenAListOfTriggers())
                .And(x => GivenTheUserHasRoles(noRoles))
                .When(x => WhenRespondIsCalled())
                .Then(x => ThenItShouldShowTheCommandHelpMessage(command))
                .BDDfy();
        }

        [Theory]
        [InlineData(Commands.Finish, Role.Finish)]
        [InlineData(Commands.Finish, Role.Admin)]
        [InlineData(Commands.Reset, Role.Reset)]
        [InlineData(Commands.Reset, Role.Admin)]
        internal void ItShouldShowTheHelpMessageForAdminCommandsToAdminUsers(string command, Role userRole)
        {
            this.Given(x => GivenAHelpMessage(command))
                .And(x => GivenAListOfTriggers())
                .And(x => GivenTheUserHasRoles(new List<UserRole>{new UserRole{Role = userRole}}))
                .When(x => WhenRespondIsCalled())
                .Then(x => ThenItShouldShowTheCommandHelpMessage(command))
                .BDDfy();
        }

        [Theory]
        [InlineData(Commands.Finish)]
        [InlineData(Commands.Reset)]
        public void ItShouldShowTheDefaultHelpMessageForAdminCommandsToNonAdminUsers(string command)
        {
            this.Given(x => GivenAHelpMessage(command))
                .And(x => GivenAListOfTriggers())
                .And(x => GivenTheUserHasRoles(noRoles))
                .When(x => WhenRespondIsCalled())
                .Then(x => ThenItShouldShowTheDefaultHelpMessage())
                .BDDfy();
        }

        private void GivenAHelpMessage(string command)
        {
            _message = new Message
            {
                Sender = new Person
                {
                    UserId = SenderId
                },
                MessageParts = new []
                {
                    new MessagePart
                    {
                        MessageType = MessageType.PersonMention,
                        Text = "PockyBot",
                        UserId = BotName
                    },
                    new MessagePart
                    {
                        MessageType = MessageType.Text,
                        Text = "help " + command
                    }
                }
            };
        }

        private void GivenADirectMessageHelpMessage(string command)
        {
            _message = new Message
            {
                Sender = new Person
                {
                    UserId = SenderId
                },
                MessageParts = new []
                {
                    new MessagePart
                    {
                        MessageType = MessageType.Text,
                        Text = "help " + command
                    }
                }
            };
        }

        private void GivenAListOfTriggers()
        {
            var peg = Substitute.For<IHelpMessageTrigger>();
            peg.Command.Returns(Commands.Peg);
            peg.Permissions.Returns(Array.Empty<Role>());
            peg.GetHelpMessage(BotName, Arg.Any<PockyUser>()).Returns($"@{BotName} {Commands.Peg}");
            triggers.Add(peg);

            var status = Substitute.For<IHelpMessageTrigger>();
            status.Command.Returns(Commands.Status);
            status.Permissions.Returns(Array.Empty<Role>());
            status.GetHelpMessage(BotName, Arg.Any<PockyUser>()).Returns($"@{BotName} {Commands.Status}");
            triggers.Add(status);

            var reset = Substitute.For<IHelpMessageTrigger>();
            reset.Command.Returns(Commands.Reset);
            reset.Permissions.Returns(new [] { Role.Reset, Role.Admin });
            reset.GetHelpMessage(BotName, Arg.Any<PockyUser>()).Returns($"@{BotName} {Commands.Reset}");
            triggers.Add(reset);

            var finish = Substitute.For<IHelpMessageTrigger>();
            finish.Command.Returns(Commands.Finish);
            finish.Permissions.Returns(new [] { Role.Finish, Role.Admin });
            finish.GetHelpMessage(BotName, Arg.Any<PockyUser>()).Returns($"@{BotName} {Commands.Finish}");
            triggers.Add(finish);
        }

        private void GivenTheUserHasRoles(List<UserRole> roles)
        {
            _pockyUserRepository.GetUser(SenderId).Returns(new PockyUser
            {
                UserId = SenderId,
                Roles = roles
            });
        }

        private async Task WhenRespondIsCalled()
        {
            _result = await _subject.Respond(_message);
        }

        private void ThenItShouldShowAListOfNonAdminCommands()
        {
            _result.Text.ShouldStartWith("## What I can do (List of Commands)");
            _result.Text.ShouldContain($"* {Commands.Peg}");
            _result.Text.ShouldContain($"* {Commands.Status}");
            _result.Text.ShouldContain($"For more information on a command type `@{BotName} help command-name` or direct message me with `help command-name`");
            _result.Text.ShouldContain("I am still being worked on, so more features to come.");

            _result.Text.ShouldNotContain($"* {Commands.Reset}");
            _result.Text.ShouldNotContain($"* {Commands.Finish}");
        }

        private void ThenItShouldShowAListOfAdminCommands()
        {
            _result.Text.ShouldStartWith("## What I can do (List of Commands)");
            _result.Text.ShouldContain($"* {Commands.Peg}");
            _result.Text.ShouldContain($"* {Commands.Status}");
            _result.Text.ShouldContain($"* {Commands.Reset}");
            _result.Text.ShouldContain($"* {Commands.Finish}");
            _result.Text.ShouldContain($"For more information on a command type `@{BotName} help command-name` or direct message me with `help command-name`");
            _result.Text.ShouldContain("I am still being worked on, so more features to come.");
        }

        private void ThenItShouldShowTheDefaultHelpMessage()
        {
            _result.Text.ShouldBe($"Command not found. To see a full list of commands type `@{BotName} help` or direct message me with `help`.");
        }

        private void ThenItShouldShowTheCommandHelpMessage(string command)
        {
            _result.Text.ShouldContain($"@{BotName} {command}");
        }
    }
}
