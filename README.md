# Discord.Net
<p align="center">
  <a href="https://discordnet.dev/" title="Click to visit the documentation!">
    <img src="https://raw.githubusercontent.com/discord-net/Discord.Net/dev/docs/marketing/logo/SVG/Combinationmark%20White%20Border.svg" alt="Logo">
  </a>
    <br />
    <br />
  <a href="https://www.nuget.org/packages/Discord.Net/">
    <img src="https://img.shields.io/nuget/vpre/Discord.Net.svg?maxAge=2592000?style=plastic" alt="NuGet">
  </a>
  <a href="https://www.myget.org/feed/Packages/discord-net">
    <img src="https://img.shields.io/myget/discord-net/vpre/Discord.Net.svg" alt="MyGet">
  </a>
  <a href="https://dev.azure.com/discord-net/Discord.Net/_build/latest?definitionId=1&branchName=dev">
    <img src="https://dev.azure.com/discord-net/Discord.Net/_apis/build/status/discord-net.Discord.Net?branchName=dev" alt="Build Status">
  </a>
  <a href="https://discord.gg/dnet">
    <img src="https://discord.com/api/guilds/848176216011046962/widget.png" alt="Discord">
  </a>
</p>
Discord NET is an unofficial .NET API Wrapper for the Discord client (https://discord.com).

## Documentation

- [Nightly](https://discordnet.dev)

## Installation

### Stable (NuGet)

Our stable builds available from NuGet through the Discord.Net metapackage:

- [Discord.Net](https://www.nuget.org/packages/Discord.Net/)

The individual components may also be installed from NuGet:

- [Discord.Net.Commands](https://www.nuget.org/packages/Discord.Net.Commands/)
- [Discord.Net.Rest](https://www.nuget.org/packages/Discord.Net.Rest/)
- [Discord.Net.WebSocket](https://www.nuget.org/packages/Discord.Net.WebSocket/)
- [Discord.Net.Webhook](https://www.nuget.org/packages/Discord.Net.Webhook/)

### Unstable (MyGet)

Nightly builds are available through our MyGet feed (`https://www.myget.org/F/discord-net/api/v3/index.json`).

### Unstable (Labs)

Labs builds are available on nuget (`https://www.nuget.org/packages/Discord.Net.Labs/`) and myget (`https://www.myget.org/F/discord-net-labs/api/v3/index.json`).

## Compiling

In order to compile Discord.Net, you require the following:

### Using Visual Studio

- [Visual Studio 2017](https://www.microsoft.com/net/core#windowsvs2017)
- [.NET Core SDK](https://www.microsoft.com/net/download/core)

The .NET Core workload must be selected during Visual Studio installation.

### Using Command Line

- [.NET Core SDK](https://www.microsoft.com/net/download/core)

## Known compatibility issues

### WebSockets (Win7 and earlier)

.NET Core 1.1 does not support WebSockets on Win7 and earlier. This issue has been fixed since the release of .NET Core 2.1. It is recommended to target .NET Core 2.1 or above for your project if you wish to run your bot on legacy platforms; alternatively, you may choose to install the [Discord.Net.Providers.WS4Net](https://www.nuget.org/packages/Discord.Net.Providers.WS4Net/) package.

## How to use

Setting up labs in your project is really simple, here's how to do it:
1) Remove Discord.Net from your project
2) Add Discord.Net Labs nuget to your project
3) That's all!

> Additional installation info can be found [here](https://labs.discordnet.dev/guides/getting_started/labs.html).

## Branches

### Dev
This branch is kept up to date with dnets dev branch. we pull of it to ensure that labs will work with pre existing dnet code.

### release/3.x
This branch is what will be pushed to nuget, sometimes its not up to date as we wait for other features to be finished.

An increment of the MAJOR component indicates that breaking changes have been made to the library; consumers should check the release notes to determine what changes need to be made.

## Branches

### Release/X.X

Release branch following Major.Minor. Upon release, patches will be pushed to these branches.
New NuGet releases will be tagged on these branches.

### Dev

Development branch, available on MyGet. This branch is what pull requests are targetted to.

### Feature/X

Branches that target Dev, adding new features. Feel free to explore these branches and give feedback where necessary.

### Docs/X

Usually targets Dev. These branches are used to update documentation with either new features or existing feature rework.
