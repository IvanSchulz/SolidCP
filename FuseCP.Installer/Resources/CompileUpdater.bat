@ECHO OFF
set basedir=
IF %1.==. GOTO NoArg
set basedir=%1
:NoArg
set sourcedir=%basedir%FuseCP.Updater\bin
set targetdir=%basedir%FuseCP.Installer

"%basedir%ILMerge.exe" "%sourcedir%\FuseCP.Updater.exe" "%sourcedir%\..\Lib\Ionic.Zip.Reduced.dll" /out:%targetdir%\Updater.exe
del %targetdir%\Updater.pdb 
