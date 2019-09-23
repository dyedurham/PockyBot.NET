using System.Collections.Generic;
using PockyBot.NET.Persistence.Models;

namespace PockyBot.NET.Persistence.Repositories
{
    public interface IConfigRepository
    {
        List<GeneralConfig> GetAllGeneralConfig();
        int? GetGeneralConfig(string name);
        List<StringConfig> GetAllStringConfig();
        List<string> GetStringConfig(string name);
    }
}
