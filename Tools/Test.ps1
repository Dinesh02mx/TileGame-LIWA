$ErrorActionPreference='Stop'
$root=Split-Path $PSScriptRoot
$csc=Join-Path $env:WINDIR 'Microsoft.NET\Framework64\v4.0.30319\csc.exe'
$out=Join-Path $root '.test-output'
New-Item -ItemType Directory -Force $out | Out-Null
$core=Join-Path $root 'UnityProject\Assets\Scripts\FloorCore.cs'
foreach($test in Get-ChildItem "$root\Tests\Verify*.cs") {
 $exe=Join-Path $out ($test.BaseName+'.exe')
 $sources=@($core,$test.FullName)
 if($test.BaseName -eq 'VerifyIdleStart'){$sources+=@("$root\UnityProject\Assets\Scripts\FloorNetwork.cs","$root\UnityProject\Assets\Scripts\InteractionLight.cs")}
 & $csc /nologo /warn:0 "/out:$exe" $sources
 if($LASTEXITCODE -ne 0){throw "Compile failed: $test"}
 & $exe
 if($LASTEXITCODE -ne 0){throw "Test failed: $test"}
}