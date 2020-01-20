using System;
using System.Collections.Generic;
using System.Linq;
using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Persistence.Repositories
{
    internal class ConfigRepository : IConfigRepository
    {
        private readonly DatabaseContext _context;

        public ConfigRepository(DatabaseContext context)
        {
            _context = context;
        }

        public IList<GeneralConfig> GetAllGeneralConfig()
        {
            return _context.GeneralConfig.ToList();
        }

        public int? GetGeneralConfig(string name)
        {
            var config = _context.GeneralConfig.FirstOrDefault(x =>
                string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase));
            return config?.Value;
        }

        public IList<StringConfig> GetAllStringConfig()
        {
            return _context.StringConfig.ToList();
        }

        public IList<string> GetStringConfig(string name)
        {
            return _context.StringConfig.Where(x =>
                string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase))
                .Select(x => x.Value).ToList();
        }
    }
}
