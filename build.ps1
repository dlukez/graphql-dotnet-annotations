# Setup
$ErrorActionPreference = "Stop"
if (-not $env:Configuration) { $env:Configuration = "Release" }
if (-not $env:BuildRunner) { $env:PackageVersion = (gitversion | ConvertFrom-Json).NuGetVersionV2 }

# Helper functions
function Invoke-BuildStep { param([scriptblock]$cmd) & $cmd; if ($LASTEXITCODE -ne 0) { exit 1 } }

# Build
Invoke-BuildStep { dotnet restore src/GraphQL.Annotations/GraphQL.Annotations.csproj }
Invoke-BuildStep { dotnet pack src/GraphQL.Annotations/GraphQL.Annotations.csproj --include-symbols }

Invoke-BuildStep { dotnet restore test/GraphQL.Annotations.Tests/GraphQL.Annotations.Tests.csproj }
Invoke-BuildStep { dotnet test test/GraphQL.Annotations.Tests/GraphQL.Annotations.Tests.csproj }

# End
exit
