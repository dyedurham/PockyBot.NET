using System;
using System.Collections.Generic;
using System.Linq;
using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Persistence.Repositories
{
    public class ConfigRepository : IConfigRepository
    {
        private readonly DatabaseContext _context;

        public ConfigRepository(DatabaseContext context)
        {
            _context = context;
        }

        public List<GeneralConfig> GetAllGeneralConfig()
        {
            return _context.GeneralConfig.ToList();
        }

        public int? GetGeneralConfig(string name)
        {
            var config = _context.GeneralConfig.FirstOrDefault(x =>
                string.Equals(x.Name, name, StringComparison.InvariantCultureIgnoreCase));
            return config?.Value ?? null;
        }

        public List<StringConfig> GetAllStringConfig()
        {
            return _context.StringConfig.ToList();
        }

        public List<string> GetStringConfig(string name)
        {
            return _context.StringConfig.Where(x =>
                string.Equals(x.Name, name, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Value).ToList();
        }
    }
}