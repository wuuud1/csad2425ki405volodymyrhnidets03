param(
    [string]$Mode,  # 'real' for Arduino, 'demo' for Proteus
    [string]$Port   # COM port (e.g., COM1)
)

$ErrorActionPreference = "Stop"

# Define paths
$repoDir = (Get-Item -Path $PSScriptRoot).Parent.Parent.FullName
$testResultsPath = "$repoDir\ci-build\test-results"
$coverageOutputDir = "$repoDir\ci-build\coverage"
$solutionPath = "$repoDir\Lab3-5\Client\Client.sln"
$testProjectPath = "$repoDir\Lab3-5\Client\Client.Domain.Tests\Client.Domain.Tests.csproj"

# Ensure fresh directories for reports
Write-Host "Preparing directories and cleaning up old reports..."
if (Test-Path $testResultsPath) {
    Remove-Item -Recurse -Force -Path $testResultsPath
}
if (Test-Path $coverageOutputDir) {
    Remove-Item -Recurse -Force -Path $coverageOutputDir
}
New-Item -ItemType Directory -Force -Path $testResultsPath | Out-Null
New-Item -ItemType Directory -Force -Path $coverageOutputDir | Out-Null

# Function to process coverage and extract percentage
function Get-CoveragePercentage {
    param([string]$CoverageReportPath)
    [xml]$coverageData = Get-Content $CoverageReportPath
    $lineRate = $coverageData.coverage.GetAttribute("line-rate")
    if ($lineRate -and $lineRate -match "^\d+(\.\d+)?$") {
        return [math]::Round(($lineRate -as [double]) * 100, 2)
    }
    return $null
}

# Run tests based on the provided Port parameter
if (-not [string]::IsNullOrEmpty($Port)) {
    Write-Host "Running client and server tests..."
    dotnet test $solutionPath `
        --logger "trx;LogFileName=$testResultsPath\server-results.trx" `
        --collect:"XPlat Code Coverage" `
        --results-directory "$coverageOutputDir\TestResults"
    Write-Host "Client and server tests completed."
} else {
    Write-Host "Running client tests..."
    dotnet test $testProjectPath `
        --logger "trx;LogFileName=$testResultsPath\client-results.trx" `
        --collect:"XPlat Code Coverage" `
        --results-directory "$coverageOutputDir\TestResults"
    Write-Host "Client tests completed."
}

# Analyze code coverage
$testCoverageReportPath = Get-ChildItem -Path "$coverageOutputDir\TestResults" -Recurse -Filter "*.cobertura.xml" | Select-Object -First 1
if ($testCoverageReportPath -ne $null) {
    $testCoveragePercents = Get-CoveragePercentage $testCoverageReportPath.FullName
    Write-Host "Test Code Coverage: $testCoveragePercents%"
} else {
    Write-Host "Error: No code coverage report found."
}

# Summary
Write-Host "CI process completed."
Write-Host "Test results saved to: $testResultsPath"
Write-Host "Code coverage reports saved to: $coverageOutputDir"