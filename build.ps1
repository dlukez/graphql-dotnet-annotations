param (
    [string]$PrereleaseTag = ${env:GitVersion.PreReleaseTag},
    [string]$Configuration = $env:Configuration
)

if (-not $PrereleaseTag) {
    if ($env:PrereleaseTag) {
        $PrereleaseTag = $env:PrereleaseTag
    } else {
        $PrereleaseTag = "dev"
    }
}

if (-not $Configuration) {
    $Configuration = "Release"
}

$ErrorActionPreference = "Stop"

function Test-ExitCode {
    if ($LASTEXITCODE -ne 0) {
        exit 1
    }
}

dotnet restore
Test-ExitCode

dotnet build src/GraphQL.Annotations/ --configuration $Configuration
Test-ExitCode

dotnet test test/GraphQL.Annotations.Tests/ --configuration $Configuration
Test-ExitCode

dotnet pack src/GraphQL.Annotations/ --configuration $Configuration --no-build --version-suffix $PrereleaseTag
Test-ExitCode

exit
