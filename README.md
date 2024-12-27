# Bet.Notifications

[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg?style=flat-square)](https://raw.githubusercontent.com/kdcllc/Bet.Notifications.Abstractions/master/LICENSE)
![master workflow](https://github.com/kdcllc/Bet.Notifications/actions/workflows/master.yml/badge.svg)
[![NuGet](https://img.shields.io/nuget/v/Bet.Notifications.Abstractions.svg)](https://www.nuget.org/packages?q=Bet.Notifications.Abstractions)
![Nuget](https://img.shields.io/nuget/dt/Bet.Notifications.Abstractions)
[![feedz.io](https://img.shields.io/badge/endpoint.svg?url=https://f.feedz.io/kdcllc/bet-notifications/shield/Bet.Notifications.Abstractions/latest)](https://f.feedz.io/kdcllc/bet-notifications/packages/Bet.Notifications.Abstractions/latest/download)

> The second letter in the Hebrew alphabet is the ב bet/beit. Its meaning is "house". In the ancient pictographic Hebrew it was a symbol resembling a tent on a landscape.

_Note: Pre-release packages are distributed via [feedz.io](https://f.feedz.io/kdcllc/bet-notifications/nuget/index.json)._

This goal of this repo is to provide with a reusable functionality for developing Microservices with Docker and Kubernetes.
The core functionality that is provided is the ability to generate notifications `emails` or `sms` messages.

## Hire me

Please send [email](mailto:kingdavidconsulting@gmail.com) if you consider to **hire me**.

[![buymeacoffee](https://www.buymeacoffee.com/assets/img/custom_images/orange_img.png)](https://www.buymeacoffee.com/vyve0og)

## Give a Star! :star:

If you like or are using this project to learn or start your solution, please give it a star. Thanks!

## Projects

- [`Bet.Notifications`](./src/Bet.Notifications) - Generic host specific DI registrations.
- [`Bet.Notifications.Abstractions`](./src/Bet.Notifications.Abstractions) - The basic notification package.
- [`Bet.Notifications.Razor`](./src/Bet.Notifications.Razor) - The Razor Templating engine.
- [`Bet.Notifications.SendGrid`](./src/Bet.Notifications.SendGrid) - The Sender based on SendGrid.
- [`Bet.Notifications.AzureCommunication`](./src/Bet.Notifications.AzureCommunication) - The Sender based on Azure Communication Services - Email.
- [`Bet.Notifications.Worker`](./src/Bet.Notifications.Worker) - The sample project demostrating the nuget packages.

## Resources

- https://github.com/lukencode/FluentEmail
- https://github.com/sendgrid/sendgrid-csharp
- https://github.com/toddams/RazorLight/tree/master/samples/RazorLight.Samples
- https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/microservice-ddd-cqrs-patterns/implement-value-objects