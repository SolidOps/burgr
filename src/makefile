MY_VAR := $(shell echo %date:~6,4%%date:~3,2%%date:~0,2%%time:~1,1%%time:~3,2%%time:~6,2%)

list:
	@echo set_hour
	@echo install
	@echo generate_um
	@echo generate_todo

install:
	dotnet restore
	dotnet build Burgr/Burgr.csproj
	dotnet pack --version-suffix local$(MY_VAR) -o nuget/ Burgr/Burgr.csproj
	dotnet tool install --add-source nuget/ --create-manifest-if-needed --version 1.0.0-local$(MY_VAR) --allow-downgrade Burgr

generate_um:
	dotnet burgr "Samples/2. User Management API/um_specs"
	
generate_todo:
	dotnet burgr "Samples/1. TODO List/todo_specs"
