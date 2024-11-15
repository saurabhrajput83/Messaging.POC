cd Reviews
"C:\Nuget\nuget.exe" restore "Reviews.sln"
cd Reviews.API
msbuild.exe "Reviews.API.csproj" -t:Build /p:Configuration=Debug
# msbuild "Reviews.API.csproj" /p:DeployOnBuild=true /p:PublishProfile=FolderProfile
msbuild "Reviews.API.csproj" /p:DeployOnBuild=true /p:WebPublishMethod="FileSystem" /p:PublishUrl="D:\Source\Builds\Reviews.API" 
cd ..
cd Reviews.Admin.API
# msbuild.exe "Reviews.Admin.API.csproj" -t:Build /p:Configuration=Release
cd ..
cd ..