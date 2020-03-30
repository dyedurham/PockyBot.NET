using System.Collections.Generic;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core.Messages;
using NSubstitute;
using PockyBot.NET.Constants;
using PockyBot.NET.Persistence.Models;
using PockyBot.NET.Persistence.Repositories;
using PockyBot.NET.Services.Triggers;
using PockyBot.NET.Tests.TestData.Triggers;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace PockyBot.NET.Tests.Services.Triggers
{
    public class NumberConfigTests
    {
        private readonly NumberConfig _subject;
        private readonly IConfigRepository _configRepository;

        private Message _message;
        private Message _result;

        public NumberConfigTests()
        {
            _configRepository = Substitute.For<IConfigRepository>();
            _subject = new NumberConfig(_configRepository);
        }

        [Theory]
        [MemberData(nameof(NumberConfigTestData.GetNumberConfigTestData), MemberType = typeof(NumberConfigTestData))]
        internal void TestGetNumberConfig(IList<GeneralConfig> allGeneralConfig, string[] response)
        {
            this.Given(x => GivenANumberConfigMessage("get"))
                .And(x => GivenAllGeneralConfig(allGeneralConfig))
                .When(x => WhenRespondingToAMessage())
                .Then(x => ThenItShouldReturnAResponse(response))
                .BDDfy();
        }

        [Theory]
        [InlineData("set config 2", 5, "config", 2)]
        [InlineData("set minimum 5", 5, "minimum", 5)]
        [InlineData("set minimum 3", 5, "minimum", 3)]
        internal void TestSetNumberConfig(string command, int limit, string name, int value)
        {
            this.Given(x => GivenANumberConfigMessage(command))
                .And(x => GivenPegLimitIs(limit))
                .When(x => WhenRespondingToAMessage())
                .Then(x => ThenItShouldReturnAResponse("Value has been set."))
                .And(x => ThenItShouldSetANumberConfig(name, value))
                .BDDfy();
        }

        [Theory]
        [InlineData("delete config", "config")]
        [InlineData("delete raccoon", "raccoon")]
        [InlineData("delete minimum", "minimum")]
        internal void TestDeleteNumberConfig(string command, string name)
        {
            this.Given(x => GivenANumberConfigMessage(command))
                .And(x => GivenANumberConfigExists(name))
                .When(x => WhenRespondingToAMessage())
                .Then(x => ThenItShouldReturnAResponse("Config has been deleted."))
                .And(x => ThenItShouldDeleteANumberConfig(name))
                .BDDfy();
        }

        [Theory]
        [InlineData("", "Please specify a command. Possible values are get, set, and delete.")]
        [InlineData("geet", "Unknown command. Possible values are get, set, and delete.")]
        internal void TestGetNumberConfigInvalid(string command, string response)
        {
            this.Given(x => GivenANumberConfigMessage(command))
                .When(x => WhenRespondingToAMessage())
                .Then(x => ThenItShouldReturnAResponse(response))
                .BDDfy();
        }

        [Theory]
        [InlineData("set ", 5, "You must specify a config name and value to set.")]
        [InlineData("set config3", 5, "You must specify a config name and value to set.")]
        [InlineData("set config 3 more", 5, "You must specify a config name and value to set.")]
        [InlineData("set config blah", 5, "Value must be set to a number.")]
        [InlineData("set config -4", 5, "Value must be greater than or equal to zero.")]
        [InlineData("set minimum 6", 5, "Minimum pegs must be less than or equal to peg limit.")]
        internal void TestSetNumberConfigInvalid(string command, int limit, string response)
        {
            this.Given(x => GivenANumberConfigMessage(command))
                .And(x => GivenPegLimitIs(limit))
                .When(x => WhenRespondingToAMessage())
                .Then(x => ThenItShouldReturnAResponse(response))
                .And(x => ThenItShouldNotSetANumberConfig())
                .BDDfy();
        }

        [Theory]
        [InlineData("delete", "You must specify a config name to delete.")]
        [InlineData("delete config more", "You must specify a config name to delete.")]
        [InlineData("delete nonexistent", "Config value nonexistent does not exist.")]
        internal void TestDeleteNumberConfigInvalid(string command, string response)
        {
            this.Given(x => GivenANumberConfigMessage(command))
                .When(x => WhenRespondingToAMessage())
                .Then(x => ThenItShouldReturnAResponse(response))
                .And(x => ThenItShouldNotDeleteANumberConfig())
                .BDDfy();
        }

        private void GivenANumberConfigMessage(string command)
        {
            _message = new Message
            {
                MessageParts = new[]
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
                        Text = $" {Commands.NumberConfig} {command}"
                    }
                }
            };
        }

        private void GivenPegLimitIs(int limit)
        {
            _configRepository.GetGeneralConfig("limit").Returns(limit);
        }

        private void GivenANumberConfigExists(string name)
        {
            _configRepository.GetGeneralConfig(name).Returns(0);
        }

        private void GivenAllGeneralConfig(IList<GeneralConfig> allGeneralConfig)
        {
            _configRepository.GetAllGeneralConfig().Returns(allGeneralConfig);
        }

        private async Task WhenRespondingToAMessage()
        {
            _result = await _subject.Respond(_message);
        }

        private void ThenItShouldReturnAResponse(params string[] response)
        {
            _result.ShouldNotBeNull();
            foreach (var item in response)
            {
                _result.Text.ShouldContain(item);
            }
        }

        private void ThenItShouldSetANumberConfig(string name, int value)
        {
            _configRepository.Received(1).SetGeneralConfig(name, value);
        }

        private void ThenItShouldDeleteANumberConfig(string name)
        {
            _configRepository.Received(1).DeleteGeneralConfig(name);
        }

        private void ThenItShouldNotSetANumberConfig()
        {
            _configRepository.DidNotReceive().SetGeneralConfig(Arg.Any<string>(), Arg.Any<int>());
        }

        private void ThenItShouldNotDeleteANumberConfig()
        {
            _configRepository.DidNotReceive().DeleteGeneralConfig(Arg.Any<string>());
        }
    }
}
