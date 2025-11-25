#!/bin/bash

# Script de démarrage Docker pour KitabStock
# Ce script facilite le déploiement de l'application

set -e

echo "🚀 Démarrage de KitabStock avec Docker..."

# Vérifier que Docker est installé
if ! command -v docker &> /dev/null; then
    echo "❌ Docker n'est pas installé. Veuillez l'installer d'abord."
    exit 1
fi

# Vérifier que Docker Compose est installé
if ! command -v docker-compose &> /dev/null; then
    echo "❌ Docker Compose n'est pas installé. Veuillez l'installer d'abord."
    exit 1
fi

# Fonction pour afficher l'aide
show_help() {
    echo "Usage: ./docker-start.sh [OPTION]"
    echo ""
    echo "Options:"
    echo "  start       Démarrer tous les services"
    echo "  stop        Arrêter tous les services"
    echo "  restart     Redémarrer tous les services"
    echo "  build       Rebuild tous les services"
    echo "  logs        Afficher les logs de tous les services"
    echo "  migrate     Appliquer les migrations de base de données"
    echo "  clean       Arrêter et supprimer tous les conteneurs et volumes"
    echo "  status      Afficher le statut des services"
    echo "  help        Afficher ce message d'aide"
    echo ""
}

# Fonction pour démarrer les services
start_services() {
    echo "📦 Démarrage des services..."
    docker-compose up -d --build
    
    echo ""
    echo "⏳ Attente que les services soient prêts..."
    sleep 10
    
    echo ""
    echo "✅ Services démarrés avec succès !"
    echo ""
    echo "📊 Statut des services :"
    docker-compose ps
    echo ""
    echo "🌐 URLs d'accès :"
    echo "   Frontend : http://localhost:3000"
    echo "   Backend  : http://localhost:5000"
    echo "   Swagger  : http://localhost:5000/swagger"
    echo ""
    echo "📝 Pour voir les logs : docker-compose logs -f"
    echo "📝 Pour appliquer les migrations : ./docker-start.sh migrate"
}

# Fonction pour arrêter les services
stop_services() {
    echo "🛑 Arrêt des services..."
    docker-compose down
    echo "✅ Services arrêtés avec succès !"
}

# Fonction pour redémarrer les services
restart_services() {
    echo "🔄 Redémarrage des services..."
    docker-compose restart
    echo "✅ Services redémarrés avec succès !"
}

# Fonction pour rebuild les services
build_services() {
    echo "🔨 Build des services..."
    docker-compose build --no-cache
    echo "✅ Build terminé avec succès !"
    echo "💡 Utilisez './docker-start.sh start' pour démarrer les services."
}

# Fonction pour afficher les logs
show_logs() {
    echo "📋 Affichage des logs (Ctrl+C pour quitter)..."
    docker-compose logs -f
}

# Fonction pour appliquer les migrations
apply_migrations() {
    echo "🗄️  Application des migrations de base de données..."
    
    # Vérifier que le conteneur backend est en cours d'exécution
    if ! docker-compose ps backend | grep -q "Up"; then
        echo "❌ Le conteneur backend n'est pas en cours d'exécution."
        echo "💡 Démarrez d'abord les services avec : ./docker-start.sh start"
        exit 1
    fi
    
    # Appliquer les migrations
    docker-compose exec backend dotnet ef database update
    
    echo "✅ Migrations appliquées avec succès !"
}

# Fonction pour nettoyer tout
clean_all() {
    echo "⚠️  Cette action va supprimer tous les conteneurs, volumes et données !"
    read -p "Êtes-vous sûr ? (y/N) " -n 1 -r
    echo
    if [[ $REPLY =~ ^[Yy]$ ]]; then
        echo "🧹 Nettoyage en cours..."
        docker-compose down -v
        echo "✅ Nettoyage terminé !"
    else
        echo "❌ Nettoyage annulé."
    fi
}

# Fonction pour afficher le statut
show_status() {
    echo "📊 Statut des services :"
    docker-compose ps
    echo ""
    echo "💾 Volumes :"
    docker volume ls | grep kitabstock || echo "Aucun volume trouvé"
    echo ""
    echo "🌐 Réseaux :"
    docker network ls | grep kitabstock || echo "Aucun réseau trouvé"
}

# Traiter les arguments
case "${1:-help}" in
    start)
        start_services
        ;;
    stop)
        stop_services
        ;;
    restart)
        restart_services
        ;;
    build)
        build_services
        ;;
    logs)
        show_logs
        ;;
    migrate)
        apply_migrations
        ;;
    clean)
        clean_all
        ;;
    status)
        show_status
        ;;
    help|*)
        show_help
        ;;
esac

