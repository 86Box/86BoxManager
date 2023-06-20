#!/bin/sh
mkdir dist
dotnet publish 86BoxManager -r win-x64   -c Release --self-contained true -o dist/win
dotnet publish 86BoxManager -r linux-x64 -c Release --self-contained true -o dist/lin
dotnet publish 86BoxManager -r osx-x64   -c Release --self-contained true -o dist/osx
mkdir pub
zip -q -r pub/86BoxManager_win.zip     dist/win
tar -czf pub/86BoxManager_linux.tar.gz dist/lin
tar -czf pub/86BoxManager_mac.tar.gz   dist/osx
