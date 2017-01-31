param (
  [Parameter(Position = 1)]
  [string]$PrereleaseTag = $env:PrereleaseTag,
  [string]$Configuration = "Release"
)

dotnet restore
if ($LASTEXITCODE -ne 0) { exit 1 }

dotnet build src/GraphQL.Annotations/ --configuration $Configuration
if ($LASTEXITCODE -ne 0) { exit 1 }

dotnet test test/GraphQL.Annotations.Tests/ --configuration $Configuration
if ($LASTEXITCODE -ne 0) { exit 1 }

dotnet pack src/GraphQL.Annotations/ --configuration $Configuration --version-suffix $PrereleaseTag
if ($LASTEXITCODE -ne 0) { exit 1 }

exit 0