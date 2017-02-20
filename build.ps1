# Setup
$ErrorActionPreference = "Stop"
if (-not $env:Configuration) { $env:Configuration = "Release" }
if (-not $env:PackageVersion) { $env:PackageVersion = (gitversion | ConvertFrom-Json).FullSemVer }

# Helper functions
function Invoke-BuildStep { param([scriptblock]$cmd) & $cmd; if ($LASTEXITCODE -ne 0) { exit 1 } }

# Build
Push-Location src/GraphQL.Annotations
Invoke-BuildStep { dotnet restore }
Invoke-BuildStep { dotnet pack --include-symbols }
Pop-Location

# End
exit
