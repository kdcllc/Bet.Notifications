#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["bet.notifications.worker/bet.notifications.worker.csproj", "bet.notifications.worker/"]
RUN dotnet restore "bet.notifications.worker/bet.notifications.worker.csproj"
COPY . .
WORKDIR "/src/bet.notifications.worker"
RUN dotnet build "bet.notifications.worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "bet.notifications.worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "bet.notifications.worker.dll"]
