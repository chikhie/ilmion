# Script de build et déploiement pour Ilmanar (Windows)
# Usage: .\build-and-deploy.ps1

$ErrorActionPreference = "Stop"

Write-Host "🚀 Ilmanar - Build et Déploiement" -ForegroundColor Cyan
Write-Host "==================================" -ForegroundColor Cyan
Write-Host ""

# 1. Build du Frontend
Write-Host "📦 Build du frontend Nuxt.js..." -ForegroundColor Yellow
Set-Location ClientApp

if (-not (Test-Path "node_modules")) {
    Write-Host "   Installation des dépendances npm..." -ForegroundColor Gray
    npm install
}

Write-Host "   Build de production..." -ForegroundColor Gray
npm run build

# 2. Copier le build dans wwwroot
Write-Host "   Copie du build dans wwwroot..." -ForegroundColor Gray
Set-Location ..

if (Test-Path "wwwroot") {
    Remove-Item -Path "wwwroot\*" -Recurse -Force
} else {
    New-Item -Path "wwwroot" -ItemType Directory
}

Copy-Item -Path "ClientApp\.output\public\*" -Destination "wwwroot\" -Recurse -Force

Write-Host "✅ Frontend build terminé" -ForegroundColor Green
Write-Host ""

# 3. Build du Backend
Write-Host "📦 Build du backend ASP.NET..." -ForegroundColor Yellow
dotnet restore
dotnet build -c Release

Write-Host "✅ Backend build terminé" -ForegroundColor Green
Write-Host ""

# 4. Migrations de base de données
Write-Host "📊 Application des migrations..." -ForegroundColor Yellow
dotnet ef database update

Write-Host "✅ Migrations appliquées" -ForegroundColor Green
Write-Host ""

# 5. Tests (optionnel)
# Write-Host "🧪 Exécution des tests..." -ForegroundColor Yellow
# dotnet test

Write-Host "==================================" -ForegroundColor Cyan
Write-Host "✅ Build et déploiement terminés !" -ForegroundColor Green
Write-Host ""
Write-Host "Pour lancer l'application:" -ForegroundColor Yellow
Write-Host "  dotnet run --project Ilmanar.csproj" -ForegroundColor Gray
Write-Host ""
Write-Host "L'application sera disponible sur:" -ForegroundColor Yellow
Write-Host "  - API: https://localhost:5000/swagger" -ForegroundColor Gray
Write-Host "  - Frontend: https://localhost:5000/" -ForegroundColor Gray
Write-Host ""

