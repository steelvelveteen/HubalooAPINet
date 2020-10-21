# get# FROM mcr.microsoft.com/dotnet/core/runtime:3.1
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS base
WORKDIR /app
EXPOSE 80

WORKDIR /src
COPY HubalooAPINet.sln ./
COPY HubalooAPI/*.csproj ./HubalooAPI/
COPY HubalooAPI.BLL/*.csproj ./HubalooAPI.BLL/
COPY HubalooAPI.Dal/*.csproj ./HubalooAPI.Dal/
COPY HubalooAPI.Exceptions/*.csproj ./HubalooAPI.Exceptions/
COPY HubalooAPI.Interfaces/*.csproj ./HubalooAPI.Interfaces/
COPY HubalooAPI.Models/*.csproj ./HubalooAPI.Models/

RUN dotnet restore
COPY . .

# FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS build
WORKDIR /src/HubalooAPI
RUN dotnet build -c Release -o /app
WORKDIR /src/HubalooAPI.BLL
RUN dotnet build -c Release -o /app
WORKDIR /src/HubalooAPI.Dal
RUN dotnet build -c Release -o /app
WORKDIR /src/HubalooAPI.Exceptions
RUN dotnet build -c Release -o /app
WORKDIR /src/HubalooAPI.Interfaces
RUN dotnet build -c Release -o /app
WORKDIR /src/HubalooAPI.Models
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT [ "dotnet", "HubalooAPI.dll" ]