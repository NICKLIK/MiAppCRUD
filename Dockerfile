FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY ["MiAppCRUD.Server/MiAppCRUD.Server.csproj", "MiAppCRUD.Server/"]
RUN dotnet restore "MiAppCRUD.Server/MiAppCRUD.Server.csproj"

COPY . .
WORKDIR "/src/MiAppCRUD.Server"
RUN dotnet build "MiAppCRUD.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MiAppCRUD.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MiAppCRUD.Server.dll"]
