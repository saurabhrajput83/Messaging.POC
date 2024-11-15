
SET REVIEWS_PROCESSOR_PROJ="Reviews.Processor.csproj"
SET NUGET_PATH="C:\Nuget\nuget.exe"
SET MSBUILD_PATH="C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe"
SET REVIEWS_PROCESSOR_PUB_DIR="D:\Source\Builds\Reviews.API"

cd Reviews
%NUGET_PATH% restore "Reviews.sln"
cd Reviews.Processor
%MSBUILD_PATH% %REVIEWS_PROCESSOR_PROJ% -t:Build /p:Configuration=Debug
cd ..
cd ..


