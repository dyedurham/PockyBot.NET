using System;
using System.Collections.Generic;
using GlobalX.ChatBots.Core.Messages;
using Microsoft.Extensions.Options;
using NSubstitute;
using PockyBot.NET.Configuration;
using PockyBot.NET.Models.Exceptions;
using PockyBot.NET.Persistence.Repositories;
using PockyBot.NET.Services.Pegs;
using PockyBot.NET.Tests.TestData.Pegs;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace PockyBot.NET.Tests.Services.Pegs
{
    public class PegRequestValidatorTests
    {
        private readonly PegRequestValidator _subject;

        private readonly PockyBotSettings _settings;
        private readonly IConfigRepository _configRepository;

        private Message _message;
        private Exception _exception;

        public PegRequestValidatorTests()
        {
            _settings = new PockyBotSettings();
            _configRepository = Substitute.For<IConfigRepository>();
            var options = Substitute.For<IOptions<PockyBotSettings>>();
            options.Value.Returns(_settings);
            _subject = new PegRequestValidator(options, _configRepository);
        }

        [Theory]
        [MemberData(nameof(PegRequestValidatorTestData.ValidTestData), MemberType = typeof(PegRequestValidatorTestData))]
        public void TestValidateValidPegRequest(PockyBotSettings settings, int commentsRequired, List<string> keywords, List<string> penaltyKeywords, List<string> linkedKeywords, int requireValues, Message message)
        {
            this.Given(x => GivenPockyBotSettings(settings))
                .And(x => GivenAConfig(commentsRequired, keywords, penaltyKeywords, linkedKeywords, requireValues))
                .And(x => GivenAMessage(message))
                .When(x => WhenValidatingAPegRequest())
                .Then(x => ThenTheMessageShouldBeValid())
                .BDDfy();
        }

        [Theory]
        [MemberData(nameof(PegRequestValidatorTestData.InvalidTestData), MemberType = typeof(PegRequestValidatorTestData))]
        public void TestValidateInvalidPegRequest(PockyBotSettings settings, int commentsRequired, List<string> keywords, List<string> penaltyKeywords,  List<string> linkedKeywords, int requireValues, Message message, string errorMessage)
        {
            this.Given(x => GivenPockyBotSettings(settings))
                .And(x => GivenAConfig(commentsRequired, keywords, penaltyKeywords, linkedKeywords, requireValues))
                .And(x => GivenAMessage(message))
                .When(x => WhenValidatingAPegRequest())
                .Then(x =>ThenTheMessageShouldBeInvalid())
                .And(x => ThenTheErrorMessageShouldBe(errorMessage))
                .BDDfy();
        }

        [Theory]
        [MemberData(nameof(PegRequestValidatorTestData.ValidFormatTestData), MemberType = typeof(PegRequestValidatorTestData))]
        public void TestValidateValidPegRequestFormat(Message message)
        {
            this.Given(x => GivenAMessage(message))
                .When(x => WhenValidatingAPegRequestsFormat())
                .Then(x => ThenTheMessageShouldBeValid())
                .BDDfy();
        }

        [Theory]
        [MemberData(nameof(PegRequestValidatorTestData.InvalidFormatTestData), MemberType = typeof(PegRequestValidatorTestData))]
        public void TestValidateInvalidPegRequestFormat(PockyBotSettings settings, Message message, string errorMessage)
        {
            this.Given(x => GivenPockyBotSettings(settings))
                .And(x => GivenAMessage(message))
                .When(x => WhenValidatingAPegRequestsFormat())
                .Then(x =>ThenTheMessageShouldBeInvalid())
                .And(x => ThenTheErrorMessageShouldBe(errorMessage))
                .BDDfy();
        }

        private void GivenPockyBotSettings(PockyBotSettings settings)
        {
            _settings.BotId = settings.BotId;
            _settings.BotName = settings.BotName;
            _settings.DatabaseConnectionString = settings.DatabaseConnectionString;
        }

        private void GivenAConfig(int commentsRequired, List<string> keywords, List<string> penaltyKeywords, List<string> linkedKeywords, int requireValues)
        {
            _configRepository.GetGeneralConfig("commentsRequired").Returns(commentsRequired);
            _configRepository.GetStringConfig("keyword").Returns(keywords);
            _configRepository.GetStringConfig("penaltyKeyword").Returns(penaltyKeywords);
            _configRepository.GetStringConfig("linkedKeyword").Returns(linkedKeywords);
            _configRepository.GetGeneralConfig("requireValues").Returns(requireValues);
        }

        private void GivenAMessage(Message message)
        {
            _message = message;
        }

        private void WhenValidatingAPegRequest()
        {
            _exception = Record.Exception(() => _subject.ValidatePegRequest(_message));
        }

        private void WhenValidatingAPegRequestsFormat()
        {
            _exception = Record.Exception(() => _subject.ValidatePegRequestFormat(_message));
        }

        private void ThenTheMessageShouldBeValid()
        {
            _exception.ShouldBeNull();
        }

        private void ThenTheMessageShouldBeInvalid()
        {
            _exception.ShouldNotBeNull();
            _exception.ShouldBeOfType<PegValidationException>();
        }

        private void ThenTheErrorMessageShouldBe(string errorMessage)
        {
            _exception.Message.ShouldBe(errorMessage);
        }
    }
}
