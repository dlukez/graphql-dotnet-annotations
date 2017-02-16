# Setup
$ErrorActionPreference = "Stop"
if (-not $env:Configuration) { $env:Configuration = "Release" }
if (-not $env:PackageVersion) { $env:PackageVersion = (gitversion | ConvertFrom-Json).NuGetVersionV2 }
if ($env:BuildRunner) { & ./tools/dotnet-install.ps1 -Version 1.0.0-rc4-004771 -Architecture x86 }
function Invoke-BuildStep { param([scriptblock]$cmd) & $cmd; if ($LASTEXITCODE -ne 0) { exit 1 } }

# Build
Set-Location src/GraphQL.Annotations
Invoke-BuildStep { dotnet restore }
Invoke-BuildStep { dotnet build }
Invoke-BuildStep { dotnet pack --include-symbols --no-build  }

# End
exit
