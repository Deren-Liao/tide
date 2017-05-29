﻿# Copyright 2017 Google Inc. All Rights Reserved.
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
#     http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.

$ErrorActionPreference = "Stop"

$debuggerSourcePath = "C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\Remote Debugger\x64\*"
if (!$debuggerSourcePath) {
	Write-Error "$debuggerSourcePath is not set"
}
Write-Output $debuggerSourcePath

Invoke-Command -Session $session -ScriptBlock { $destinationPath = "..\RemoteDebugger" }
Invoke-Command -Session $session -ScriptBlock { $copyComplete = "copy-msvsmon-complete" }
Invoke-Command -Session $session -ScriptBlock { $setupComplete = "setup-msvsmon-complete" }

Invoke-Command -Session $session -ScriptBlock {if (!(Test-Path $destinationPath)) {mkdir $destinationPath}}
Invoke-Command -Session $session -ScriptBlock {cd $destinationPath}

$destinationFullPath = Invoke-Command -Session $session -ScriptBlock { Get-Location }
$destinationFullPath = $destinationFullPath.Path
Write-Output $destinationFullPath

function Install()
{
    Copy-Item -Path $debuggerSourcePath -Destination $destinationFullPath -ToSession $session -Recurse -Force 
    Invoke-Command -Session $session -ScriptBlock {New-Item $copyComplete -type file}
}

function Setup-Msvsmon
{
    Invoke-Command -Session $session -ScriptBlock { .\msvsmon.exe /prepcomputer /public  }
    Invoke-Command -Session $session -ScriptBlock {New-Item $setupComplete -type file}
}

if (!(Invoke-Command -Session $session -ScriptBlock { Test-Path $copyComplete })) {
    Install
}

if (!(Invoke-Command -Session $session -ScriptBlock { Test-Path $setupComplete })) {
    Setup-Msvsmon
}

if (!(Invoke-Command -Session $session -ScriptBlock { Test-Path $setupComplete })) {
	Write-Error "not installed"
}
