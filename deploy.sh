#!/bin/bash
PUBLISHDIR=/var/www/elite
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
pushd $DIR
pushd EDScenicRouteWeb/EDScenicRouteWeb.Server/
rm -rf bin/Release
mkdir -p bin/Release/netcoreapp2.1
ln -s $PUBLISHDIR bin/Release/netcoreapp2.1/publish
dotnet publish -c Release -p:PublishWithAspNetCoreTargetManifest=false
cp EDScenicRouteWeb.Client.blazor.config $PUBLISHDIR
popd
pushd EDScenicRouteWeb/EDScenicRouteWeb.Client
rsync -vra --delete wwwroot $PUBLISHDIR
popd
exit 0

