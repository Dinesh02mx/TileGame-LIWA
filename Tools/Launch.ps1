param([ValidateSet('Simulation','Hardware')][string]$Mode='Simulation',[string]$LocalAddress='192.168.4.1')
$ErrorActionPreference='Stop'
$root=Split-Path $PSScriptRoot
$app=Join-Path $root 'Builds\Windows'
$config=Get-Content (Join-Path $root "Config\$Mode.json") -Raw | ConvertFrom-Json
if($Mode -eq 'Hardware') {
 $addresses=@([Net.NetworkInformation.NetworkInterface]::GetAllNetworkInterfaces() | ForEach-Object {$_.GetIPProperties().UnicastAddresses} | ForEach-Object {$_.Address.ToString()})
 if($LocalAddress -notin $addresses){throw "Ethernet address $LocalAddress is not configured on this PC. Configure the dedicated floor adapter first. Available: $($addresses -join ', ')"}
 $config.localAddress=$LocalAddress
 # Existing hardware profile is a /24 subnet. Different subnet sizes need a manually edited profile.
 $config.broadcastAddress=($LocalAddress -replace '\.\d+$','.255')
}
if(Get-Process Light -ErrorAction SilentlyContinue){throw 'A Light game is already running. Close it before switching mode.'}
$config | ConvertTo-Json -Depth 5 | Set-Content (Join-Path $app 'FloorConfig.json') -Encoding UTF8
Start-Process -FilePath (Join-Path $app 'Light.exe') -WorkingDirectory $app -ArgumentList '-screen-fullscreen 0 -screen-width 1280 -screen-height 800 -logFile Game.log'