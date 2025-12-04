@echo off
%systemroot%\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe FuseCP.VmConfig.exe
del FuseCP.VmConfig.InstallLog
del FuseCP.VmConfig.InstallState
del InstallUtil.InstallLog
