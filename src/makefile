MY_VAR := $(shell echo %date:~6,4%%date:~3,2%%date:~0,2%%time:~1,1%%time:~3,2%%time:~6,2%)

set_hour:
	set hour=%time:~0,2%
	if "%hour:~0,1%" == " " set hour=0%hour:~1,1%
	echo hour=%hour%
	set min=%time:~3,2%
	if "%min:~0,1%" == " " set min=0%min:~1,1%
	echo min=%min%
	set secs=%time:~6,2%
	if "%secs:~0,1%" == " " set secs=0%secs:~1,1%
	echo secs=%secs%
	set year=%date:~-4%
	echo year=%year%
	set month=%date:~4,2%
	if "%month:~0,1%" == " " set month=0%month:~1,1%
	echo month=%month%
	set day=%date:~0,2%
	if "%day:~0,1%" == " " set day=0%day:~1,1%
	echo day=%day%
	echo %year%%month%%day%_%hour%%min%%secs%

install:
	dotnet restore
	dotnet build Burgr/Burgr.csproj
	dotnet pack --version-suffix local$(MY_VAR) -o nuget/ Burgr/Burgr.csproj
	dotnet tool install --add-source nuget/ --create-manifest-if-needed --version 1.0.0-local$(MY_VAR) --allow-downgrade Burgr

generate_um:
	dotnet burgr "Samples/2. User Management API/um_specs"
	
generate_todo:
	dotnet burgr "Samples/1. TODO List/todo_specs"
