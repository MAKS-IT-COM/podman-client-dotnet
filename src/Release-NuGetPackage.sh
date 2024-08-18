#!/bin/sh

# Retrieve the API key from the environment variable
apiKey=$NUGET_MAKS_IT
if [ -z "$apiKey" ]; then
    echo "Error: API key not found in environment variable NUGET_MAKS_IT."
    exit 1
fi

# NuGet source
nugetSource="https://api.nuget.org/v3/index.json"

# Define paths
scriptDir=$(dirname "$0")
solutionDir=$(realpath "$scriptDir")
projectDir="$solutionDir/PodmanClient"
outputDir="$projectDir/bin/Release"

# Clean previous builds
echo "Cleaning previous builds..."
dotnet clean "$projectDir" -c Release

# Build the project
echo "Building the project..."
dotnet build "$projectDir" -c Release

# Pack the NuGet package
echo "Packing the project..."
dotnet pack "$projectDir" -c Release --no-build

# Look for the .nupkg file
packageFile=$(find "$outputDir" -name "*.nupkg" -print0 | xargs -0 ls -t | head -n 1)

if [ -n "$packageFile" ]; then
    echo "Package created successfully: $packageFile"
    
    # Push the package to NuGet
    echo "Pushing the package to NuGet..."
    dotnet nuget push "$packageFile" -k "$apiKey" -s "$nugetSource" --skip-duplicate
    
    if [ $? -eq 0 ]; then
        echo "Package pushed successfully."
    else
        echo "Failed to push the package."
    fi
else
    echo "Package creation failed. No .nupkg file found."
    exit 1
fi
