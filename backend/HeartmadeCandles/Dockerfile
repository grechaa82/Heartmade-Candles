FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]
COPY ["HeartmadeCandles.API/HeartmadeCandles.API.csproj", "HeartmadeCandles.API/"]
COPY ["HeartmadeCandles.Admin.BL/HeartmadeCandles.Admin.BL.csproj", "HeartmadeCandles.Admin.BL/"]
COPY ["HeartmadeCandles.Admin.Core/HeartmadeCandles.Admin.Core.csproj", "HeartmadeCandles.Admin.Core/"]
COPY ["HeartmadeCandles.Admin.DAL/HeartmadeCandles.Admin.DAL.csproj", "HeartmadeCandles.Admin.DAL/"]
COPY ["HeartmadeCandles.Bot.BL/HeartmadeCandles.Bot.BL.csproj", "HeartmadeCandles.Bot.BL/"]
COPY ["HeartmadeCandles.Bot.Core/HeartmadeCandles.Bot.Core.csproj", "HeartmadeCandles.Bot.Core/"]
COPY ["HeartmadeCandles.Order.Core/HeartmadeCandles.Order.Core.csproj", "HeartmadeCandles.Order.Core/"]
COPY ["HeartmadeCandles.Bot.DAL/HeartmadeCandles.Bot.DAL.csproj", "HeartmadeCandles.Bot.DAL/"]
COPY ["HeartmadeCandles.Constructor.BL/HeartmadeCandles.Constructor.BL.csproj", "HeartmadeCandles.Constructor.BL/"]
COPY ["HeartmadeCandles.Constructor.Core/HeartmadeCandles.Constructor.Core.csproj", "HeartmadeCandles.Constructor.Core/"]
COPY ["HeartmadeCandles.Constructor.DAL/HeartmadeCandles.Constructor.DAL.csproj", "HeartmadeCandles.Constructor.DAL/"]
COPY ["HeartmadeCandles.Migrations/HeartmadeCandles.Migrations.csproj", "HeartmadeCandles.Migrations/"]
COPY ["HeartmadeCandles.UserAndAuth.DAL/HeartmadeCandles.UserAndAuth.DAL.csproj", "HeartmadeCandles.UserAndAuth.DAL/"]
COPY ["HeartmadeCandles.UserAndAuth.Core/HeartmadeCandles.UserAndAuth.Core.csproj", "HeartmadeCandles.UserAndAuth.Core/"]
COPY ["HeartmadeCandles.Order.BL/HeartmadeCandles.Order.BL.csproj", "HeartmadeCandles.Order.BL/"]
COPY ["HeartmadeCandles.Order.DAL/HeartmadeCandles.Order.DAL.csproj", "HeartmadeCandles.Order.DAL/"]
COPY ["HeartmadeCandles.UserAndAuth.BL/HeartmadeCandles.UserAndAuth.BL.csproj", "HeartmadeCandles.UserAndAuth.BL/"]
RUN dotnet restore "./HeartmadeCandles.API/./HeartmadeCandles.API.csproj"

COPY . .

COPY ["HeartmadeCandles.API/StaticFiles", "/app/StaticFiles"]

WORKDIR "/src/HeartmadeCandles.API"
RUN dotnet build "./HeartmadeCandles.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HeartmadeCandles.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HeartmadeCandles.API.dll"]