# PockyBot.NET

<p align="center">
    <a href="https://www.nuget.org/packages/PockyBot.NET">
        <img src="https://flat.badgen.net/nuget/v/pockybot.net" alt="PockyBot.NET nuget package" />
    </a>
    <a href="https://travis-ci.org/GlobalX/PockyBot.NET">
        <img src="https://flat.badgen.net/travis/GlobalX/PockyBot.NET" alt="PockyBot.NET on Travis CI" />
    </a>
    <a href="https://codecov.io/gh/GlobalX/PockyBot.NET">
        <img src="https://flat.badgen.net/codecov/c/github/globalx/pockybot.net" alt="PockyBot.NET on Codecov" />
    </a>
    <img src="https://flat.badgen.net/github/commits/globalx/pockybot.net" alt="commits" />
    <img src="https://flat.badgen.net/github/contributors/globalx/pockybot.net" alt="contributors" />
    <img src="https://flat.badgen.net/badge/commitizen/friendly/green" alt="commitizen friendly" />
</p>

A C# .NET Standard implementation of [PockyBot](https://github.com/GlobalX/pockybot).

## Getting started

### Bot Library

In order to use this bot, an implementation of GlobalX.ChatBots.Core is
required. Install the library alongside this library and follow its
configuration instructions.

### Configuration

In order to use this bot, some configuration is required. This can either be
done through appsettings.json, or at the time of configuring the bot.

#### Example Configuration

```json
// In appsettings.json
{
    "PockyBot.NET": {
        "BotId": "id",
        "BotName": "BotName",
        "DatabaseConnectionString": "Host=postgres.host.com;Port=5432;Username=user;Password=pass;Database=pockybot;"
    }
}
```

### Using Dependency Injection

In the `ConfigureServices` method of your `Startup.cs` file, add the following:

```cs
using PockyBot.NET;

public IServiceProvider ConfigureServices(IServiceCollection services)
{
    // Add other service registrations here
    services.ConfigurePockyBot(Configuration);
    return services;
}
```

If you have not provided your configuration inside appsettings.json, you may do
so when you configure the bot:

```cs
using PockyBot.NET;
using PockyBot.NET.Configuration;

public IServiceProvider ConfigureServices(IServiceCollection services)
{
    // Add other service registrations here
    var settings = new PockyBotSettings
    {
        BotId = "id",
        BotName = "BotName",
        DatabaseConnectionString = "Host=postgres.host.com;Port=5432;Username=user;Password=pass;Database=pockybot;"
    };

    services.ConfigurePockyBot(settings);
}
```

### Without Dependency Injection

You can get a concrete implementation of the library by calling the
`PockyBotFactory.CreatePockyBot` method.

```cs
using GlobalX.ChatBots.Core;
using PockyBot.NET;
using PockyBot.NET.Configuration;

// Some code here

var chatHelper; // An implementation of GlobalX.ChatBots.Core.IChatHelper

var settings = new PockyBotSettings
{
    BotId = "id",
    BotName = "BotName",
    DatabaseConnectionString = "Host=postgres.host.com;Port=5432;Username=user;Password=pass;Database=pockybot;"
};

IPockyBot pockybot = PockyBotFactory.CreatePockyBot(settings, chatHelper);
```
