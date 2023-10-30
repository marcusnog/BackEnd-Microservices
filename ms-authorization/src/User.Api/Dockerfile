FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine as build
ARG PAT
WORKDIR /app
COPY . .
RUN echo "<?xml version='1.0' encoding='utf-8'?><configuration><packageSources><clear /><add key='Reconhece' value='https://pkgs.dev.azure.com/digidevs/1c19ed26-f31d-428b-89e9-541db3d12dab/_packaging/Reconhece/nuget/v3/index.json' /></packageSources><packageSourceCredentials><Reconhece><add key='Username' value='andre.araujo' /><add key='ClearTextPassword' value='$PAT' /></Reconhece></packageSourceCredentials></configuration>" > NuGet.Config
RUN dotnet restore User.Api.csproj
RUN dotnet publish User.Api.csproj -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine as runtime
WORKDIR /app
COPY --from=build /app/publish /app
ENTRYPOINT ["dotnet", "/app/User.Api.dll"]