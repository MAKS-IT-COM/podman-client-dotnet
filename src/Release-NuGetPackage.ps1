# Retrieve the API key from the environment variable
$apiKey = $env:NUGET_MAKS_IT
if (-not $apiKey) {
    Write-Host "Error: API key not found in environment variable NUGET_MAKS_IT."
    exit 1
}

# NuGet source
$nugetSource = "https://api.nuget.org/v3/index.json"

# Define paths
$solutionDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$projectDir = "$solutionDir\PodmanClient"
$outputDir = "$projectDir\bin\Release"

# Clean previous builds
Write-Host "Cleaning previous builds..."
dotnet clean $projectDir -c Release

# Build the project
Write-Host "Building the project..."
dotnet build $projectDir -c Release

# Pack the NuGet package
Write-Host "Packing the project..."
dotnet pack $projectDir -c Release --no-build

# Look for the .nupkg file
$packageFile = Get-ChildItem -Path $outputDir -Filter "*.nupkg" -Recurse | Sort-Object LastWriteTime -Descending | Select-Object -First 1

if ($packageFile) {
    Write-Host "Package created successfully: $($packageFile.FullName)"
    
    # Push the package to NuGet
    Write-Host "Pushing the package to NuGet..."
    dotnet nuget push $packageFile.FullName -k $apiKey -s $nugetSource --skip-duplicate
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "Package pushed successfully."
    } else {
        Write-Host "Failed to push the package."
    }
} else {
    Write-Host "Package creation failed. No .nupkg file found."
    exit 1
}
