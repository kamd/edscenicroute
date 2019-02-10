#!/bin/bash
PUBLISHDIR=/var/www/elite
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
pushd $DIR
pushd EDScenicRouteWeb/EDScenicRouteWeb.Server/
rm -rf bin/Release
mkdir -p bin/Release/netcoreapp2.2
ln -s $PUBLISHDIR bin/Release/netcoreapp2.2/publish
dotnet publish -c Release -p:PublishWithAspNetCoreTargetManifest=false
popd
pushd EDScenicRouteWeb/EDScenicRouteWeb.Server/edclientapp
npm run build
rsync -vra --delete build $PUBLISHDIR
popd
exit 0

