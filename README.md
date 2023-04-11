# Fluff

Pretty-print pretty and minimal logs for C# applications. The formatting was heavily inspired by [log by Charm](https://github.com/charmbracelet/log).

# Usage

First, install the package using Nuget:
```sh
dotnet add package Fluff
```

Then, import it into your project:
```cs
using Fluff;
```

## Levels

Messages sent to the logger are sent with a specific *level*, which defines the severity of the message.

```cs
// Debugging messages/actions.
Log.Level.Debug

// Events that may be worth noting.
Log.Level.Info

// Errors which can be handled.
Log.Level.Warning

// Critical, non-crashing errors, which may be dismissable.
Log.Level.Error

// System is unstable and/or should exit imminently
Log.Level.Fatal
```

By default, everything more severe than `Info`-messages are logged to the console.

## Logger configuration

```cs
Log.Logger.Options = new Log.LogOptions
{
    IncludeCaller = true,
    IncludeTime = true,
    TimeFormat = Log.TimeFormat.Latin,
    Level = Log.Level.Warning
};
```

Alternatively, you can change individual fields:
```cs
Log.Logger.Options.Prefix = "Backend";
```

## Formatting

To attach attributes to a message, you can add an object to the invocation:

```cs
Log.Debug("User logged in.", new { username = "max", ip = "localhost" });
```

![](https://i.imgur.com/83nlYa9.png)