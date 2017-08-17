#!bin/bash
set -e
dotnet restore MemoryProject.sln
rm -rf $(pwd)/publish/api
rm -rf $(pwd)/publish/web
dotnet publish MemoryServer/MemoryServer.csproj -c debug -o $(pwd)/publish/api
dotnet publish MemoryClient.Web/MemoryClient.Web.csproj -c debug -o $(pwd)/publish/web