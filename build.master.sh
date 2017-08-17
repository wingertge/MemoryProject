#!bin/bash
set -e
dotnet restore MemoryProject.sln
rm -rf $(pwd)/publish/api
rm -rf $(pwd)/publish/web
ls --color=never
dotnet publish $(pwd)/MemoryServer/MemoryServer.csproj -c release -o $(pwd)/publish/api
dotnet publish $(pwd)/MemoryClient.Web/MemoryClient.Web.csproj -c release -o $(pwd)/publish/web