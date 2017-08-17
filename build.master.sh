#!bin/bash
cd sln
set -e
dotnet restore MemoryProject.sln
rm -rf $(pwd)/publish/api
rm -rf $(pwd)/publish/web
dotnet publish $(pwd)/MemoryServer/MemoryServer.csproj -c release -o $(pwd)/publish/api
dotnet publish $(pwd)/MemoryClient.Web/MemoryClient.Web.csproj -c release -o $(pwd)/publish/web