# Script de démarrage Docker pour KitabStock (Windows PowerShell)
# Ce script facilite le déploiement de l'application

param(
    [Parameter(Position=0)]
    [ValidateSet('start', 'stop', 'restart', 'build', 'logs', 'migrate', 'clean', 'status', 'help')]
    [string]$Action = 'help'
)

$ErrorActionPreference = "Stop"

Write-Host "🚀 KitabStock Docker Manager" -ForegroundColor Cyan
Write-Host ""

# Vérifier que Docker est installé
function Test-Docker {
    try {
        docker --version | Out-Null
        return $true
    }
    catch {
        Write-Host "❌ Docker n'est pas installé ou n'est pas dans le PATH." -ForegroundColor Red
        Write-Host "Veuillez installer Docker Desktop : https://www.docker.com/products/docker-desktop" -ForegroundColor Yellow
        return $false
    }
}

# Vérifier que Docker Compose est installé
function Test-DockerCompose {
    try {
        docker-compose --version | Out-Null
        return $true
    }
    catch {
        Write-Host "❌ Docker Compose n'est pas installé." -ForegroundColor Red
        return $false
    }
}

# Afficher l'aide
function Show-Help {
    Write-Host "Usage: .\docker-start.ps1 [OPTION]" -ForegroundColor White
    Write-Host ""
    Write-Host "Options:" -ForegroundColor Yellow
    Write-Host "  start       Démarrer tous les services" -ForegroundColor Green
    Write-Host "  stop        Arrêter tous les services" -ForegroundColor Red
    Write-Host "  restart     Redémarrer tous les services" -ForegroundColor Blue
    Write-Host "  build       Rebuild tous les services" -ForegroundColor Magenta
    Write-Host "  logs        Afficher les logs de tous les services" -ForegroundColor Cyan
    Write-Host "  migrate     Appliquer les migrations de base de données" -ForegroundColor Yellow
    Write-Host "  clean       Arrêter et supprimer tous les conteneurs et volumes" -ForegroundColor DarkRed
    Write-Host "  status      Afficher le statut des services" -ForegroundColor White
    Write-Host "  help        Afficher ce message d'aide" -ForegroundColor Gray
    Write-Host ""
}

# Démarrer les services
function Start-Services {
    Write-Host "📦 Démarrage des services..." -ForegroundColor Cyan
    docker-compose up -d --build
    
    Write-Host ""
    Write-Host "⏳ Attente que les services soient prêts..." -ForegroundColor Yellow
    Start-Sleep -Seconds 10
    
    Write-Host ""
    Write-Host "✅ Services démarrés avec succès !" -ForegroundColor Green
    Write-Host ""
    Write-Host "📊 Statut des services :" -ForegroundColor Cyan
    docker-compose ps
    Write-Host ""
    Write-Host "🌐 URLs d'accès :" -ForegroundColor Cyan
    Write-Host "   Frontend : http://localhost:3000" -ForegroundColor White
    Write-Host "   Backend  : http://localhost:5000" -ForegroundColor White
    Write-Host "   Swagger  : http://localhost:5000/swagger" -ForegroundColor White
    Write-Host ""
    Write-Host "📝 Pour voir les logs : docker-compose logs -f" -ForegroundColor Gray
    Write-Host "📝 Pour appliquer les migrations : .\docker-start.ps1 migrate" -ForegroundColor Gray
}

# Arrêter les services
function Stop-Services {
    Write-Host "🛑 Arrêt des services..." -ForegroundColor Red
    docker-compose down
    Write-Host "✅ Services arrêtés avec succès !" -ForegroundColor Green
}

# Redémarrer les services
function Restart-Services {
    Write-Host "🔄 Redémarrage des services..." -ForegroundColor Blue
    docker-compose restart
    Write-Host "✅ Services redémarrés avec succès !" -ForegroundColor Green
}

# Rebuild les services
function Build-Services {
    Write-Host "🔨 Build des services..." -ForegroundColor Magenta
    docker-compose build --no-cache
    Write-Host "✅ Build terminé avec succès !" -ForegroundColor Green
    Write-Host "💡 Utilisez '.\docker-start.ps1 start' pour démarrer les services." -ForegroundColor Yellow
}

# Afficher les logs
function Show-Logs {
    Write-Host "📋 Affichage des logs (Ctrl+C pour quitter)..." -ForegroundColor Cyan
    docker-compose logs -f
}

# Appliquer les migrations
function Apply-Migrations {
    Write-Host "🗄️  Application des migrations de base de données..." -ForegroundColor Yellow
    
    # Vérifier que le conteneur backend est en cours d'exécution
    $backendStatus = docker-compose ps backend 2>$null
    if ($backendStatus -notmatch "Up") {
        Write-Host "❌ Le conteneur backend n'est pas en cours d'exécution." -ForegroundColor Red
        Write-Host "💡 Démarrez d'abord les services avec : .\docker-start.ps1 start" -ForegroundColor Yellow
        return
    }
    
    # Appliquer les migrations
    docker-compose exec backend dotnet ef database update
    
    Write-Host "✅ Migrations appliquées avec succès !" -ForegroundColor Green
}

# Nettoyer tout
function Clean-All {
    Write-Host "⚠️  Cette action va supprimer tous les conteneurs, volumes et données !" -ForegroundColor Red
    $confirmation = Read-Host "Êtes-vous sûr ? (y/N)"
    if ($confirmation -eq 'y' -or $confirmation -eq 'Y') {
        Write-Host "🧹 Nettoyage en cours..." -ForegroundColor Yellow
        docker-compose down -v
        Write-Host "✅ Nettoyage terminé !" -ForegroundColor Green
    }
    else {
        Write-Host "❌ Nettoyage annulé." -ForegroundColor Red
    }
}

# Afficher le statut
function Show-Status {
    Write-Host "📊 Statut des services :" -ForegroundColor Cyan
    docker-compose ps
    Write-Host ""
    Write-Host "💾 Volumes :" -ForegroundColor Cyan
    docker volume ls | Select-String "kitabstock"
    Write-Host ""
    Write-Host "🌐 Réseaux :" -ForegroundColor Cyan
    docker network ls | Select-String "kitabstock"
}

# Vérifier les prérequis
if (-not (Test-Docker)) {
    exit 1
}

if (-not (Test-DockerCompose)) {
    exit 1
}

# Exécuter l'action demandée
switch ($Action) {
    'start' { Start-Services }
    'stop' { Stop-Services }
    'restart' { Restart-Services }
    'build' { Build-Services }
    'logs' { Show-Logs }
    'migrate' { Apply-Migrations }
    'clean' { Clean-All }
    'status' { Show-Status }
    'help' { Show-Help }
    default { Show-Help }
}

