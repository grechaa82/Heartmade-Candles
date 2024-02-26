FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["HeartmadeCandles.API/HeartmadeCandles.API.csproj", "HeartmadeCandles.API/"]
COPY ["HeartmadeCandles.Admin.BL/HeartmadeCandles.Admin.BL.csproj", "HeartmadeCandles.Admin.BL/"]
COPY ["HeartmadeCandles.Admin.Core/HeartmadeCandles.Admin.Core.csproj", "HeartmadeCandles.Admin.Core/"]
COPY ["HeartmadeCandles.Admin.DAL/HeartmadeCandles.Admin.DAL.csproj", "HeartmadeCandles.Admin.DAL/"]
COPY ["HeartmadeCandles.Auth.BL/HeartmadeCandles.Auth.BL.csproj", "HeartmadeCandles.Auth.BL/"]
COPY ["HeartmadeCandles.Auth.Core/HeartmadeCandles.Auth.Core.csproj", "HeartmadeCandles.Auth.Core/"]
COPY ["HeartmadeCandles.Bot.BL/HeartmadeCandles.Bot.BL.csproj", "HeartmadeCandles.Bot.BL/"]
COPY ["HeartmadeCandles.Bot.Core/HeartmadeCandles.Bot.Core.csproj", "HeartmadeCandles.Bot.Core/"]
COPY ["HeartmadeCandles.Bot.DAL/HeartmadeCandles.Bot.DAL.csproj", "HeartmadeCandles.Bot.DAL/"]
COPY ["HeartmadeCandles.Constructor.BL/HeartmadeCandles.Constructor.BL.csproj", "HeartmadeCandles.Constructor.BL/"]
COPY ["HeartmadeCandles.Constructor.Core/HeartmadeCandles.Constructor.Core.csproj", "HeartmadeCandles.Constructor.Core/"]
COPY ["HeartmadeCandles.Constructor.DAL/HeartmadeCandles.Constructor.DAL.csproj", "HeartmadeCandles.Constructor.DAL/"]
COPY ["HeartmadeCandles.Migrations/HeartmadeCandles.Migrations.csproj", "HeartmadeCandles.Migrations/"]
COPY ["HeartmadeCandles.Order.BL/HeartmadeCandles.Order.BL.csproj", "HeartmadeCandles.Order.BL/"]
COPY ["HeartmadeCandles.Order.Core/HeartmadeCandles.Order.Core.csproj", "HeartmadeCandles.Order.Core/"]
COPY ["HeartmadeCandles.Order.DAL/HeartmadeCandles.Order.DAL.csproj", "HeartmadeCandles.Order.DAL/"]
RUN dotnet restore "HeartmadeCandles.API/HeartmadeCandles.API.csproj"
COPY . .
WORKDIR "/src/HeartmadeCandles.API"
RUN dotnet build "HeartmadeCandles.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HeartmadeCandles.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HeartmadeCandles.API.dll"]