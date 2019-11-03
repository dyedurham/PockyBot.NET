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

#### Using Dependency Injection

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

#### Without Dependency Injection

You can get a concrete implementation of the library by calling the
`PockyBotFactory.CreatePockyBot` method.

```cs
using GlobalX.ChatBots.Core;
using PockyBot.NET;
using PockyBot.NET.Configuration;

// Some code here

IChatHelper chatHelper; // An implementation of GlobalX.ChatBots.Core.IChatHelper

var settings = new PockyBotSettings
{
    BotId = "id",
    BotName = "BotName",
    DatabaseConnectionString = "Host=postgres.host.com;Port=5432;Username=user;Password=pass;Database=pockybot;"
};

IPockyBot pockybot = PockyBotFactory.CreatePockyBot(settings, chatHelper);
```

### Using The Bot

Once you have an instance of IPockyBot, you can use it to respond to a message like so:

```cs
using GlobalX.ChatBots.Core;
using PockyBot.NET;

Message message; // input from somewhere
IPockyBot pockybot; // initialised elsewhere

pockybot.Respond(message);
```

## Database

PockyBot requires a postgres database. THe `database` folder in this
repository contains a number of `.sql` files you can use to set this up.

Table `generalconfig` is initalised with default values as follows:

| Value             | Default | Explanation                                                                                                                         |
| :---------------- | :------ | :---------------------------------------------------------------------------------------------------------------------------------- |
| limit             | 10      | The number of pegs each user is allowed to give out each cycle                                                                      |
| minimum           | 5       | The minimum number of pegs each user is _required_ to give out to be eligible to win                                                |
| winners           | 3       | The number of winners displayed (using a dense ranking)                                                                             |
| commentsRequired  | 1       | Boolean value of whether a reason is required to give a peg                                                                         |
| pegWithoutKeyword | 0       | Boolean value of whether the "peg" keyword is required to give a peg (if true, pegs can be given with `@PockyBot @Person <reason>`) |
| requireValues     | 1       | Boolean value of whether a keyword is required in the reason for a peg to be given                                                  |

Table `stringconfig` is used to define keywords.
Name field is 'keyword' and 'value' is the value of the keyword desired.
Default keywords are 'customer', 'brave', 'awesome', 'collaborative', and 'real', shorthands for the GlobalX company values.

Existing roles are 'ADMIN', 'UNMETERED', 'RESULTS', 'FINISH', 'RESET', 'UPDATE', and 'WINNERS'.
Users can have more than one role. Any users with the 'ADMIN' role are considered to have all other roles except for 'UNMETERED'.
'UNMETERED' users are not restricted by the usual 'limit' value from `generalconfig`.
All other roles relate to the commands of the same name displayed below.

## Commands

All commands related to PockyBot must begin with a mention of the bot, or be
sent directly to the bot. In this readme, mentions will be identified by
`@PockyBot`.

### General Commands

Use any of these commands in a room PockyBot is participating in to perform
commands.

-   `@PockyBot ping` — verify that the bot is alive.
-   `@PockyBot peg @Person <reason>` — give someone a peg.
-   `@PockyBot status` — get the list of pegs you have given.

#### Direct Message Commands

PockyBot can be messaged directly with certain commands.

-   `ping`
-   `status`

## Contributing

For notes on how to contribute, please see our [Contribution Guildelines](https://github.com/GlobalX/PockyBot.NET/blob/master/CONTRIBUTING.md).
