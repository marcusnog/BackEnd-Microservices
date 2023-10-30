#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

ARG PAT
FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base

ENV Logging__Console__FormatterName=Simple

WORKDIR /app
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY . .

ARG PAT="bssye37hfu5k2vouzaejolrf32ivtbagsljud6uhldqqrsfu3qxa"
RUN echo "<?xml version='1.0' encoding='utf-8'?><configuration><packageSources><clear /><add key='Reconhece' value='https://pkgs.dev.azure.com/digidevs/1c19ed26-f31d-428b-89e9-541db3d12dab/_packaging/Reconhece/nuget/v3/index.json' /></packageSources><packageSourceCredentials><Reconhece><add key='Username' value='andre.araujo' /><add key='ClearTextPassword' value='$PAT' /></Reconhece></packageSourceCredentials></configuration>" > NuGet.Config

RUN dotnet restore "MsProductIntegrationNetShoes.csproj"
RUN dotnet build "MsProductIntegrationNetShoes.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "MsProductIntegrationNetShoes.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .


ENTRYPOINT ["dotnet", "MsProductIntegrationNetShoes.dll"]