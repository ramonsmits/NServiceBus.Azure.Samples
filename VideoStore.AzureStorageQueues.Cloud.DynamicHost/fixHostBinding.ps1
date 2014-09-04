$baseDir = Split-Path (Resolve-Path $MyInvocation.MyCommand.Path)
[xml]$xml = get-content "$baseDir\VideoStore.ContentManagement\packages.config"
$version = $xml.SelectSinglenode("/packages/package[@id='NServiceBus']").version
$version = $version.substring(0, $version.LastIndexOf("."))
$majorMinor = "$version.0.0"
"Setting version to $majorMinor"

$azureHostProcessConfig = "$baseDir\VideoStore.ContentManagement\NServiceBus.Hosting.Azure.HostProcess.exe.config"
set-content -path $azureHostProcessConfig -value ((get-content $azureHostProcessConfig) -replace "\d+\.\d+\.\d+\.\d+`"", "$majorMinor`"")