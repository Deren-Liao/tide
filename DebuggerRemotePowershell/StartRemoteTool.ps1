#New-PSSession 35.184.53.125 -Authentication Credssp -Credential Deren
# 104.196.225.68  aspnet-west1-b
# 35.185.42.18 aspnet-3

$SessionOptions = New-PSSessionOption –SkipCACheck –SkipCNCheck –SkipRevocationCheck
$session = New-PSSession 35.185.240.69 -UseSSL -SessionOption $SessionOptions -Credential Deren 
#Copy-Item -Path "C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\Remote Debugger\x64\" `
#    -Destination "C:\Users\deren\Documents\RemoteDebugger" -ToSession $session -Recurse
Enter-PSSession $session
# Start-Process .\msvsmon.exe -ArgumentList "/prepcomputer /public "  # -Verb runas
# Start-Process .\msvsmon.exe -ArgumentList "/silent " 
# Get-Process | where ProcessName -eq "msvsmon"