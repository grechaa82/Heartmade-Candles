#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

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
COPY ["HeartmadeCandles.Migrations/HeartmadeCandles.Migrations.csproj", "HeartmadeCandles.Migrations/"]
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