# Bet.Notifications.Worker

Bet.Notifications.Worker is a sample application demonstrating using `Bet.Notifications` library.

![Stand With Israel](../../img/IStandWithIsrael.png)


> The second letter in the Hebrew alphabet is the ב bet/beit. Its meaning is "house". In the ancient pictographic Hebrew it was a symbol resembling a tent on a landscape.

_Note: Pre-release packages are distributed via [feedz.io](https://f.feedz.io/kdcllc/bet-notifications/nuget/index.json)._
This sample application is demonstrating usage of `Bet.Notifications` nuget packages.
The console application is based on [`dotnet new --install console.di::2.0.0`](https://github.com/kdcllc/Bet.Extensions.Templating)


## Hire me

Please send [email](mailto:kingdavidconsulting@gmail.com) if you consider to **hire me**.

[![buymeacoffee](https://www.buymeacoffee.com/assets/img/custom_images/orange_img.png)](https://www.buymeacoffee.com/vyve0og)

## Give a Star! :star:

If you like or are using this project to learn or start your solution, please give it a star. Thanks!

## Usage

In order for the Razor Pages to be rendered please include this in your project:

```xml
  <PropertyGroup>
    <PreserveCompilationContext>true</PreserveCompilationContext>
  </PropertyGroup>
```
In addition, if you have a template directory that must be copied to the bin directory then use the following configuration in the project file:

```xml
  <ItemGroup>
    <Content Include="Views\**\*" CopyToOutputDirectory="PreserveNewest" />
  </ItemGroup>
```
