@echo off
:select
cls
echo ��ѡ������Ҫ�л���ϵͳ
echo 0 - �˳�
echo 1 - x86
echo 2 - x64
set /p i=����������:
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
