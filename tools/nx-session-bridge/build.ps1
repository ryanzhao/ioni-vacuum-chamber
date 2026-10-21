param(
    [Parameter(Mandatory = $true)][string]$NxBin,
    [Parameter(Mandatory = $true)][string]$Compiler
)
$ErrorActionPreference = 'Stop'
$nxAssembly = Join-Path $NxBin 'managed/NXOpen.dll'
$utilities = Join-Path $NxBin 'managed/NXOpen.Utilities.dll'
if (-not (Test-Path -LiteralPath $nxAssembly -PathType Leaf)) {
    throw 'NXOpen.dll was not found under the supplied NXBIN/managed directory.'
}
if (-not (Test-Path -LiteralPath $Compiler -PathType Leaf)) {
    throw 'Supply the actual compatible .NET Framework csc.exe path.'
}
$source = Join-Path $PSScriptRoot 'NxSessionBridge.cs'
$output = Join-Path $PSScriptRoot 'NxSessionBridge_v2.dll'
if (Test-Path -LiteralPath $output) {
    throw 'Output already exists. Preserve the loaded binary; build in a fresh private folder.'
}
& $Compiler /nologo /target:library "/out:$output" "/reference:$nxAssembly" "/reference:$utilities" /reference:System.Windows.Forms.dll /reference:System.Web.Extensions.dll $source
if ($LASTEXITCODE -ne 0) { throw 'Bridge compilation failed.' }
New-Item -ItemType Directory -Path (Join-Path $PSScriptRoot 'commands') -Force | Out-Null
Write-Output 'Compiled only. Load the bootstrap inside NX, then verify live inspection.'
