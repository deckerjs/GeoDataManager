FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["GeoDataWebPortal/GeoDataWebPortal.csproj", "GeoDataWebPortal/"]
RUN dotnet restore "GeoDataWebPortal/GeoDataWebPortal.csproj"
COPY . .
WORKDIR /src/GeoDataWebPortal
RUN dotnet build "GeoDataWebPortal.csproj" -c Release -o /app

FROM node:10-alpine AS build-node
WORKDIR /ClientApp
COPY GeoDataWebPortal/ClientApp/ .
RUN npm install
RUN npm run buildprod

FROM build AS publish
RUN dotnet publish "GeoDataWebPortal.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
COPY --from=build-node /ClientApp/dist ./ClientApp/dist
ENTRYPOINT ["dotnet", "GeoDataWebPortal.dll"]
