set "base=%cd%"
set "workspaceroot=%base%\%1"

set filenameAppSettingsJson=appsettings.json
set targetframework=netcoreapp2.1

REM set "efcorepackagename=Microsoft.EntityFrameworkCore.SqlServer"
REM set "efdesignpackagename=Microsoft.EntityFrameworkCore.Design"

set "efcorepackagename=Microsoft.AspNetCore.App"
set "efcorepackageversion=2.1.1"

echo %base%
echo %workspaceroot%


mkdir %workspaceroot%
mkdir %workspaceroot%\src
mkdir %workspaceroot%\src\%1.Api
mkdir %workspaceroot%\src\%1.WebUi

mkdir %workspaceroot%\test
mkdir %workspaceroot%\test\%1.Tests

cd %workspaceroot%\src\%1.Api

dotnet new classlib -f %targetframework%

echo { >> %filenameAppSettingsJson%
echo    "ConnectionStrings": { >> %filenameAppSettingsJson%
echo      "default": "Server=(local); Database=%1; Trusted_Connection=True;" >> %filenameAppSettingsJson%
echo    } >> %filenameAppSettingsJson%
echo } >> %filenameAppSettingsJson%

echo CodeRush template commands: dbc, dbs, dbcf  >> demo-notes.txt
echo Workaround for framework error from ef core migrations--> Add to csproj file >> demo-notes.txt
echo ^<PropertyGroup^> >> demo-notes.txt
echo  ^<GenerateRuntimeConfigurationFiles^>true^</GenerateRuntimeConfigurationFiles^> >> demo-notes.txt
echo ^</PropertyGroup^> >> demo-notes.txt

cd %workspaceroot%\src\%1.WebUi

dotnet new mvc

cd %workspaceroot%\test\%1.Tests

dotnet new mstest

cd %workspaceroot%

dotnet new sln

dotnet sln .\%1.sln add .\src\%1.Api\%1.Api.csproj

dotnet sln .\%1.sln add .\src\%1.WebUi\%1.WebUi.csproj

dotnet sln .\%1.sln add .\test\%1.Tests\%1.Tests.csproj

cd %workspaceroot%\src\%1.WebUi

dotnet add reference ..\%1.Api\%1.Api.csproj

cd %workspaceroot%\test\%1.Tests

dotnet add reference ..\..\src\%1.Api\%1.Api.csproj
dotnet add reference ..\..\src\%1.WebUi\%1.WebUi.csproj

cd %workspaceroot%

REM dotnet remove %workspaceroot%\src\%1.WebUi\%1.WebUi.csproj package %efcorepackagename%
REM dotnet add %workspaceroot%\src\%1.WebUi\%1.WebUi.csproj package -v %efcorepackageversion% %efcorepackagename%

dotnet add %workspaceroot%\src\%1.Api\%1.Api.csproj package -v %efcorepackageversion% %efcorepackagename%
REM dotnet add %workspaceroot%\src\%1.Api\%1.Api.csproj package %efdesignpackagename%

dotnet add %workspaceroot%\test\%1.Tests\%1.Tests.csproj package -v %efcorepackageversion%  %efcorepackagename%


dotnet restore

dotnet build


