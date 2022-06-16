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
        private readonly List<UserRole> _noRoles = new List<UserRole>();
        private readonly List<UserRole> _adminRole = new List<UserRole>{new UserRole{Role = Role.Admin}};

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
            var configRepository = Substitute.For<IConfigRepository>();
            _subject = new Help(_pockyUserRepository, pockyUserSettings, configRepository);
        }

        [Fact]
        public void ItShouldShowAListOfCommandsToANonAdminUser()
        {
            this.Given(x => GivenAHelpMessage(""))
                .And(x => GivenTheUserHasRoles(_noRoles))
                .When(x => WhenRespondIsCalled())
                .Then(x => ThenItShouldShowAListOfNonAdminCommands())
                .BDDfy();
        }

        [Fact]
        public void ItShouldShowAListOfCommandsToANonAdminUserInADirectMessage()
        {
            this.Given(x => GivenADirectMessageHelpMessage(""))
                .And(x => GivenTheUserHasRoles(_noRoles))
                .When(x => WhenRespondIsCalled())
                .Then(x => ThenItShouldShowAListOfNonAdminCommands())
                .BDDfy();
        }

        [Fact]
        public void ItShouldShowAListOfAdminCommandsToAnAdminUser()
        {
            this.Given(x => GivenAHelpMessage(""))
                .And(x => GivenTheUserHasRoles(_adminRole))
                .When(x => WhenRespondIsCalled())
                .Then(x => ThenItShouldShowAListOfAdminCommands())
                .BDDfy();
        }

        [Fact]
        public void ItShouldShowTheDefaultHelpMessageWhenTheCommandDoesNotExist()
        {
            this.Given(x => GivenAHelpMessage("bogusCommand"))
                .And(x => GivenTheUserHasRoles(_noRoles))
                .When(x => WhenRespondIsCalled())
                .Then(x => ThenItShouldShowTheDefaultHelpMessage())
                .BDDfy();
        }

        [Fact]
        public void ItShouldShowTheDefaultHelpMessageWhenTheCommandDoesNotExistInADirectMessage()
        {
            this.Given(x => GivenADirectMessageHelpMessage("bogusCommand"))
                .And(x => GivenTheUserHasRoles(_noRoles))
                .When(x => WhenRespondIsCalled())
                .Then(x => ThenItShouldShowTheDefaultHelpMessage())
                .BDDfy();
        }

        [Theory]
        [InlineData(Commands.Peg)]
        [InlineData(Commands.Status)]
        [InlineData(Commands.Keywords)]
        [InlineData(Commands.Ping)]
        [InlineData(Commands.Welcome)]
        [InlineData(Commands.Rotation)]
        [InlineData(Commands.LocationConfig)]
        public void ItShouldShowTheHelpMessageForNonAdminCommands(string command)
        {
            this.Given(x => GivenAHelpMessage(command))
                .And(x => GivenTheUserHasRoles(_noRoles))
                .When(x => WhenRespondIsCalled())
                .Then(x => ThenItShouldShowTheCommandHelpMessage(command))
                .BDDfy();
        }

        [Theory]
        [InlineData(Commands.Peg)]
        [InlineData(Commands.Status)]
        [InlineData(Commands.Keywords)]
        [InlineData(Commands.Ping)]
        [InlineData(Commands.Welcome)]
        [InlineData(Commands.Rotation)]
        [InlineData(Commands.LocationConfig)]
        public void ItShouldShowTheHelpMessageForNonAdminCommandsInADirectMessage(string command)
        {
            this.Given(x => GivenADirectMessageHelpMessage(command))
                .And(x => GivenTheUserHasRoles(_noRoles))
                .When(x => WhenRespondIsCalled())
                .Then(x => ThenItShouldShowTheCommandHelpMessage(command))
                .BDDfy();
        }

        [Theory]
        [InlineData(Commands.Finish, Role.Finish)]
        [InlineData(Commands.Finish, Role.Admin)]
        [InlineData(Commands.Reset, Role.Reset)]
        [InlineData(Commands.Reset, Role.Admin)]
        [InlineData(Commands.StringConfig, Role.Admin)]
        [InlineData(Commands.StringConfig, Role.Config)]
        [InlineData(Commands.RoleConfig, Role.Admin)]
        [InlineData(Commands.RoleConfig, Role.Config)]
        [InlineData(Commands.LocationWeight, Role.Admin)]
        [InlineData(Commands.LocationWeight, Role.Config)]
        [InlineData(Commands.RemoveUser, Role.Admin)]
        [InlineData(Commands.RemoveUser, Role.RemoveUser)]
        [InlineData(Commands.Results, Role.Admin)]
        [InlineData(Commands.Results, Role.Results)]
        [InlineData(Commands.NumberConfig, Role.Admin)]
        [InlineData(Commands.NumberConfig, Role.Config)]
        internal void ItShouldShowTheHelpMessageForAdminCommandsToAdminUsers(string command, Role userRole)
        {
            this.Given(x => GivenAHelpMessage(command))
                .And(x => GivenTheUserHasRoles(new List<UserRole>{new UserRole{Role = userRole}}))
                .When(x => WhenRespondIsCalled())
                .Then(x => ThenItShouldShowTheCommandHelpMessage(command))
                .BDDfy();
        }

        [Theory]
        [InlineData(Commands.Finish)]
        [InlineData(Commands.Reset)]
        [InlineData(Commands.StringConfig)]
        [InlineData(Commands.RoleConfig)]
        [InlineData(Commands.LocationWeight)]
        [InlineData(Commands.RemoveUser)]
        [InlineData(Commands.Results)]
        [InlineData(Commands.NumberConfig)]
        public void ItShouldShowTheDefaultHelpMessageForAdminCommandsToNonAdminUsers(string command)
        {
            this.Given(x => GivenAHelpMessage(command))
                .And(x => GivenTheUserHasRoles(_noRoles))
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
            _result.Text.ShouldContain($"* {Commands.Keywords}");
            _result.Text.ShouldContain($"* {Commands.Ping}");
            _result.Text.ShouldContain($"* {Commands.Welcome}");
            _result.Text.ShouldContain($"* {Commands.Rotation}");
            _result.Text.ShouldContain($"* {Commands.LocationConfig}");
            _result.Text.ShouldContain($"* {Commands.UserLocation}");
            _result.Text.ShouldContain($"For more information on a command type `@{BotName} help command-name` or direct message me with `help command-name`");
            _result.Text.ShouldContain("I am still being worked on, so more features to come.");

            _result.Text.ShouldNotContain($"* {Commands.Reset}");
            _result.Text.ShouldNotContain($"* {Commands.Finish}");
            _result.Text.ShouldNotContain($"* {Commands.StringConfig}");
            _result.Text.ShouldNotContain($"* {Commands.RoleConfig}");
            _result.Text.ShouldNotContain($"* {Commands.LocationWeight}");
            _result.Text.ShouldNotContain($"* {Commands.RemoveUser}");
            _result.Text.ShouldNotContain($"* {Commands.Results}");
            _result.Text.ShouldNotContain($"* {Commands.NumberConfig}");
        }

        private void ThenItShouldShowAListOfAdminCommands()
        {
            _result.Text.ShouldStartWith("## What I can do (List of Commands)");
            _result.Text.ShouldContain($"* {Commands.Peg}");
            _result.Text.ShouldContain($"* {Commands.Status}");
            _result.Text.ShouldContain($"* {Commands.Keywords}");
            _result.Text.ShouldContain($"* {Commands.Ping}");
            _result.Text.ShouldContain($"* {Commands.Welcome}");
            _result.Text.ShouldContain($"* {Commands.Rotation}");
            _result.Text.ShouldContain($"* {Commands.LocationConfig}");
            _result.Text.ShouldContain($"* {Commands.UserLocation}");
            _result.Text.ShouldContain($"* {Commands.Reset}");
            _result.Text.ShouldContain($"* {Commands.Finish}");
            _result.Text.ShouldContain($"* {Commands.StringConfig}");
            _result.Text.ShouldContain($"* {Commands.RoleConfig}");
            _result.Text.ShouldContain($"* {Commands.LocationWeight}");
            _result.Text.ShouldContain($"* {Commands.RemoveUser}");
            _result.Text.ShouldContain($"* {Commands.Results}");
            _result.Text.ShouldContain($"* {Commands.NumberConfig}");
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
