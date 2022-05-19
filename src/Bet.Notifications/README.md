# Bet.Notifications

[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg?style=flat-square)](https://raw.githubusercontent.com/kdcllc/Bet.Notifications/master/LICENSE)
![master workflow](https://github.com/kdcllc/Bet.Notifications/actions/workflows/master.yml/badge.svg)
[![NuGet](https://img.shields.io/nuget/v/Bet.Notifications.svg)](https://www.nuget.org/packages?q=Bet.Notifications)
![Nuget](https://img.shields.io/nuget/dt/Bet.Notifications)
[![feedz.io](https://img.shields.io/badge/endpoint.svg?url=https://f.feedz.io/kdcllc/bet-notifications/shield/Bet.Notifications/latest)](https://f.feedz.io/kdcllc/bet-notifications/packages/Bet.Notifications/latest/download)

> The second letter in the Hebrew alphabet is the ב bet/beit. Its meaning is "house". In the ancient pictographic Hebrew it was a symbol resembling a tent on a landscape.

_Note: Pre-release packages are distributed via [feedz.io](https://f.feedz.io/kdcllc/bet-notifications/nuget/index.json)._

This goal of this repo is to provide with a reusable functionality for developing Microservices with Docker and Kubernetes.

## Hire me

Please send [email](mailto:kingdavidconsulting@gmail.com) if you consider to **hire me**.

[![buymeacoffee](https://www.buymeacoffee.com/assets/img/custom_images/orange_img.png)](https://www.buymeacoffee.com/vyve0og)

## Give a Star! :star:

If you like or are using this project to learn or start your solution, please give it a star. Thanks!

## Usage

```csharp
        services.AddEmailConfigurator()
            .AddReplaceTempleteRenderer()
            .AddFileSystemEmailMessageHandler();
```
