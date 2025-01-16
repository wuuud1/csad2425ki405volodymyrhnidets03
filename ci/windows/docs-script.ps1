$repoDir = (Get-Item -Path $PSScriptRoot).Parent.Parent.FullName
$binDir = "$repoDir\bin"
$srcDir = "$repoDir\Lab3-4\Client"
$docsDir = "$repoDir\ci-build\docs"
$latexDir = "$docsDir\latex"
$xmlDir = "$docsDir\xml"
$htmlDir = "$docsDir\html"

if (-not (Test-Path $binDir))
{
    New-Item -ItemType Directory -Path $binDir | Out-Null
}
if (-not (Test-Path $docsDir))
{
    New-Item -ItemType Directory -Path $docsDir | Out-Null
}
if (-not (Test-Path $xmlDir))
{
    New-Item -ItemType Directory -Path $xmlDir | Out-Null
}
if (-not (Test-Path $htmlDir))
{
    New-Item -ItemType Directory -Path $htmlDir | Out-Null
}

$doxygenUrl = "https://github.com/doxygen/doxygen/releases/download/Release_1_12_0/doxygen-1.12.0.windows.x64.bin.zip"
$doxygenZipFile = "$binDir\doxygen.zip"
$doxygenDir = "$binDir\doxy"

Invoke-WebRequest -Uri $doxygenUrl -OutFile $doxygenZipFile
Write-Host "Downloaded Doxygen to $doxygenZipFile"

Expand-Archive -Path $doxygenZipFile -DestinationPath $doxygenDir -Force
Write-Host "Expanded Doxygen to $doxygenDir"

$doxygenExeFile = "$doxygenDir\doxygen.exe"
$env:Path += ";$($doxygenExeFile)"

$doxygenConfigFile = "$docsDir\Doxyfile"
if (-not (Test-Path $doxygenConfigFile))
{
    & $doxygenExeFile -g $doxygenConfigFile
}

$projectName = "csad2425ki405volodymyrhnidets03"

(Get-Content $doxygenConfigFile) -replace 'PROJECT_NAME\s*=\s*\".*\"', "PROJECT_NAME = `"$projectName`"" |
    Set-Content $doxygenConfigFile
(Get-Content $doxygenConfigFile) -replace 'INPUT\s*=\s*.*', "INPUT = `"$srcDir`"" |
    Set-Content $doxygenConfigFile
(Get-Content $doxygenConfigFile) -replace 'GENERATE_XML\s*=\s*NO', "GENERATE_XML = YES" |
    Set-Content $doxygenConfigFile
(Get-Content $doxygenConfigFile) -replace 'XML_OUTPUT\s*=\s*.*', "XML_OUTPUT = `"$xmlDir`"" |
    Set-Content $doxygenConfigFile
(Get-Content $doxygenConfigFile) -replace 'GENERATE_HTML\s*=\s*NO', "GENERATE_HTML = YES" |
    Set-Content $doxygenConfigFile
(Get-Content $doxygenConfigFile) -replace 'HTML_OUTPUT\s*=\s*.*', "HTML_OUTPUT = `"$htmlDir`"" |
    Set-Content $doxygenConfigFile
(Get-Content $doxygenConfigFile) -replace 'RECURSIVE\s*=\s*.*', "RECURSIVE = YES" |
    Set-Content $doxygenConfigFile
$content = Get-Content $doxygenConfigFile -Raw
$content = $content -replace 'FILE_PATTERNS\s*=([\s\S]*?)(?=\n[A-Z]|$)', "FILE_PATTERNS = *.cs`n"
Set-Content -Path $doxygenConfigFile -Value $content
(Get-Content $doxygenConfigFile) -replace 'LATEX_OUTPUT\s*=\s*.*', "LATEX_OUTPUT = `"$latexDir`"" |
    Set-Content $doxygenConfigFile

& $doxygenExeFile $doxygenConfigFile

Write-Host "Docs generated to $docsDir"
