@echo off
set "psCommand=powershell -Command "$pword = read-host '����������' -AsSecureString ; ^
	$BSTR=[System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($pword); ^
		[System.Runtime.InteropServices.Marshal]::PtrToStringAuto($BSTR)""
for /f "usebackq delims=" %%p in (`%psCommand%`) do set password=%%p

cls
set /p t=������ע������:
cls

STManger.exe %password% %t%
if %errorlevel% neq 0 goto fail

:success
echo. ע��ɹ�������
goto end

:fail
echo. ע��ʧ�ܣ�����

:end
pause