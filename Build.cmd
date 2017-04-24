@ECHO OFF
Set ProductName=LogViewer
Set SolutionName=LogViewer
Set Set BuildMessage=

IF EXIST "%VSDEVCMD%" Goto Build
IF EXIST "%MSBUILDPATH%" Goto Build

:VS2017Env
Echo %BuildMessage%
REM Due to bug(?) in VS 2017 not setting the VS150COMNTOOLS environment  
REM variable, try finding the tools directory path using brute force
Set ToolsDir=C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\Tools\
IF EXIST "%ToolsDir%" Set VS150COMNTOOLS=%ToolsDir%
IF EXIST "%ToolsDir%" Goto VS2017EnvPrep
Set ToolsDir=C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\Common7\Tools\
IF EXIST "%ToolsDir%" Set VS150COMNTOOLS=%ToolsDir%
IF EXIST "%ToolsDir%" Goto VS2017EnvPrep
Set ToolsDir=C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\Common7\Tools\
IF EXIST "%ToolsDir%" Set VS150COMNTOOLS=%ToolsDir%
IF EXIST "%ToolsDir%" Goto VS2017EnvPrep
Set ToolsDir=C:\Program Files (x86)\Microsoft Visual Studio\2017\BuildTools\Common7\Tools\
IF EXIST "%ToolsDir%" Set VS150COMNTOOLS=%ToolsDir%
IF EXIST "%ToolsDir%" Goto VS2017EnvPrep
Set ToolsDir=C:\Program Files\Microsoft Visual Studio\2017\Community\Common7\Tools\
IF EXIST "%ToolsDir%" Set VS150COMNTOOLS=%ToolsDir%
IF EXIST "%ToolsDir%" Goto VS2017EnvPrep
Set ToolsDir=C:\Program Files\Microsoft Visual Studio\2017\Professional\Common7\Tools\
IF EXIST "%ToolsDir%" Set VS150COMNTOOLS=%ToolsDir%
IF EXIST "%ToolsDir%" Goto VS2017EnvPrep
Set ToolsDir=C:\Program Files\Microsoft Visual Studio\2017\Enterprise\Common7\Tools\
IF EXIST "%ToolsDir%" Set VS150COMNTOOLS=%ToolsDir%
IF EXIST "%ToolsDir%" Goto VS2017EnvPrep
Set ToolsDir=C:\Program Files\Microsoft Visual Studio\2017\BuildTools\Common7\Tools\
IF EXIST "%ToolsDir%" Set VS150COMNTOOLS=%ToolsDir%
IF EXIST "%ToolsDir%" Goto VS2017EnvPrep

:VS2017EnvPrep
Set VSDEVCMD=%VS150COMNTOOLS%VsDevCmd.bat
Echo Checking to see if Visual Studio 2017 is installed (VSDEVCMD="%VSDEVCMD%")
IF NOT EXIST "%VSDEVCMD%" Set BuildMessage=Visual Studio 2017 do not seem to be installed. Terminating.
IF NOT EXIST "%VSDEVCMD%" Goto End
Echo Visual Studio 2017 is installed, preparing build environment...
call "%VSDEVCMD%"
Goto Build

:Build
Echo Preparing to build %ProductName%...
Echo Restoring nuget packages...
nuget.exe restore %SolutionName%.sln
Echo Finished restoring nuget packages.
Echo Building %ProductName% using MSBuild...
msbuild.exe %SolutionName%.build %1 %2 %3 %4 %5 %6 %7 %8 %9
Set BuildErrorLevel=%ERRORLEVEL%
IF %BuildErrorLevel%==0 Set BuildMessage=Sucessfully build %ProductName%
IF NOT %BuildErrorLevel% == 0 Set BuildMessage=Failed to build %ProductName%
Goto End

:End
Echo %BuildMessage%
