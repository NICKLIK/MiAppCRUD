# Fase de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Instala Node.js (solo si tu frontend necesita build)
RUN apt-get update && \
    apt-get install -y curl && \
    curl -fsSL https://deb.nodesource.com/setup_16.x | bash - && \
    apt-get install -y nodejs

WORKDIR /src

# 1. Backend - Restaura dependencias
COPY ["MiAppCRUD.Server/MiAppCRUD.Server.csproj", "MiAppCRUD.Server/"]
RUN dotnet restore "MiAppCRUD.Server/MiAppCRUD.Server.csproj"

# 2. Frontend - Instala dependencias (si es necesario)
COPY ["miappcrud.client/package.json", "miappcrud.client/package-lock.json", "miappcrud.client/"]
WORKDIR "/src/miappcrud.client"
RUN npm install

# Copia todo el código
WORKDIR "/src"
COPY . .

# Construye el backend
WORKDIR "/src/MiAppCRUD.Server"
RUN dotnet publish "MiAppCRUD.Server.csproj" -c Release -o /app/publish

# Fase final
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Solo si tienes frontend React:
COPY --from=build /src/miappcrud.client/dist ./wwwroot

EXPOSE 80
ENTRYPOINT ["dotnet", "MiAppCRUD.Server.dll"]

