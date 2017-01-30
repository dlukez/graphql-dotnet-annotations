param (
  [Parameter(Mandatory=$true)]
  [string]$PrereleaseTag,
  [Parameter()]
  [string]$Configuration = "Release"
)

dotnet restore
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

dotnet build src/GraphQL.Annotations/ --configuration $Configuration
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

dotnet test test/GraphQL.Annotations.Tests/ --configuration $Configuration
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

dotnet pack src/GraphQL.Annotations/ --configuration $Configuration --version-suffix $PrereleaseTag
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

exit 0