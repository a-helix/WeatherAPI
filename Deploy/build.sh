#!/bin/bash
sudo rm -r src
echo "Building WeatherAPI..."
cd ..
dotnet build WeatherAPI.sln -c Release -o "src/"
echo "WeatherAPI has been built."
echo "Building container..."
cd Deploy
docker build Dockerfile -t api
echo "Container has been built."