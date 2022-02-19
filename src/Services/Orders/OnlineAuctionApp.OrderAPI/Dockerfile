#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/Services/Orders/OnlineAuctionApp.OrderAPI/OnlineAuctionApp.OrderAPI.csproj", "src/Services/Orders/OnlineAuctionApp.OrderAPI/"]
COPY ["src/Services/Orders/OnlineAuctionApp.Infrastructure/OnlineAuctionApp.Infrastructure.csproj", "src/Services/Orders/OnlineAuctionApp.Infrastructure/"]
COPY ["src/Services/Orders/OnlineAuctionApp.Domain/OnlineAuctionApp.Domain.csproj", "src/Services/Orders/OnlineAuctionApp.Domain/"]
COPY ["src/Services/Commons/OnlineAuctionApp.Core/OnlineAuctionApp.Core.csproj", "src/Services/Commons/OnlineAuctionApp.Core/"]
COPY ["src/Services/Orders/OnlineAuctionApp.Application/OnlineAuctionApp.Application.csproj", "src/Services/Orders/OnlineAuctionApp.Application/"]
RUN dotnet restore "src/Services/Orders/OnlineAuctionApp.OrderAPI/OnlineAuctionApp.OrderAPI.csproj"
COPY . .
WORKDIR "/src/src/Services/Orders/OnlineAuctionApp.OrderAPI"
RUN dotnet build "OnlineAuctionApp.OrderAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OnlineAuctionApp.OrderAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OnlineAuctionApp.OrderAPI.dll"]
