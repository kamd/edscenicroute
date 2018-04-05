#!/bin/bash
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
pushd $DIR
pushd EDScenicRouteWeb/EDScenicRouteWeb.Server/
dotnet publish -c Release -p:PublishWithAspNetCoreTargetManifest=false
cp EDScenicRouteWeb.Client.blazor.config /var/www/elite/
popd
popd
exit 0

