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
        private readonly IConfigRepository _configRepository;

        private const string BotName = "Pocky";
        private const string SenderId = "testSender";
        private readonly List<Role> noRoles = new List<Role>();
        private readonly List<Role> adminRole = new List<Role>{new Role{UserRole = Roles.Admin}};

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
            _configRepository = Substitute.For<IConfigRepository>();
            _subject = new Help(_pockyUserRepository, pockyUserSettings, _configRepository);
        }

        [Fact]
        public void ItShouldShowAListOfCommandsToANonAdminUser()
        {
            this.Given(x => GivenAHelpMessage(""))
                .And(x => GivenTheUserHasRoles(noRoles))
                .When(x => WhenRespondIsCalled())
                .Then(x => ThenItShouldShowAListOfNonAdminCommands())
                .BDDfy();
        }

        [Fact]
        public void ItShouldShowAListOfCommandsToANonAdminUserInADirectMessage()
        {
            this.Given(x => GivenADirectMessageHelpMessage(""))
                .And(x => GivenTheUserHasRoles(noRoles))
                .When(x => WhenRespondIsCalled())
                .Then(x => ThenItShouldShowAListOfNonAdminCommands())
                .BDDfy();
        }

        [Fact]
        public void ItShouldShowAListOfAdminCommandsToAnAdminUser()
        {
            this.Given(x => GivenAHelpMessage(""))
                .And(x => GivenTheUserHasRoles(adminRole))
                .When(x => WhenRespondIsCalled())
                .Then(x => ThenItShouldShowAListOfAdminCommands())
                .BDDfy();
        }

        [Fact]
        public void ItShouldShowTheDefaultHelpMessageWhenTheCommandDoesNotExist()
        {
            this.Given(x => GivenAHelpMessage("bogusCommand"))
                .And(x => GivenTheUserHasRoles(noRoles))
                .When(x => WhenRespondIsCalled())
                .Then(x => ThenItShouldShowTheDefaultHelpMessage())
                .BDDfy();
        }

        [Fact]
        public void ItShouldShowTheDefaultHelpMessageWhenTheCommandDoesNotExistInADirectMessage()
        {
            this.Given(x => GivenADirectMessageHelpMessage("bogusCommand"))
                .And(x => GivenTheUserHasRoles(noRoles))
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
                .And(x => GivenTheUserHasRoles(noRoles))
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
                .And(x => GivenTheUserHasRoles(noRoles))
                .When(x => WhenRespondIsCalled())
                .Then(x => ThenItShouldShowTheCommandHelpMessage(command))
                .BDDfy();
        }

        [Theory]
        [InlineData(Commands.Finish, Roles.Finish)]
        [InlineData(Commands.Finish, Roles.Admin)]
        [InlineData(Commands.Reset, Roles.Reset)]
        [InlineData(Commands.Reset, Roles.Admin)]
        [InlineData(Commands.StringConfig, Roles.Admin)]
        [InlineData(Commands.StringConfig, Roles.Config)]
        [InlineData(Commands.LocationWeight, Roles.Admin)]
        [InlineData(Commands.LocationWeight, Roles.Config)]
        [InlineData(Commands.RemoveUser, Roles.Admin)]
        [InlineData(Commands.RemoveUser, Roles.RemoveUser)]
        public void ItShouldShowTheHelpMessageForAdminCommandsToAdminUsers(string command, string userRole)
        {
            this.Given(x => GivenAHelpMessage(command))
                .And(x => GivenTheUserHasRoles(new List<Role>{new Role{UserRole = userRole}}))
                .When(x => WhenRespondIsCalled())
                .Then(x => ThenItShouldShowTheCommandHelpMessage(command))
                .BDDfy();
        }

        [Theory]
        [InlineData(Commands.Finish)]
        [InlineData(Commands.Reset)]
        [InlineData(Commands.StringConfig)]
        [InlineData(Commands.LocationWeight)]
        [InlineData(Commands.RemoveUser)]
        public void ItShouldShowTheDefaultHelpMessageForAdminCommandsToNonAdminUsers(string command)
        {
            this.Given(x => GivenAHelpMessage(command))
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

        private void GivenTheUserHasRoles(List<Role> roles)
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
            _result.Text.ShouldBe("## What I can do (List of Commands)\n\n" +
                                  $"* {Commands.Peg}\n" +
                                  $"* {Commands.Status}\n" +
                                  $"* {Commands.Keywords}\n" +
                                  $"* {Commands.Ping}\n" +
                                  $"* {Commands.Welcome}\n" +
                                  $"* {Commands.Rotation}\n" +
                                  $"* {Commands.LocationConfig}\n" +
                                  $"\nFor more information on a command type `@{BotName} help command-name` or direct message me with `help command-name`\n" +
                                  "\nI am still being worked on, so more features to come.");
        }

        private void ThenItShouldShowAListOfAdminCommands()
        {
            _result.Text.ShouldBe("## What I can do (List of Commands)\n\n" +
                                  $"* {Commands.Peg}\n" +
                                  $"* {Commands.Status}\n" +
                                  $"* {Commands.Keywords}\n" +
                                  $"* {Commands.Ping}\n" +
                                  $"* {Commands.Welcome}\n" +
                                  $"* {Commands.Rotation}\n" +
                                  $"* {Commands.LocationConfig}\n" +
                                  $"* {Commands.Reset}\n" +
                                  $"* {Commands.Finish}\n" +
                                  $"* {Commands.StringConfig}\n" +
                                  $"* {Commands.LocationWeight}\n" +
                                  $"* {Commands.RemoveUser}\n" +
                                  $"\nFor more information on a command type `@{BotName} help command-name` or direct message me with `help command-name`\n" +
                                  "\nI am still being worked on, so more features to come.");
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
