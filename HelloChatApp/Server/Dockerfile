#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HelloChatApp/Server/HelloChatApp.Server.csproj", "HelloChatApp/Server/"]
COPY ["HelloChatApp/Client/HelloChatApp.Client.csproj", "HelloChatApp/Client/"]
COPY ["HelloChatApp/Shared/HelloChatApp.Shared.csproj", "HelloChatApp/Shared/"]
RUN dotnet restore "HelloChatApp/Server/HelloChatApp.Server.csproj"
COPY . .
WORKDIR "/src/HelloChatApp/Server"
RUN dotnet build "HelloChatApp.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HelloChatApp.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HelloChatApp.Server.dll"]