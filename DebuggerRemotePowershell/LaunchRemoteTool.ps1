$ErrorActionPreference = "Stop"

#Get-Location 

function Start-Msvsmon
{
    dir .\msvsmon.exe
    .\msvsmon.exe /prepcomputer /public  
}

if (Test-Path "setup-msvsmon-complete") {
   Start-Msvsmon
}
elseif (Test-Path "copy-msvsmon-complete") {
    dir .\msvsmon.exe
    .\msvsmon.exe /prepcomputer /public      
    New-Item "setup-msvsmon-complete" -type file

    Start-Msvsmon
}
else {
    return "not installed"
}