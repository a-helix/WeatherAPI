#!/bin/bash
sudo rm -r src
dotnet build WeatherAPI.sln -c Release -o "src/"
docker build Dockerfile -t api
