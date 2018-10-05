#!/bin/bash
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
pushd $DIR
pushd EDScenicRouteWeb/EDScenicRouteWeb.Server/
rm -rf bin/Release
mkdir -p bin/Release/netcoreapp2.1
ln -s /var/www/elite bin/Release/netcoreapp2.1/publish
dotnet publish -c Release -p:PublishWithAspNetCoreTargetManifest=false
cp EDScenicRouteWeb.Client.blazor.config /var/www/elite/
popd
popd
exit 0

