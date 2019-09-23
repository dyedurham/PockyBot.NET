using GlobalX.ChatBots.Core.Messages;
using Microsoft.Extensions.Options;
using NSubstitute;
using PockyBot.NET.Configuration;
using PockyBot.NET.Persistence.Models;
using PockyBot.NET.Persistence.Repositories;
using PockyBot.NET.Services;
using PockyBot.NET.Services.Triggers;
using PockyBot.NET.Tests.TestData;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace PockyBot.NET.Tests.Services
{
    public class TriggerResponseTesterTests
    {
        private readonly TriggerResponseTester _subject;

        private readonly PockyBotSettings _settings;
        private readonly IPockyUserRepository _pockyUserRepository;

        private Message _message;
        private ITrigger _trigger;
        private bool _result;

        public TriggerResponseTesterTests()
        {
            _settings = new PockyBotSettings();
            _pockyUserRepository = Substitute.For<IPockyUserRepository>();
            var options = Substitute.For<IOptions<PockyBotSettings>>();
            options.Value.Returns(_settings);
            _subject = new TriggerResponseTester(options, _pockyUserRepository);
        }

        [Theory]
        [MemberData(nameof(TriggerResponseTesterTestData.RoomTestData), MemberType =
            typeof(TriggerResponseTesterTestData))]
        internal void TestShouldTriggerInRoom(PockyBotSettings settings, PockyUser user, Message message,
            ITrigger trigger, bool shouldTrigger)
        {
            this.Given(x => GivenPockyBotSettings(settings))
                .And(x => GivenAPockyUser(user))
                .And(x => GivenAMessage(message))
                .And(x => GivenATrigger(trigger))
                .When(x => WhenCallingShouldTriggerInRoom())
                .Then(x => ThenItShouldReturn(shouldTrigger))
                .BDDfy();
        }

        [Theory]
        [MemberData(nameof(TriggerResponseTesterTestData.DirectMessageTestData), MemberType =
            typeof(TriggerResponseTesterTestData))]
        internal void TestShouldTriggerInDirectMessage(PockyUser user, Message message, ITrigger trigger,
            bool shouldTrigger)
        {
            this.Given(x => GivenAPockyUser(user))
                .And(x => GivenAMessage(message))
                .And(x => GivenATrigger(trigger))
                .When(x => WhenCallingShouldTriggerInDirectMessage())
                .Then(x => ThenItShouldReturn(shouldTrigger))
                .BDDfy();
        }

        private void GivenPockyBotSettings(PockyBotSettings settings)
        {
            _settings.BotId = settings.BotId;
            _settings.BotName = settings.BotName;
            _settings.DatabaseConnectionString = settings.DatabaseConnectionString;
        }

        private void GivenAPockyUser(PockyUser user)
        {
            _pockyUserRepository.GetUser(user.UserId).Returns(user);
        }

        private void GivenAMessage(Message message)
        {
            _message = message;
        }

        private void GivenATrigger(ITrigger trigger)
        {
            _trigger = trigger;
        }

        private void WhenCallingShouldTriggerInRoom()
        {
            _result = _subject.ShouldTriggerInRoom(_message, _trigger);
        }

        private void WhenCallingShouldTriggerInDirectMessage()
        {
            _result = _subject.ShouldTriggerInDirectMessage(_message, _trigger);
        }

        private void ThenItShouldReturn(bool shouldTrigger)
        {
            _result.ShouldBe(shouldTrigger);
        }
    }
}
