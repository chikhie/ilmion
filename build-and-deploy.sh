#!/bin/bash

# Script de build et déploiement pour Ilmanar
# Usage: ./build-and-deploy.sh

set -e  # Exit on error

echo "🚀 Ilmanar - Build et Déploiement"
echo "=================================="
echo ""

# 1. Build du Frontend
echo "📦 Build du frontend Nuxt.js..."
cd ClientApp

if [ ! -d "node_modules" ]; then
    echo "   Installation des dépendances npm..."
    npm install
fi

echo "   Build de production..."
npm run build

# 2. Copier le build dans wwwroot
echo "   Copie du build dans wwwroot..."
cd ..
rm -rf wwwroot/*
cp -r ClientApp/.output/public/* wwwroot/

echo "✅ Frontend build terminé"
echo ""

# 3. Build du Backend
echo "📦 Build du backend ASP.NET..."
dotnet restore
dotnet build -c Release

echo "✅ Backend build terminé"
echo ""

# 4. Migrations de base de données
echo "📊 Application des migrations..."
dotnet ef database update

echo "✅ Migrations appliquées"
echo ""

# 5. Tests (optionnel)
# echo "🧪 Exécution des tests..."
# dotnet test

echo "=================================="
echo "✅ Build et déploiement terminés !"
echo ""
echo "Pour lancer l'application:"
echo "  dotnet run --project Ilmanar.csproj"
echo ""
echo "L'application sera disponible sur:"
echo "  - API: https://localhost:5000/swagger"
echo "  - Frontend: https://localhost:5000/"
echo ""

