@echo off
RMDIR /S /Q "Bin"
IF not defined NoRebuild (
	FOR /F "tokens=*" %%G IN ('DIR /B /AD /S bin') DO RMDIR /S /Q "%%G"
	FOR /F "tokens=*" %%G IN ('DIR /B /AD /S obj') DO RMDIR /S /Q "%%G"
	FOR /F "tokens=*" %%G IN ('DIR /B /AD /S bin_dotnet') DO RMDIR /S /Q "%%G"
)

IF not defined MsBuildSwitches ( Set MsBuildSwitches=/v:n /m)
IF not defined SolidCPVersion ( Set SolidCPVersion=2.0.0)
IF not defined SolidCPFileVersion ( Set SolidCPFileVersion=2.0.0)
IF not defined Configuration ( Set Configuration=Release)

IF EXIST "%ProgramFiles%\Microsoft Visual Studio\18\Community\MSBuild\Current\bin\MSBuild.exe" (
	Set SCPMSBuild="%ProgramFiles%\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe"
	Set SCPVSVer=18.0
	Echo Found VS 2026 Community
	GOTO Build 
 )
IF EXIST "%ProgramFiles%\Microsoft Visual Studio\18\Professional\MSBuild\Current\bin\MSBuild.exe" (
	Set SCPMSBuild="%ProgramFiles%\Microsoft Visual Studio\18\Professional\MSBuild\Current\Bin\MSBuild.exe"
	Set SCPVSVer=18.0
	Echo Found VS 2026 Professional
	GOTO Build 
 )
IF EXIST "%ProgramFiles%\Microsoft Visual Studio\18\Enterprise\MSBuild\Current\bin\MSBuild.exe" (
	Set SCPMSBuild="%ProgramFiles%\Microsoft Visual Studio\18\Enterprise\MSBuild\Current\Bin\MSBuild.exe"
	Set SCPVSVer=18.0
	Echo Found VS 2026 Enterprise
	GOTO Build 
 )
IF EXIST "%ProgramFiles%\Microsoft Visual Studio\18\Preview\MSBuild\Current\Bin\MSBuild.exe" (
	Set SCPMSBuild="%ProgramFiles%\Microsoft Visual Studio\18\Preview\MSBuild\Current\Bin\MSBuild.exe"
	Set SCPVSVer=18.0
	Echo Found VS 2026 Preview
	GOTO Build 
 )
 IF EXIST "%ProgramFiles(x86)%\Microsoft Visual Studio\18\BuildTools\MSBuild\Current\Bin\MSBuild.exe" (
	Set SCPMSBuild="%ProgramFiles(x86)%\Microsoft Visual Studio\18\BuildTools\MSBuild\Current\Bin\MSBuild.exe"
	Set SCPVSVer=18.0
	Echo Found VS 2026 Build Tools
	GOTO Build 
 )

echo "VisualStudio 2026 not found. VS 2026 must be installed to build SolidCP."

Set SCPMSBuild="msbuild"

:Build
dotnet msbuild build.xml /target:Deploy /p:BuildConfiguration=%Configuration% /p:Version="%SolidCPVersion%" /p:FileVersion="%SolidCPFileVersion%" /p:VersionLabel="%SolidCPFileVersion%" %MsBuildSwitches% /fileLogger /flp:verbosity=normal /p:VisualStudioVersion=%SCPVSVer%
