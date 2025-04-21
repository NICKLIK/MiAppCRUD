# Usar la imagen base de .NET 8.0
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Usar la imagen SDK de .NET 8.0 para la fase de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar el archivo .csproj y restaurar las dependencias
COPY ["MiAppCRUD.Server/MiAppCRUD.Server.csproj", "MiAppCRUD.Server/"]
RUN dotnet restore "MiAppCRUD.Server/MiAppCRUD.Server.csproj"

# Copiar el resto del código y construir el proyecto
COPY . . 
WORKDIR "/src/MiAppCRUD.Server"
RUN dotnet build "MiAppCRUD.Server.csproj" -c Release -o /app/build

# Fase de publicación
FROM build AS publish
RUN dotnet publish "MiAppCRUD.Server.csproj" -c Release -o /app/publish

# Fase final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish . 
ENTRYPOINT ["dotnet", "MiAppCRUD.Server.dll"]
