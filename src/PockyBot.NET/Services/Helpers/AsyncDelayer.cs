using System;
using System.Threading;
using System.Threading.Tasks;

namespace PockyBot.NET.Services.Helpers
{
    internal class AsyncDelayer: IAsyncDelayer
    {
        public Task Delay(TimeSpan timeSpan, CancellationToken token)
        {
            return Task.Delay(timeSpan, token);
        }
    }
}
