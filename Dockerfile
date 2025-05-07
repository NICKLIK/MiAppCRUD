# Fase de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Copia solo los archivos .csproj y restaura dependencias del backend
COPY ["MiAppCRUD.Server/MiAppCRUD.Server.csproj", "MiAppCRUD.Server/"]
RUN dotnet restore "MiAppCRUD.Server/MiAppCRUD.Server.csproj"

# 2. Copia el frontend React
COPY ["miappcrud.client/package.json", "miappcrud.client/package-lock.json", "miappcrud.client/"]
WORKDIR "/src/miappcrud.client"
RUN npm install

# 3. Copia todo el código fuente
COPY . .

# 4. Construye el backend
WORKDIR "/src/MiAppCRUD.Server"
RUN dotnet build "MiAppCRUD.Server.csproj" -c Release -o /app/build

# Fase de publicación
FROM build AS publish
RUN dotnet publish "MiAppCRUD.Server.csproj" -c Release -o /app/publish

# Construye el frontend
WORKDIR "/src/miappcrud.client"
RUN npm run build

# Fase final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=build /src/miappcrud.client/dist ./wwwroot

EXPOSE 80
ENTRYPOINT ["dotnet", "MiAppCRUD.Server.dll"]

