# PockyBot.NET

<p align="center">
    <a href="https://www.nuget.org/packages/PockyBot.NET">
        <img src="https://flat.badgen.net/nuget/v/pockybot.net" alt="PockyBot.NET nuget package" />
    </a>
    <a href="https://travis-ci.org/GlobalX/PockyBot.NET">
        <img src="https://flat.badgen.net/travis/GlobalX/PockyBot.NET" alt="PockyBot.NET on Travis CI" />
    </a>
    <a href="https://www.codacy.com/gh/GlobalX/PockyBot.NET/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=GlobalX/PockyBot.NET&amp;utm_campaign=Badge_Grade">
        <img src="https://app.codacy.com/project/badge/Grade/8fa5a933c8604d1db3bf402771f28716" alt="Codacy Grade Badge"/>
    </a>
    <a href="https://www.codacy.com/gh/GlobalX/PockyBot.NET/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=GlobalX/PockyBot.NET&amp;utm_campaign=Badge_Coverage">
        <img src="https://app.codacy.com/project/badge/Coverage/8fa5a933c8604d1db3bf402771f28716" alt="Codacy Coverage Badge" />  
    </a>
    <a href="https://codecov.io/gh/GlobalX/PockyBot.NET">
        <img src="https://flat.badgen.net/codecov/c/github/globalx/pockybot.net" alt="PockyBot.NET on Codecov" />
    </a>
    <a href="https://lgtm.com/projects/g/GlobalX/PockyBot.NET/alerts/">
        <img alt="Total alerts on LGTM" src="https://img.shields.io/lgtm/alerts/g/GlobalX/PockyBot.NET.svg?logo=lgtm&logoWidth=18"/>
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

You will also need to provide an implementation of `IResultsUploader` to upload
the generated results to a location such as Google Cloud and return the link
where those results are accessible.

```cs
using PockyBot.NET;

public IServiceProvider ConfigureServices(IServiceCollection services)
{
    // other service registrations
    services.AddTransient<IResultsUploader, MyResultsUploader>();
}
```

You will also need to inject logging classes.
[See this documentation for details](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-2.1).

#### Without Dependency Injection

You can get a concrete implementation of the library by calling the
`PockyBotFactory.CreatePockyBot` method.

```cs
using GlobalX.ChatBots.Core;
using PockyBot.NET;
using PockyBot.NET.Configuration;

// Some code here

IChatHelper chatHelper; // An implementation of GlobalX.ChatBots.Core.IChatHelper
IResultsUploader resultsUploader; // An implementation of PockyBot.NET.IResultsUploader

var settings = new PockyBotSettings
{
    BotId = "id",
    BotName = "BotName",
    DatabaseConnectionString = "Host=postgres.host.com;Port=5432;Username=user;Password=pass;Database=pockybot;"
};

ILoggerFactory factory = new LoggerFactory().AddConsole(); // add any providers you want here
IPockyBot pockybot = PockyBotFactory.CreatePockyBot(settings, chatHelper, resultsUploader, factory);
```

### Using The Bot

Once you have an instance of IPockyBot, you can use it to respond to a message
like so:

```cs
using GlobalX.ChatBots.Core;
using PockyBot.NET;

Message message; // input from somewhere
IPockyBot pockybot; // initialised elsewhere

pockybot.Respond(message);
```

## Database

PockyBot requires a postgres database. The `database` folder in this
repository contains a number of `.sql` files you can use to set this up.

Table `generalconfig` is initalised with default values as follows:

| Value             | Default | Explanation                                                                                                                         |
| :---------------- | :------ | :---------------------------------------------------------------------------------------------------------------------------------- |
| limit             | 10      | The number of pegs each user is allowed to give out each cycle                                                                      |
| minimum           | 5       | The minimum number of pegs each user is *required* to give out to be eligible to win                                                |
| winners           | 3       | The number of winners displayed (using a dense ranking)                                                                             |
| commentsRequired  | 1       | Boolean value of whether a reason is required to give a peg                                                                         |
| pegWithoutKeyword | 0       | Boolean value of whether the "peg" keyword is required to give a peg (if true, pegs can be given with `@PockyBot @Person <reason>`) |
| requireValues     | 1       | Boolean value of whether a keyword is required in the reason for a peg to be given                                                  |

Table `stringconfig` is used to define keywords.
Name field is 'keyword' and 'value' is the value of the keyword desired.
Default keywords are 'customer', 'collaboration', 'amazing', 'integrity', and
'improvement', shorthands for the Dye & Durham company values.

Table `stringconfig` is also used to define linked keywords.
Name field is 'linkedKeyword' and 'value' is the value of the keyword and a
linked word, separated by a colon (e.g. 'amazing:awesome').
These are used to define other words that will act as one of the main
keywords for validation and results purposes. 

Existing roles are 'ADMIN', 'UNMETERED', 'RESULTS', 'FINISH', 'RESET',
'UPDATE', and 'WINNERS'. Users can have more than one role. Any users with the
'ADMIN' role are considered to have all other roles except for 'UNMETERED'.
'UNMETERED' users are not restricted by the usual 'limit' value from
`generalconfig`. All other roles relate to the commands of the same name
displayed below.

## Commands

All commands related to PockyBot must begin with a mention of the bot, or be
sent directly to the bot. In this readme, mentions will be identified by
`@PockyBot`.

### General Commands

Use any of these commands in a room PockyBot is participating in to perform
commands.

- `@PockyBot ping` — verify that the bot is alive.
- `@PockyBot peg @Person <reason>` — give someone a peg.
- `@PockyBot status` — get the list of pegs you have given.
- `@PockyBot help` — get help with using the bot.
- `@PockyBot rotation` — get the snack buying rotation.
- `@PockyBot welcome` — welcome users.
- `@PockyBot locationconfig get|add|set|delete <location>` — manage locations.
- `@PockyBot userlocation`:
    - `@PockyBot userlocation get me|all|unset|@User` — get users' locations.
    - `@PockyBot userlocation set <location> me` — set your location.
    - `@PockyBot userlocation delete me` — delete your location.

#### Admin-only commands

These commands require special permissions to use.

- `@PockyBot finish` — get the results of the cycle.
- `@PockyBot reset` — clear all pegs from the last cycle.
- `@PockyBot userlocation`:
    - `@PockyBot userlocation set <location> @User1 @User2` — set one or more users' locations.
    - `@PockyBot userlocation delete @User1 @User2` — delete one or more users' locations.

#### Direct Message Commands

PockyBot can be messaged directly with certain commands.

- `ping`
- `status`
- `help`
- `rotation`
- `welcome`

## Contributing

For notes on how to contribute, please see our [Contribution Guildelines](https://github.com/GlobalX/PockyBot.NET/blob/master/CONTRIBUTING.md).
