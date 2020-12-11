#!/bin/bash
if [[ -d src ]]
	then
		echo "Deleting old src..."
		sudo rm -r src
		echo "Done."
fi
echo "Building WeatherAPI..."
cd ..
dotnet build WeatherAPI.sln -c Release -o "src/"
echo "WeatherAPI has been built."
echo "Building container..."
cd Deploy
sudo mv Dockerfile ..
cd ..
docker build -t api .
echo "Container has been built."