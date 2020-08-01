using System;

namespace PockyBot.NET.Services.Helpers
{
    public class RandomnessHandler : IRandomnessHandler
    {
        public Random Random { get; } = new Random();
    }
}
