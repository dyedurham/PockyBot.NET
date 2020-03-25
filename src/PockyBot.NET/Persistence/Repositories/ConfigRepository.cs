using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task SetGeneralConfig(string name, int value)
        {
            var existingConfig =
                _context.GeneralConfig.FirstOrDefault(x =>
                    string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase));

            if (existingConfig != null)
            {
                _context.Attach(existingConfig);
                existingConfig.Value = value;
                _context.Entry(existingConfig).Property(x => x.Value).IsModified = true;
            }
            else
            {
                _context.GeneralConfig.Add(new GeneralConfig
                {
                    Name = name,
                    Value = value
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteGeneralConfig(GeneralConfig config)
        {
            _context.GeneralConfig.Remove(config);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteGeneralConfig(string name)
        {
            var existingConfig = _context.GeneralConfig.First(x =>
                string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase));
            _context.Remove(existingConfig);
            await _context.SaveChangesAsync();
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

        public async Task AddStringConfig(string name, string value)
        {
            _context.StringConfig.Add(new StringConfig
            {
                Name = name, Value = value
            });
            await _context.SaveChangesAsync();
        }

        public async Task DeleteStringConfig(string name, string value)
        {
            var configToDelete = _context.StringConfig.First(x => x.Name == name && x.Value == value);
            _context.StringConfig.Remove(configToDelete);
            await _context.SaveChangesAsync();
        }
    }
}
