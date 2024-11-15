
SET REVIEWS_ADMIN_API_PROJ="Reviews\Reviews.Admin.API\Reviews.Admin.API.csproj"
SET NUGET_PATH="C:\Nuget\nuget.exe"
SET MSBUILD_PATH="C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe"
SET REVIEWS_ADMIN_API_PUB_DIR="..\..\build_output\Reviews.Admin.API"

%NUGET_PATH% restore "Reviews\Reviews.sln"
%MSBUILD_PATH% %REVIEWS_ADMIN_API_PROJ% /p:Configuration=Release /clp:ErrorsOnly
%MSBUILD_PATH% %REVIEWS_ADMIN_API_PROJ% /p:DeployOnBuild=True /p:Configuration=Release /p:DeployDefaultTarget=WebPublish /p:WebPublishMethod=FileSystem /p:DeleteExistingFiles=True /p:publishUrl=%REVIEWS_ADMIN_API_PUB_DIR%


