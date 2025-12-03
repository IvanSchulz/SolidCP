Import-Module WebAdministration

cd ..
$path = pwd
cd Tools

New-Website -Name "FuseCP Server" -Port 9003 -PhysicalPath "$path\Sources\FuseCP.Server" -ErrorAction SilentlyContinue
New-Website -Name "FuseCP Enterprise Server" -Port 9002 -PhysicalPath "$path\Sources\FuseCP.EnterpriseServer" -ErrorAction SilentlyContinue
New-Website -Name "FuseCP Portal" -Port 9001 -PhysicalPath "$path\Sources\FuseCP.WebPortal" -ErrorAction SilentlyContinue
	
	
	
