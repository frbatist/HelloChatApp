#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HelloChatApp.BotConsole/HelloChatApp.BotConsole.csproj", "HelloChatApp.BotConsole/"]
COPY ["HelloChatApp/Shared/HelloChatApp.Shared.csproj", "HelloChatApp/Shared/"]
RUN dotnet restore "HelloChatApp.BotConsole/HelloChatApp.BotConsole.csproj"
COPY . .
WORKDIR "/src/HelloChatApp.BotConsole"
RUN dotnet build "HelloChatApp.BotConsole.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HelloChatApp.BotConsole.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HelloChatApp.BotConsole.dll"]