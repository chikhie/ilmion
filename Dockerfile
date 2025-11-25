# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copier le fichier de projet et restaurer les dépendances
COPY ["Ilmanar.csproj", "./"]
RUN dotnet restore "Ilmanar.csproj"

# Copier tous les fichiers source
COPY . .

# Build l'application
RUN dotnet build "Ilmanar.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "Ilmanar.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Installer les dépendances nécessaires pour PostgreSQL
RUN apt-get update && apt-get install -y \
    libpq5 \
    && rm -rf /var/lib/apt/lists/*

# Copier les fichiers publiés
COPY --from=publish /app/publish .

# Créer les dossiers nécessaires pour les fichiers statiques
RUN mkdir -p /app/wwwroot/uploads/profiles && \
    mkdir -p /app/wwwroot/protected/components && \
    mkdir -p /app/Infra/res/videos/videoTest

# Exposer le port
EXPOSE 8080

# Point d'entrée
ENTRYPOINT ["dotnet", "Ilmanar.dll"]

