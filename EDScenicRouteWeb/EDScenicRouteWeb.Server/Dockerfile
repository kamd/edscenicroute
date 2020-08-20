
# Build react frontend
FROM node:10-alpine as build-node
WORKDIR /app
COPY EDScenicRouteWeb/EDScenicRouteWeb.Server/edclientapp/package.json .
COPY EDScenicRouteWeb/EDScenicRouteWeb.Server/edclientapp/package-lock.json .
RUN npm install
COPY EDScenicRouteWeb/EDScenicRouteWeb.Server/edclientapp/public/ ./public/ 
COPY EDScenicRouteWeb/EDScenicRouteWeb.Server/edclientapp/src/ ./src/ 
COPY EDScenicRouteWeb/EDScenicRouteWeb.Server/edclientapp/tsconfig.json .
RUN npm run build  

# Build ASP.NET Core backend
FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
WORKDIR /app
# Copy csprojs and restore from nuget
COPY EDScenicRouteWeb/EDScenicRouteWeb.Server/EDScenicRouteWeb.Server.csproj ./EDScenicRouteWeb/EDScenicRouteWeb.Server/
RUN export DOTNET_SYSTEM_NET_HTTP_USESOCKETSHTTPHANDLER=0
RUN dotnet restore EDScenicRouteWeb/EDScenicRouteWeb.Server/EDScenicRouteWeb.Server.csproj
# Copy everything else and build
COPY . ./
RUN dotnet publish EDScenicRouteWeb/EDScenicRouteWeb.Server/EDScenicRouteWeb.Server.csproj -c Release -o /app/out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY --from=build-env /app/out .
COPY --from=build-node /app/build ./edclientapp/
RUN mkdir /tmp/pois
RUN mkdir /tmp/systems
EXPOSE 6788

ENTRYPOINT ["dotnet", "EDScenicRouteWeb.Server.dll"]
