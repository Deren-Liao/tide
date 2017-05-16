$ErrorActionPreference = "Stop"

$DebuggerPath = "C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\Remote Debugger\x64\"

Invoke-Command -Session $session -ScriptBlock { $DestinationPath = "..\RemoteDebugger" }
Invoke-Command -Session $session -ScriptBlock { $copyComplete = "copy-msvsmon-complete" }
Invoke-Command -Session $session -ScriptBlock { $setupComplete = "setup-msvsmon-complete" }


Invoke-Command -Session $session -ScriptBlock {if (!(Test-Path $DestinationPath)) {mkdir $DestinationPath}}
Invoke-Command -Session $session -ScriptBlock {cd $DestinationPath}

$DestinationFullPath = Invoke-Command -Session $session -ScriptBlock { Get-Location } 
$DestinationFullPath = $DestinationFullPath.Path

function Install()
{
    Copy-Item -Path $DebuggerPath -Destination $DestinationFullPath -ToSession $session -Recurse -Force 
    Invoke-Command -Session $session -ScriptBlock {New-Item $copyComplete -type file}
}

function Setup-Msvsmon
{
    Invoke-Command -Session $session -ScriptBlock { .\msvsmon.exe /prepcomputer /public  }
    Invoke-Command -Session $session -ScriptBlock {New-Item $setupComplete -type file}
}

function Start-Msvsmon
{
    Invoke-Command -Session $session -ScriptBlock { .\msvsmon.exe /silent  }
}

if (!(Invoke-Command -Session $session -ScriptBlock { Test-Path $copyComplete })) {
    Install
}

Invoke-Command -Session $session -ScriptBlock { cd ".\x64" }

if (!(Invoke-Command -Session $session -ScriptBlock { Test-Path $setupComplete })) {
    Setup-Msvsmon
}

Start-Msvsmon

