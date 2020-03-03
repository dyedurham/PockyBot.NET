using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core.Messages;
using NSubstitute;
using PockyBot.NET.Constants;
using PockyBot.NET.Persistence.Repositories;
using PockyBot.NET.Services.Triggers;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace PockyBot.NET.Tests.Services.Triggers
{
    public class StringConfigTests
    {
        private readonly StringConfig _subject;
        private readonly IConfigRepository _configRepository;

        private Message _message;
        private Message _result;

        public StringConfigTests()
        {
            _configRepository = Substitute.For<IConfigRepository>();
            _subject = new StringConfig(_configRepository);
        }

        [Fact]
        public void ItShouldDisplayAllStringConfigValues()
        {
            this.Given(x => GivenAStringConfigMessage("get"))
                .And(x => GivenTheConfigRepositoryReturnsTheStringConfig())
                .When(x => WhenRespondIsCalled())
                .Then(x => ThenItShouldReturnTheCurrentStringConfig())
                .BDDfy();
        }

        [Fact]
        public void ItShouldAddANewStringConfigValue()
        {
            this.Given(x => GivenAStringConfigMessage("add keyword added"))
                .And(x => GivenTheConfigRepositoryReturnsNoStringConfigItems("keyword"))
                .When(x => WhenRespondIsCalled())
                .Then(x => ThenItShouldAddTheStringConfigItem("keyword", "added"))
                .BDDfy();
        }

        [Fact]
        public void ItShouldDeleteAStringConfigValue()
        {
            this.Given(x => GivenAStringConfigMessage("delete keyword test"))
                .And(x => GivenTheConfigRepositoryReturnsTheStringConfigItem("keyword", "test"))
                .When(x => WhenRespondIsCalled())
                .Then(x => ThenItShouldDeleteTheStringConfigItem("keyword", "test"))
                .BDDfy();
        }

        [Fact]
        public void ItShouldDisplayAMessageIfAConfigActionIsNotProvided()
        {
            this.Given(x => GivenAStringConfigMessage(""))
                .When(x => WhenRespondIsCalled())
                .Then(x => ItShouldDisplayAMessage($"Please specify a command. Possible values are {string.Join(", ", ConfigActions.All())}."))
                .BDDfy();
        }

        [Fact]
        public void ItShouldDisplayAMessageIfAnInvalidConfigActionIsProvided()
        {
            this.Given(x => GivenAStringConfigMessage("invalid"))
                .When(x => WhenRespondIsCalled())
                .Then(x => ItShouldDisplayAMessage($"Invalid string config command. Possible values are {string.Join(", ", ConfigActions.All())}."))
                .BDDfy();
        }

        [Fact]
        public void ItShouldDisplayAMessageIfNoKeyValuePairIsProvidedToAdd()
        {
            this.Given(x => GivenAStringConfigMessage("add"))
                .When(x => WhenRespondIsCalled())
                .Then(x => ItShouldDisplayAMessage("You must specify a config name and value to add."))
                .BDDfy();
        }

        [Fact]
        public void ItShouldDisplayAMessageWhenTryingToAddAKeyValuePairThatAlreadyExists()
        {
            this.Given(x => GivenAStringConfigMessage("add test exists"))
                .And(x => GivenTheConfigRepositoryReturnsTheStringConfigItem("test", "exists"))
                .When(x => WhenRespondIsCalled())
                .Then(x => ItShouldDisplayAMessage("String config name:value pair test:exists already exists."))
                .BDDfy();
        }

        [Fact]
        public void ItShouldDisplayAMessageIfNoKeyValuePairIsProvidedToDelete()
        {
            this.Given(x => GivenAStringConfigMessage("delete"))
                .When(x => WhenRespondIsCalled())
                .Then(x => ItShouldDisplayAMessage("You must specify a config name and value to be deleted."))
                .BDDfy();
        }

        [Fact]
        public void ItShouldDisplayAMessageWhenTryingToDeleteAKeyValuePairThatDoesNotExist()
        {
            this.Given(x => GivenAStringConfigMessage("delete doesnt exist"))
                .And(x => GivenTheConfigRepositoryReturnsNoStringConfigItems("doesnt"))
                .When(x => WhenRespondIsCalled())
                .Then(x => ItShouldDisplayAMessage("String config name:value pair doesnt:exist does not exist."))
                .BDDfy();
        }

        private void GivenAStringConfigMessage(string command)
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
                        Text = "stringconfig " + command
                    }
                }
            };
        }

        private void GivenTheConfigRepositoryReturnsTheStringConfig()
        {
            _configRepository.GetAllStringConfig().Returns(new List<Persistence.Models.StringConfig>
            {
                new Persistence.Models.StringConfig
                {
                    Name = "keyword",
                    Value = "test"
                },
                new Persistence.Models.StringConfig
                {
                    Name = "keyword",
                    Value = "pocky"
                },
                new Persistence.Models.StringConfig
                {
                    Name = "stuff",
                    Value = "tester"
                },
                new Persistence.Models.StringConfig
                {
                    Name = "rotation",
                    Value = "test1,test2"
                }
            });
        }

        private void GivenTheConfigRepositoryReturnsNoStringConfigItems(string name)
        {
            _configRepository.GetStringConfig(name).Returns(new Collection<string>());
        }

        private void GivenTheConfigRepositoryReturnsTheStringConfigItem(string name, string value)
        {
            _configRepository.GetStringConfig(name).Returns(new Collection<string>
            {
                value
            });
        }

        private async Task WhenRespondIsCalled()
        {
            _result = await _subject.Respond(_message);
        }

        private void ThenItShouldReturnTheCurrentStringConfig()
        {
            _result.Text.ShouldBe("Here is the current config (**name:** value):\n" +
                                  "* **keyword:**\n" +
                                  "    * test\n" +
                                  "    * pocky\n" +
                                  "* **stuff:** tester\n" +
                                  "* **rotation:** test1,test2\n");
        }

        private void ThenItShouldAddTheStringConfigItem(string name, string value)
        {
            _result.Text.ShouldBe($"Config has been updated: {name}:{value} has been added.");
            _configRepository.Received(1).AddStringConfig(name, value);
        }

        private void ThenItShouldDeleteTheStringConfigItem(string name, string value)
        {
            _result.Text.ShouldBe($"Config has been updated: {name}:{value} has been deleted.");
            _configRepository.Received(1).DeleteStringConfig(name, value);
        }

        private void ItShouldDisplayAMessage(string message)
        {
            _result.Text.ShouldBe(message);
        }
    }
}
