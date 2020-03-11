using System.Collections.Generic;
using System.Threading.Tasks;
using GlobalX.ChatBots.Core.Messages;
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
    public class LocationWeightTests
    {
        private readonly IConfigRepository _configRepository;
        private readonly ILocationRepository _locationRepository;

        private readonly LocationWeight _subject;

        private Message _message;
        private Message _response;

        public LocationWeightTests()
        {
            _configRepository = Substitute.For<IConfigRepository>();
            _locationRepository = Substitute.For<ILocationRepository>();
            _subject = new LocationWeight(_configRepository, _locationRepository);
        }

        [Theory]
        [MemberData(nameof(LocationWeightTestData.RespondTestData), MemberType = typeof(LocationWeightTestData))]
        internal void TestRespond(Message message, string[] locations, List<GeneralConfig> allGeneralConfig, Message response)
        {
            this.Given(x => GivenAMessage(message))
                .And(x => GivenAListOfLocations(locations))
                .And(x => GivenAListOfGeneralConfigs(allGeneralConfig))
                .When(x => WhenRespondingToAMessage())
                .Then(x => ThenItShouldReturnAResponse(response))
                .BDDfy();
        }

        [Theory]
        [MemberData(nameof(LocationWeightTestData.SetLocationWeightTestData), MemberType = typeof(LocationWeightTestData))]
        internal void TestSetLocationWeight(Message message, string[] locations, List<GeneralConfig> allGeneralConfig,
            Message response, string name, int value)
        {
            this.Given(x => GivenAMessage(message))
                .And(x => GivenAListOfLocations(locations))
                .And(x => GivenAListOfGeneralConfigs(allGeneralConfig))
                .When(x => WhenRespondingToAMessage())
                .Then(x => ThenItShouldReturnAResponse(response))
                .And(x => ThenItShouldCallSetGeneralConfig(name, value))
                .BDDfy();
        }

        [Theory]
        [MemberData(nameof(LocationWeightTestData.DeleteLocationWeightTestData), MemberType = typeof(LocationWeightTestData))]
        internal void TestDeleteLocationWeight(Message message, string[] locations,
            List<GeneralConfig> allGeneralConfig, Message response, GeneralConfig config)
        {
            this.Given(x => GivenAMessage(message))
                .And(x => GivenAListOfLocations(locations))
                .And(x => GivenAListOfGeneralConfigs(allGeneralConfig))
                .When(x => WhenRespondingToAMessage())
                .Then(x => ThenItShouldReturnAResponse(response))
                .And(x => ThenItShouldCallDeleteGeneralConfig(config))
                .BDDfy();
        }

        private void GivenAMessage(Message message)
        {
            _message = message;
        }

        private void GivenAListOfLocations(string[] locations)
        {
            _locationRepository.GetAllLocations().Returns(locations);
        }

        private void GivenAListOfGeneralConfigs(List<GeneralConfig> allGeneralConfig)
        {
            _configRepository.GetAllGeneralConfig().Returns(allGeneralConfig);
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

        private void ThenItShouldCallSetGeneralConfig(string name, int value)
        {
            _configRepository.Received(1).SetGeneralConfig(name, value);
        }

        private void ThenItShouldCallDeleteGeneralConfig(GeneralConfig config)
        {
            _configRepository.Received(1)
                .DeleteGeneralConfig(Arg.Is<GeneralConfig>(x => x.Name == config.Name && x.Value == config.Value));
        }
    }
}
