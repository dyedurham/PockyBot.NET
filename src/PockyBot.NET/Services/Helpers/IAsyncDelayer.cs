using System;
using System.Threading;
using System.Threading.Tasks;

namespace PockyBot.NET.Services.Helpers
{
    interface IAsyncDelayer
    {
        Task Delay(TimeSpan timeSpan, CancellationToken token);
    }
}
