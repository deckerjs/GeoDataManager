FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src
COPY ["GeoStoreAPI/GeoStoreAPI.csproj", "GeoStoreAPI/"]
RUN dotnet restore "GeoStoreAPI/GeoStoreAPI.csproj"
COPY . .
WORKDIR "/src/GeoStoreAPI"
RUN dotnet build "GeoStoreAPI.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "GeoStoreAPI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "GeoStoreAPI.dll"]
