# Imagen base de ASP.NET para producción
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Imagen con SDK para compilar
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Instalar Node.js
RUN apt-get update && apt-get install -y curl \
    && curl -fsSL https://deb.nodesource.com/setup_20.x | bash - \
    && apt-get install -y nodejs

# Copiar el proyecto y restaurar
COPY ["MiAppCRUD.Server/MiAppCRUD.Server.csproj", "MiAppCRUD.Server/"]
COPY ["MiAppCRUD.Client/MiAppCRUD.Client.csproj", "MiAppCRUD.Client/"]
RUN dotnet restore "MiAppCRUD.Server/MiAppCRUD.Server.csproj"

# Copiar el resto del código
COPY . .

WORKDIR "/src/MiAppCRUD.Server"
RUN dotnet build "MiAppCRUD.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MiAppCRUD.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MiAppCRUD.Server.dll"]

