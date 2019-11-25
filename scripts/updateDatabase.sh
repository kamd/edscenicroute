#!/bin/bash
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
pushd $DIR
dotnet ef database update --project EDScenicRouteCore/EDScenicRouteCore.csproj --startup-project EDScenicRouteWeb/EDScenicRouteWeb.Server/EDScenicRouteWeb.Server.csproj
popd
