param(
    [string]$Mode,         # 'real' for Arduino, 'demo' for Proteus
    [string]$Port          # COM port (e.g., COM1)
)

$ErrorActionPreference = "Stop"

$repoDir = (Get-Item -Path $PSScriptRoot).Parent.Parent.FullName
$binDir = "$repoDir\bin"
$hexOutputDir = "$repoDir\ci-build\server"
$arduinoCliPath = "$binDir\arduino-cli.exe"
$sketchPath = "$repoDir\Lab3\Server\firmware\firmware.ino"
$outputHexPath = "$hexOutputDir\firmware.ino.hex"
$testProjectPath = "$repoDir\Lab3\Client\Client.Domain.Tests\Client.Domain.Tests.csproj"
$testResultsPath = "$repoDir\ci-build\test-results"

Write-Host "Installing Arduino CLI..."
New-Item -ItemType Directory -Force -Path $binDir | Out-Null
Invoke-WebRequest -Uri "https://downloads.arduino.cc/arduino-cli/arduino-cli_latest_Windows_64bit.zip" -OutFile "$binDir\arduino-cli.zip"
Expand-Archive -Path "$binDir\arduino-cli.zip" -DestinationPath $binDir -Force
Remove-Item "$binDir\arduino-cli.zip"

Write-Host "Configuring Arduino CLI..."
& $arduinoCliPath config init --overwrite
& $arduinoCliPath core update-index
& $arduinoCliPath core install arduino:avr

Write-Host "Compiling Arduino code..."
New-Item -ItemType Directory -Force -Path $hexOutputDir | Out-Null
& $arduinoCliPath compile --fqbn arduino:avr:uno --output-dir $hexOutputDir $sketchPath

Write-Host "Saving HEX file..."
if (Test-Path $outputHexPath) {
    Write-Host "HEX file successfully saved to: $outputHexPath"
} else {
    Write-Host "Error: HEX file not found at $outputHexPath"
    Exit 1
}

if ($Mode -eq 'real') {
    if (-not $Port) {
        Write-Host "Error: COM port must be specified in 'real' mode."
        Exit 1
    }

    Write-Host "Uploading HEX file to Arduino on port $Port..."
    & $arduinoCliPath upload -p $Port --fqbn arduino:avr:uno -i $outputHexPath

    Write-Host "HEX file uploaded successfully to the Real Arduino."
} elseif ($Mode -eq 'demo') {
    Write-Host "Proteus (Demo) mode detected. Skipping upload to Arduino..."
    Write-Host "Load the HEX file into Proteus manually: $outputHexPath"
} else {
    Write-Host "Error: Invalid mode specified. Use 'real' for Real Arduino or 'demo' for Proteus."
    Exit 1
}

Write-Host "Running tests with COM port parameter..."
New-Item -ItemType Directory -Force -Path $testResultsPath | Out-Null

$testRunParameters = "TestRunParameters.Parameter(name=\`"COM_PORT\`", value=\`"$Port\`")"

dotnet test $testProjectPath `
    --logger "trx;LogFileName=$testResultsPath\test-results.trx" `
    -- $testRunParameters

Write-Host "CI process completed. HEX file saved to: $outputHexPath"
Write-Host "Test results saved to: $testResultsPath\test-results.trx"
