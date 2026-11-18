$ErrorActionPreference='Stop'
$root=Split-Path $PSScriptRoot
$out=Join-Path $root '.test-output'
New-Item -ItemType Directory -Force $out | Out-Null
$argsList=@('/nologo','/target:library','/warn:0',('/out:"'+$out+'\Assembly-CSharp.dll"'))
Get-ChildItem "$root\Builds\Windows\Light_Data\Managed\*.dll" | Where-Object {$_.Name -like 'Unity*.dll' -or $_.Name -in @('netstandard.dll','Assembly-CSharp-firstpass.dll')} | ForEach-Object {$argsList+=('/reference:"'+$_.FullName+'"')}
Get-ChildItem "$root\LegacyPlayerSource\*.cs" | ForEach-Object {$argsList+=('"'+$_.FullName+'"')}
$rsp=Join-Path $out 'legacy.rsp'
$argsList | Set-Content $rsp -Encoding UTF8
& "$env:WINDIR\Microsoft.NET\Framework64\v4.0.30319\csc.exe" ('@'+$rsp)
if($LASTEXITCODE -ne 0){throw 'Compilation failed'}
Write-Host "Compiled $out\Assembly-CSharp.dll. Close the game before manually installing it into Builds\Windows\Light_Data\Managed."