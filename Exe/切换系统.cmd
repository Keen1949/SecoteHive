@echo off
:select
cls
echo 请选择你需要切换的系统
echo 0 - 退出
echo 1 - x86
echo 2 - x64
set /p i=请输入数字:
if %i%==0 goto end
if %i%==1 goto x86
if %i%==2 goto x64
goto select

:x86
copy .\dll\x86\*.dll /y
goto end

:x64
copy .\dll\x64\*.dll /y
goto end

:end
