# TODO: timeout in 10 minutes  10*60
..\RemoteDebugger\x64\msvsmon.exe /silent /timeout 120
do {
    Start-Sleep -Seconds 5  # wait for a few seconds so that the processes are started.
	$msvsmon = Get-Process | where ProcessName -eq msvsmon
} while ($msvsmon)