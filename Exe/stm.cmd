@echo off
set "psCommand=powershell -Command "$pword = read-host '请输入密码' -AsSecureString ; ^
	$BSTR=[System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($pword); ^
		[System.Runtime.InteropServices.Marshal]::PtrToStringAuto($BSTR)""
for /f "usebackq delims=" %%p in (`%psCommand%`) do set password=%%p

cls
set /p t=请输入注册天数:
cls

STManger.exe %password% %t%
if %errorlevel% neq 0 goto fail

:success
echo. 注册成功！！！
goto end

:fail
echo. 注册失败！！！

:end
pause