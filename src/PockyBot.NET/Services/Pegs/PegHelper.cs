using System;
using System.Globalization;
using System.Linq;
using PockyBot.NET.Persistence.Repositories;

namespace PockyBot.NET.Services.Pegs
{
    internal class PegHelper : IPegHelper
    {
        private const int DefaultRemoteLocationWeighting = 2;

        private readonly IConfigRepository _configRepository;

        public PegHelper(IConfigRepository configRepository)
        {
            _configRepository = configRepository;
        }

        public bool IsPegValid(string comment, int? requireKeywords, string[] keywords, string[] penaltyKeywords)
        {
            if (requireKeywords == 1)
            {
                return keywords.Any(x => comment.IndexOf(x, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            return !penaltyKeywords.Any(x => comment.IndexOf(x, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        public int GetPegWeighting(string senderLocation, string receiverLocation)
        {
            if (senderLocation == null || receiverLocation == null || senderLocation == receiverLocation)
            {
                return 1;
            }

            var senderToReceiver = $"locationWeight{senderLocation}to{receiverLocation}".ToLower(CultureInfo.InvariantCulture);
            var receiverToSender = $"locationWeight{receiverLocation}to{senderLocation}".ToLower(CultureInfo.InvariantCulture);
            var config = _configRepository.GetAllGeneralConfig()
                .FirstOrDefault(x => x.Name.ToLower() == senderToReceiver || x.Name.ToLower() == receiverToSender);

            if (config != null)
            {
                return config.Value;
            }

            return _configRepository.GetGeneralConfig("remoteLocationWeightingDefault") ?? DefaultRemoteLocationWeighting;
        }

        public bool IsPenaltyPeg(string comment, string[] penaltyKeywords, string[] validKeywords)
        {
            return penaltyKeywords.Any(x => comment.IndexOf(x, StringComparison.OrdinalIgnoreCase) >= 0) &&
                   !validKeywords.Any(x => comment.IndexOf(x, StringComparison.OrdinalIgnoreCase) >= 0);
        }
    }
}
