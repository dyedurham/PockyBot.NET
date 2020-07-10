using System;

namespace PockyBot.NET.Services
{
    public class RandomnessHandler : IRandomnessHandler
    {
        public Random Random { get; } = new Random();
    }
}
