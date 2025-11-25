# Guide de Déploiement Docker - KitabStock

Ce guide explique comment déployer l'application KitabStock avec Docker et Docker Compose.

## 📋 Prérequis

- Docker installé (version 20.10+)
- Docker Compose installé (version 2.0+)
- Au moins 2 GB de RAM disponible
- Ports disponibles : 3000 (Frontend), 5000 (Backend), 5432 (PostgreSQL)

## 🏗️ Architecture

L'application est composée de 3 services :

1. **PostgreSQL** : Base de données (port 5432)
2. **Backend** : API ASP.NET Core (port 5000)
3. **Frontend** : Application Nuxt.js (port 3000)

## 🚀 Démarrage Rapide

### 1. Configuration initiale

Copiez le fichier `.env.example` vers `.env` et configurez vos variables d'environnement :

```bash
cp .env.example .env
```

Puis éditez le fichier `.env` avec vos valeurs de production.

### 2. Migration vers PostgreSQL

Le code utilise actuellement SQLite. Pour utiliser PostgreSQL, modifiez le fichier `Program.cs` :

```csharp
// Remplacer cette ligne (ligne 23)
options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))

// Par
options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
```

N'oubliez pas d'ajouter l'import en haut du fichier :
```csharp
using Npgsql.EntityFrameworkCore.PostgreSQL;
```

### 3. Configuration du Frontend

Dans `ClientApp/nuxt.config.ts`, assurez-vous que l'API base URL est configurée :

```typescript
export default defineNuxtConfig({
  runtimeConfig: {
    public: {
      apiBase: process.env.NUXT_PUBLIC_API_BASE || 'http://localhost:5000'
    }
  }
})
```

### 4. Build et Démarrage

```bash
# Build et démarrer tous les services
docker-compose up -d --build

# Voir les logs
docker-compose logs -f

# Voir les logs d'un service spécifique
docker-compose logs -f backend
docker-compose logs -f frontend
docker-compose logs -f postgres
```

### 5. Appliquer les migrations de base de données

Une fois le backend démarré, appliquez les migrations :

```bash
# Entrer dans le conteneur backend
docker-compose exec backend bash

# Appliquer les migrations
dotnet ef database update

# Sortir du conteneur
exit
```

## 🔧 Commandes Utiles

### Gestion des services

```bash
# Arrêter tous les services
docker-compose down

# Arrêter et supprimer les volumes (⚠️ supprime les données)
docker-compose down -v

# Redémarrer un service spécifique
docker-compose restart backend

# Voir le statut des services
docker-compose ps

# Voir l'utilisation des ressources
docker stats
```

### Logs et débogage

```bash
# Logs en temps réel de tous les services
docker-compose logs -f

# Dernières 100 lignes de logs du backend
docker-compose logs --tail=100 backend

# Entrer dans un conteneur
docker-compose exec backend bash
docker-compose exec frontend sh
docker-compose exec postgres psql -U kitabstock_user -d kitabstock_db
```

### Build et mise à jour

```bash
# Rebuild un service spécifique
docker-compose build backend
docker-compose up -d backend

# Rebuild tous les services
docker-compose build --no-cache

# Pull les dernières images
docker-compose pull
```

## 🗄️ Gestion de la Base de Données

### Backup PostgreSQL

```bash
# Créer un backup
docker-compose exec postgres pg_dump -U kitabstock_user kitabstock_db > backup.sql

# Restaurer un backup
cat backup.sql | docker-compose exec -T postgres psql -U kitabstock_user -d kitabstock_db
```

### Accéder à PostgreSQL

```bash
# Via le client psql
docker-compose exec postgres psql -U kitabstock_user -d kitabstock_db

# Commandes psql utiles :
# \dt - Lister les tables
# \d table_name - Décrire une table
# \q - Quitter
```

## 📊 Volumes Persistants

Les données suivantes sont persistées dans des volumes Docker :

- `postgres_data` : Données PostgreSQL
- `backend_uploads` : Photos de profil et fichiers uploadés
- `backend_protected` : Composants chiffrés
- `backend_videos` : Fichiers vidéo

Pour sauvegarder ces volumes :

```bash
# Lister les volumes
docker volume ls

# Inspecter un volume
docker volume inspect kitabstock_postgres_data
```

## 🌐 URLs d'Accès

Une fois déployé :

- **Frontend** : http://localhost:3000
- **Backend API** : http://localhost:5000
- **Swagger API** : http://localhost:5000/swagger (uniquement en développement)
- **PostgreSQL** : localhost:5432

## 🔒 Sécurité en Production

Pour un déploiement en production, pensez à :

1. **Changer tous les mots de passe** dans le fichier `.env`
2. **Utiliser HTTPS** avec un reverse proxy (nginx, traefik)
3. **Ne pas exposer PostgreSQL** publiquement (retirer le mapping de port)
4. **Configurer les CORS** correctement dans `Program.cs`
5. **Utiliser des secrets Docker** pour les informations sensibles
6. **Activer les backups automatiques** de la base de données
7. **Limiter les ressources** des conteneurs

### Exemple avec reverse proxy nginx

```yaml
# Ajouter ce service dans docker-compose.yml
nginx:
  image: nginx:alpine
  ports:
    - "80:80"
    - "443:443"
  volumes:
    - ./nginx.conf:/etc/nginx/nginx.conf
    - ./ssl:/etc/nginx/ssl
  depends_on:
    - frontend
    - backend
  networks:
    - kitabstock-network
```

## 🐛 Dépannage

### Le backend ne démarre pas

```bash
# Vérifier les logs
docker-compose logs backend

# Vérifier que PostgreSQL est prêt
docker-compose exec postgres pg_isready -U kitabstock_user
```

### Le frontend ne se connecte pas au backend

- Vérifiez la variable `NUXT_PUBLIC_API_BASE` dans le docker-compose
- Vérifiez les CORS dans `Program.cs`
- Vérifiez les logs du navigateur (F12)

### Erreurs de migration de base de données

```bash
# Supprimer la base de données et recommencer
docker-compose down -v
docker-compose up -d postgres
# Attendre que PostgreSQL soit prêt
docker-compose up -d backend
docker-compose exec backend dotnet ef database update
```

### Performances lentes

```bash
# Vérifier l'utilisation des ressources
docker stats

# Allouer plus de mémoire à Docker Desktop
# Paramètres > Resources > Memory > Augmenter à 4GB+
```

## 📝 Développement Local

Pour développer en local sans Docker :

```bash
# Backend
dotnet restore
dotnet run

# Frontend
cd ClientApp
npm install
npm run dev
```

## 🔄 Mise à jour de l'application

```bash
# 1. Pull les derniers changements
git pull

# 2. Rebuild les conteneurs
docker-compose build

# 3. Redémarrer avec les nouveaux conteneurs
docker-compose up -d

# 4. Appliquer les nouvelles migrations si nécessaire
docker-compose exec backend dotnet ef database update
```

## 📞 Support

En cas de problème, vérifiez :
- Les logs : `docker-compose logs`
- La documentation officielle Docker
- Les issues GitHub du projet

