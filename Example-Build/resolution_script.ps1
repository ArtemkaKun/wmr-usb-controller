$monitor_info = .\ChangeScreenResolution.exe /l
$monitor_info = $monitor_info.split([Environment]::NewLine)
$index = 0
$min_width = 1920
$min_height = 1080
$min_refesh = 60
[System.Collections.ArrayList]$set_VR_res_cmd_arr = @() # Array of set 1080p commands
[System.Collections.ArrayList]$reset_res_cmd_arr = @() # Array of reset monitor commands
[System.Collections.ArrayList]$ind_arr = @() # Array of matched indecies
[System.Collections.ArrayList]$monitor_info_arr = @() # Array of monitor info lines

# Getting each monitor line index in monitor info
foreach ($line in $monitor_info) {
    if ($line.Contains("Monitor") -And (-Not ($line.Contains("Mixed")))) {
        $ind_arr.Add($index) | Out-Null
    }
    $index++
}

# Parsing each line of monitor info for id, refresh rate, resolution
foreach ($ind_match in $ind_arr) {
    $mon_id = 0
    $mon_ref = 0
    $mon_width = 0
    $mon_height = 0

    # Matching for monitor id
    $found_monitor_id = $monitor_info[$ind_match - 1] -match '\[(.)\]'
    if ($found_monitor_id) {
        $mon_id = $Matches[1]
    }
    # Matching for monitor refresh rate
    $found_monitor_ref = $monitor_info[$ind_match + 1] -match '@(.*)Hz'
    if ($found_monitor_ref) {
        $mon_ref = $Matches[1]
    }
    # Matching for monitor resolution
    $found_monitor_res = $monitor_info[$ind_match + 1] -match"(....)x(....)"
    if ($found_monitor_res) {
        $mon_width = $Matches[1] -replace '\s',''
        $mon_height = $Matches[2] -replace '\s',''
    }

    # Tuple of (id, width, height, refresh rate) appended to $monitor_info_arr
    $mon_tuple = [System.Tuple]::Create($mon_id, $mon_width, $mon_height, $mon_ref)
    $monitor_info_arr.Add($mon_tuple) | Out-Null
}

# Creating commands for each connected monitor
foreach ($mon_info in $monitor_info_arr) {
    $id = $mon_info.Item1
    $width = $mon_info.Item2
    $height = $mon_info.Item3
    $refresh = $mon_info.Item4

    # Checking for vertical monitor
    if ([int]$height -gt [int]$width) {
        # Flipping min height and min width
        $temp = $min_width
        $min_width = $min_height
        $min_height = $temp
    }
    # Checking that monitor is greater than 1920x1080 60hz before making command
    if (([int]$width -gt $min_width) -And ([int]$height -gt $min_height)) {
        if ([int]$refresh -gt $min_refesh) { # Refresh rate greater than minimum
            $set_VR_res_cmd_arr.Add(".\ChangeScreenResolution.exe /w=$min_width /h=$min_height /d=$id /f=$min_refesh") | Out-Null
            $reset_res_cmd_arr.Add(".\ChangeScreenResolution.exe /w=$width /h=$height /d=$id /f=$refresh") | Out-Null
        } else { # Refresh rate below or equal to minimum
            $set_VR_res_cmd_arr.Add(".\ChangeScreenResolution.exe /w=$min_width /h=$min_height /d=$id") | Out-Null
            $reset_res_cmd_arr.Add(".\ChangeScreenResolution.exe /w=$width /h=$height /d=$id") | Out-Null
        }
    # Checking if monitor is high refresh rate 1080p panel
    } elseif (([int]$width -eq $min_width) -And ([int]$height -eq $min_height) -And ([int]$refresh -gt $min_refesh)) {
        $set_VR_res_cmd_arr.Add(".\ChangeScreenResolution.exe /d=$id /f=$min_refresh") | Out-Null
        $reset_res_cmd_arr.Add(".\ChangeScreenResolution.exe /d=$id /f=$refresh") | Out-Null
    }
}

# Setting all relavent screens to 1920x1080 60hz
foreach ($set_cmd in $set_VR_res_cmd_arr) {
    Invoke-Expression $set_cmd
}
Start-Sleep -s 5

# Starting Mixed Reality Portal and waiting 5 seconds before changing resolution
explorer.exe shell:AppsFolder\$(get-appxpackage -name Microsoft.MixedReality.Portal | Select-Object -expandproperty PackageFamilyName)!App
Start-Sleep -s 5

# Waiting for WMR to exit
$id = Get-Process MixedRealityPortal
$id.WaitForExit()

# Setting all relavent screens back to original settings
foreach ($reset_cmd in $reset_res_cmd_arr) {
    Invoke-Expression $reset_cmd
}
