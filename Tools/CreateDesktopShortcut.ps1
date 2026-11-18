$root=Split-Path $PSScriptRoot
$shell=New-Object -ComObject WScript.Shell
$link=$shell.CreateShortcut((Join-Path ([Environment]::GetFolderPath('Desktop')) 'LIWA Floor Game.lnk'))
$link.TargetPath=Join-Path $root 'Play.cmd'
$link.WorkingDirectory=$root
$link.IconLocation=Join-Path $root 'Builds\Windows\Light.exe'
$link.Save()
Write-Host 'Desktop shortcut created (simulation). Use Play.cmd -Mode Hardware for the connected floor.'