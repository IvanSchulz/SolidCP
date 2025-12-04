#
# tool to fix Development WebConfigs after merge
# you MUST set $devroot to the base of your source tree... script will tell you if you've not set it right
#
$devroot = "C:\Rich"
#
if (-not (Get-Item "$devroot\FuseCP\Sources\FuseCP.WebPortal\Web.config") ) {
    $wshell = New-Object -ComObject Wscript.Shell
    $wshell.Popup("devroot is invalid",0,"Done",0x1) 
    exit
}


#Base (working) Install
[xml]$SolidPortal = Get-Content -Path "C:\FuseCP\Portal\Web.config"
[xml]$SolidEnterprise = Get-Content -Path "C:\FuseCP\Enterprise Server\Web.config"
[xml]$SolidServer = Get-Content -Path "C:\FuseCP\Server\Web.config"

#Dev (post merge) files
[xml]$devPortal = Get-Content -Path "$devroot\FuseCP\Sources\FuseCP.WebPortal\Web.config"
[xml]$devEnterprise = Get-Content -Path "$devroot\FuseCP\Sources\FuseCP.EnterpriseServer\Web.config"
[xml]$devServer = Get-Content -Path "$devroot\FuseCP\Sources\FuseCP.Server\Web.config"

# Portal Web Config - session validation key
$SolidPortal.configuration.appSettings.add | ForEach { if ( $_.key -eq "SessionValidationKey" ) {$hold = $_.value } }
$devPortal.configuration.appSettings.add | ForEach { if ( $_.key -eq "SessionValidationKey" ) { $_.value = $hold } }

# Enterprise WebConfig - connection string and crypto key
$devEnterprise.configuration.connectionStrings.add.connectionString = $SolidEnterprise.configuration.connectionStrings.add.connectionString
$SolidEnterprise.configuration.appSettings.add | ForEach { if ( $_.key -eq "FuseCP.CryptoKey" ) {$hold = $_.value } }
$devEnterprise.configuration.appSettings.add | ForEach { if ( $_.key -eq "FuseCP.CryptoKey" ) { $_.value = $hold } }

# Server WebConfig - server password
$devServer.configuration.'FuseCP.server'.security.password.value = $SolidServer.configuration.'FuseCP.server'.security.password.value

# backup existing files w/ timestamp just in case of overwrite, so they can be restored if something really goes wrong
$t = get-date -format "yyyyMMddhhmss"
Rename-Item "$devroot\FuseCP\Sources\FuseCP.WebPortal\Web.config"        "$devroot\FuseCP\Sources\FuseCP.WebPortal\Web.config-Git-$t"
Rename-Item "$devroot\FuseCP\Sources\FuseCP.EnterpriseServer\Web.config" "$devroot\FuseCP\Sources\FuseCP.EnterpriseServer\Web.config-Git-$t"
Rename-Item "$devroot\FuseCP\Sources\FuseCP.Server\Web.config"           "$devroot\FuseCP\Sources\FuseCP.Server\Web.config-Git-$t"

# write out new configs
$devPortal.save("$devroot\FuseCP\Sources\FuseCP.WebPortal\Web.config")
$devEnterprise.save("$devroot\FuseCP\Sources\FuseCP.EnterpriseServer\Web.config")
$devServer.Save("$devroot\FuseCP\Sources\FuseCP.Server\Web.config")
