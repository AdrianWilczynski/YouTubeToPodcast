pushd ..\src\YouTubeToPodcast
libman restore
dotnet build
dotnet ef database update
popd