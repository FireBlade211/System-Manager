param(
    [string]$ZipFile = "Samples.zip"
)

# If you specify maxCpuCount without a value, it will use the amount of threads on the PC
MSBUILD System-Manager_PluginSamples.slnx /m '/interactive:False' '/p:Configuration=Release;TargetFrameworkVersion=v4.5.2'

if ($LASTEXITCODE -ne 0) {
    $answer = (Read-Host "The build failed. Do you want to continue anyway? (y/n)") -match '^(y|yes)$'

    if (!$answer) {
        return
    }
}

# Find all the built files.
$files = Get-ChildItem -Path $PSScriptRoot -Filter '*.dll' -File -Exclude "Properties.Resources.Designer.cs.dll" -Recurse |
    Where-Object { $_.Name -notlike 'FireBlade.*' }

$files += Get-Item (Join-Path $PSScriptRoot 'README.md')
$files += Get-Item (Join-Path $PSScriptRoot '..\LICENSE')

# Move them into a ZIP folder.
$temp = Join-Path ([System.IO.Path]::GetTempPath()) ([System.Guid]::NewGuid().ToString())
New-Item -ItemType Directory -Path $temp | Out-Null

try {
    foreach ($file in $files) {
        Copy-Item -LiteralPath $file.FullName -Destination $temp
    }

    Compress-Archive -Path (Join-Path $temp '*') `
        -DestinationPath (Join-Path $PSScriptRoot $ZipFile) `
        -Force
}
finally {
    Remove-Item -LiteralPath $temp -Recurse -Force
}