FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY ["DigiWACS-Server/DigiWACS-Server.csproj", "DigiWACS-Server/"]
RUN dotnet restore "DigiWACS-Server/DigiWACS-Server.csproj"
COPY . .
WORKDIR "/src/DigiWACS-Server"
RUN dotnet build "DigiWACS-Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DigiWACS-Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DigiWACS-Server.dll"]
