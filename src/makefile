MY_VAR := $(shell echo %date:~6,4%%date:~3,2%%date:~0,2%%time:~1,1%%time:~3,2%%time:~6,2%)

list:
	@echo set_hour
	@echo install
	@echo generate_um
	@echo generate_mini

install:
	dotnet restore
	dotnet build Burgr/Burgr.csproj
	dotnet pack --version-suffix local$(MY_VAR) -o nuget/ Burgr/Burgr.csproj
	dotnet tool install --add-source nuget/ --create-manifest-if-needed --version 1.0.0-local$(MY_VAR) --allow-downgrade Burgr

generate_um:
	dotnet burgr "Samples/2. User management API/specs"
	
generate_mini:
	dotnet burgr "Samples/3. Mini API/specs" /templateSpecDirectory:Templates/Mini/templates
