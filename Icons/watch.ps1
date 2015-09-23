# watch a file changes in the current directory,
# execute all tests when a file is changed or renamed

param($BatFile)

$watcher = New-Object System.IO.FileSystemWatcher
$watcher.Path = Get-Location
$watcher.IncludeSubdirectories = $false
$watcher.EnableRaisingEvents = $false
$watcher.NotifyFilter = [System.IO.NotifyFilters]::LastWrite -bor [System.IO.NotifyFilters]::FileName

& $BatFile
while ($true) {
    $result = $watcher.WaitForChanged([System.IO.WatcherChangeTypes]::Changed -bor [System.IO.WatcherChangeTypes]::Renamed -bOr [System.IO.WatcherChangeTypes]::Created, 1000);
    if ($result.TimedOut) {
        continue
    }

    if ($result.Name.EndsWith(".png")) {
        continue;
    }

    Start-Sleep -s 1

    "Change in $($result.Name)"
    & $BatFile
}