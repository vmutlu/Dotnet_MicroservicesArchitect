#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/Gateway/OnlineAuctionApp.APIGateway/OnlineAuctionApp.APIGateway.csproj", "src/Gateway/OnlineAuctionApp.APIGateway/"]
RUN dotnet restore "src/Gateway/OnlineAuctionApp.APIGateway/OnlineAuctionApp.APIGateway.csproj"
COPY . .
WORKDIR "/src/src/Gateway/OnlineAuctionApp.APIGateway"
RUN dotnet build "OnlineAuctionApp.APIGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OnlineAuctionApp.APIGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OnlineAuctionApp.APIGateway.dll"]
