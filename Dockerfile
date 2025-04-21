# Usa la imagen base de .NET 8 para el contenedor de producción
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Usa la imagen de SDK de .NET 8 para el contenedor de compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia el archivo del proyecto del back-end (.server)
COPY ["MiAppCRUD.Server/MiAppCRUD.Server.csproj", "MiAppCRUD.Server/"]
RUN dotnet restore "MiAppCRUD.Server/MiAppCRUD.Server.csproj"

# Copia el archivo del proyecto del front-end (.client)
COPY ["miappcrud.client/miappcrud.client.csproj", "miappcrud.client/"]
RUN dotnet restore "miappcrud.client/miappcrud.client.csproj"

# Copia todo el código fuente del back-end y front-end
COPY . .

WORKDIR "/src/MiAppCRUD.Server"
RUN dotnet build "MiAppCRUD.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MiAppCRUD.Server.csproj" -c Release -o /app/publish

# Usa la imagen base para la fase final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Configura el contenedor para iniciar la aplicación del back-end
ENTRYPOINT ["dotnet", "MiAppCRUD.Server.dll"]


