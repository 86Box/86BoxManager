@echo off
mkdir dist
dotnet publish 86BoxManager -r win-x64   -c Release --self-contained true -o dist\win
dotnet publish 86BoxManager -r linux-x64 -c Release --self-contained true -o dist\lin
dotnet publish 86BoxManager -r osx-x64   -c Release --self-contained true -o dist\osx
