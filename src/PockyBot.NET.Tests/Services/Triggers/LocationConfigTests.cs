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
    public class LocationConfigTests
    {
        private readonly LocationConfig _subject;

        private readonly ILocationRepository _locationRepository;
        private readonly IPockyUserRepository _pockyUserRepository;

        private Message _message;
        private Message _response;

        public LocationConfigTests()
        {
            _locationRepository = Substitute.For<ILocationRepository>();
            _pockyUserRepository = Substitute.For<IPockyUserRepository>();
            _subject = new LocationConfig(_locationRepository, _pockyUserRepository);
        }

        [Theory]
        [MemberData(nameof(LocationConfigTestData.RespondTestData), MemberType = typeof(LocationConfigTestData))]
        internal void TestRespond(Message message, PockyUser user, string[] locations, Message response)
        {
            this.Given(x => GivenAMessage(message))
                .And(x => GivenAUser(user))
                .And(x => GivenLocations(locations))
                .When(x => WhenRespondingToAMessage())
                .Then(x => ThenItShouldReturnAResponse(response))
                .BDDfy();
        }

        [Theory]
        [MemberData(nameof(LocationConfigTestData.SetLocationTestData), MemberType = typeof(LocationConfigTestData))]
        internal void TestSetLocation(Message message, PockyUser user, string[] locations, Message response,
            string locationName)
        {
            this.Given(x => GivenAMessage(message))
                .And(x => GivenAUser(user))
                .And(x => GivenLocations(locations))
                .When(x => WhenRespondingToAMessage())
                .Then(x => ThenItShouldReturnAResponse(response))
                .And(x => ThenItShouldSetALocation(locationName))
                .BDDfy();
        }

        [Theory]
        [MemberData(nameof(LocationConfigTestData.DeleteLocationTestData), MemberType = typeof(LocationConfigTestData))]
        internal void TestDeleteLocation(Message message, PockyUser user, string[] locations, Message response,
            string locationName)
        {
            this.Given(x => GivenAMessage(message))
                .And(x => GivenAUser(user))
                .And(x => GivenLocations(locations))
                .When(x => WhenRespondingToAMessage())
                .Then(x => ThenItShouldReturnAResponse(response))
                .And(x => ThenItShouldDeleteALocation(locationName))
                .BDDfy();
        }

        private void GivenAMessage(Message message)
        {
            _message = message;
        }

        private void GivenAUser(PockyUser user)
        {
            _pockyUserRepository.GetUser(user.UserId).Returns(user);
        }

        private void GivenLocations(string[] locations)
        {
            _locationRepository.GetAllLocations().Returns(locations);
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

        private void ThenItShouldSetALocation(string locationName)
        {
            _locationRepository.Received(1).SetLocation(locationName);
        }

        private void ThenItShouldDeleteALocation(string locationName)
        {
            _locationRepository.Received(1).DeleteLocation(locationName);
        }
    }
}
