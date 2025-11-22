# Ilmanar - Plateforme Éducative

Une plateforme d'apprentissage moderne avec système de paiement intégré, développée avec ASP.NET Core et Nuxt.js.

## 🚀 Fonctionnalités

### Authentification & Profil Utilisateur
- ✅ Inscription et connexion avec JWT
- ✅ Confirmation d'email
- ✅ Refresh tokens
- ✅ Profil utilisateur complet (photo, nom, prénom, bio, date de naissance)
- ✅ Upload de photo de profil
- ✅ Changement de mot de passe
- ✅ Récupération de mot de passe (TODO)

### Contenu Éducatif
- ✅ **Matières (Subjects)** - Différents domaines d'étude (Math, Physique, etc.)
- ✅ **Modules** - Unités d'apprentissage avec vidéos
- ✅ **Sections** - Contenu détaillé (texte, interactif, quiz, exercices, résumés)
- ✅ Système de prix par module (gratuit ou payant)
- ✅ Vidéos HLS avec streaming adaptatif

### Système de Paiement
- ✅ Intégration Stripe Checkout
- ✅ Achat unique par module
- ✅ Gestion des webhooks Stripe
- ✅ Historique des achats
- ✅ Vérification d'accès au contenu
- ✅ Pages de succès/annulation de paiement

### Frontend Moderne
- ✅ Interface Nuxt.js avec Tailwind CSS
- ✅ Composants réutilisables
- ✅ Navigation intuitive
- ✅ Responsive design
- ✅ Gestion d'état avec Pinia

## 🛠️ Stack Technologique

### Backend
- **ASP.NET Core 9.0** - Framework web
- **Entity Framework Core** - ORM
- **SQLite** - Base de données
- **Identity** - Authentification et autorisation
- **JWT** - Tokens d'authentification
- **Stripe.NET** - Paiements
- **FFmpeg** - Traitement vidéo

### Frontend
- **Nuxt.js 3** - Framework Vue.js
- **TypeScript** - Typage statique
- **Tailwind CSS** - Styling
- **Pinia** - State management

## 📦 Installation

### Prérequis

- .NET SDK 9.0+
- Node.js 18+
- FFmpeg (pour le traitement vidéo)
- Compte Stripe (pour les paiements)

### 1. Backend (ASP.NET)

```bash
# Cloner le dépôt
git clone <repository-url>
cd KitabStock

# Restaurer les packages
dotnet restore

# Configurer la base de données
dotnet ef database update

# Lancer l'application
dotnet run
```

L'API sera disponible sur `https://localhost:5000`

### 2. Frontend (Nuxt.js)

```bash
cd ClientApp

# Installer les dépendances
npm install

# Mode développement
npm run dev

# Build pour production
npm run build
```

Le frontend sera disponible sur `http://localhost:3000`

## ⚙️ Configuration

### Backend - appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=db.sqlite"
  },
  "Jwt": {
    "Key": "VOTRE_CLE_SECRETE_JWT",
    "Issuer": "Ilmanar Api",
    "Audience": "IlmanarClient"
  },
  "Stripe": {
    "PublishableKey": "pk_test_...",
    "SecretKey": "sk_test_...",
    "WebhookSecret": "whsec_...",
    "SuccessUrl": "http://localhost:5000/payment/success",
    "CancelUrl": "http://localhost:5000/payment/cancel"
  },
  "Smtp": {
    "Host": "smtp.example.com",
    "Port": "587",
    "User": "votre@email.com",
    "Pass": "votre_mot_de_passe",
    "From": "noreply@ilmanar.com"
  }
}
```

### Frontend - nuxt.config.ts

```typescript
export default defineNuxtConfig({
  runtimeConfig: {
    public: {
      apiBase: 'https://localhost:5000/api'
    }
  }
})
```

## 🗂️ Structure du Projet

```
KitabStock/
├── Api/
│   ├── Controllers/          # Contrôleurs API
│   │   ├── AuthController.cs
│   │   ├── SubjectController.cs
│   │   ├── ModuleController.cs
│   │   ├── SectionController.cs
│   │   ├── PaymentController.cs
│   │   ├── UserController.cs
│   │   └── StripeWebhookController.cs
│   ├── Dtos/                 # Data Transfer Objects
│   └── Services/             # Services métier
├── Infra/
│   ├── Entities/             # Entités de base de données
│   │   ├── UserEntity.cs
│   │   ├── SubjectEntity.cs
│   │   ├── ModuleEntity.cs
│   │   ├── SectionEntity.cs
│   │   ├── VideoEntity.cs
│   │   └── ModulePurchaseEntity.cs
│   ├── repository/           # Repositories
│   ├── Services/             # Services d'infrastructure
│   │   ├── StripePaymentService.cs
│   │   ├── VideoProcessingService.cs
│   │   └── SmtpMailService.cs
│   └── DbContext.cs          # Configuration EF Core
├── ClientApp/                # Application Nuxt.js
│   ├── components/           # Composants Vue
│   │   ├── AppHeader.vue
│   │   ├── UserMenu.vue
│   │   ├── PaymentButton.vue
│   │   ├── SubjectCard.vue
│   │   ├── ModuleCard.vue
│   │   └── SectionViewer.vue
│   ├── pages/                # Pages de l'application
│   │   ├── index.vue
│   │   ├── login.vue
│   │   ├── register.vue
│   │   ├── profile.vue
│   │   ├── my-purchases.vue
│   │   ├── subjects/[id].vue
│   │   ├── modules/[id].vue
│   │   └── payment/
│   │       ├── success.vue
│   │       └── cancel.vue
│   ├── stores/               # Pinia stores
│   │   └── auth.ts
│   ├── composables/          # Composables Vue
│   │   └── useApi.ts
│   ├── middleware/           # Middlewares de navigation
│   │   ├── auth.ts
│   │   └── guest.ts
│   ├── types/                # Définitions TypeScript
│   └── plugins/              # Plugins Nuxt
└── Program.cs                # Point d'entrée ASP.NET
```

## 🔐 Sécurité

### Backend
- Authentification JWT avec refresh tokens
- Hachage des mots de passe avec Identity
- Validation des webhooks Stripe
- CORS configuré
- Protection CSRF

### Frontend
- Stockage sécurisé des tokens (localStorage)
- Rafraîchissement automatique des tokens
- Middlewares de protection des routes
- Validation des formulaires

## 🎨 Interface Utilisateur

### Pages Principales

1. **Page d'accueil** - Liste des matières disponibles
2. **Page matière** - Modules d'une matière avec prix
3. **Page module** - Détails et sections d'un module
4. **Profil utilisateur** - Gestion du compte
5. **Mes achats** - Historique des modules achetés
6. **Paiement** - Intégration Stripe Checkout

### Composants Clés

- **AppHeader** - Navigation principale avec menu utilisateur
- **UserMenu** - Menu déroulant du profil
- **PaymentButton** - Gestion des achats de modules
- **ModuleCard** - Carte d'affichage d'un module
- **SectionViewer** - Visualisation du contenu des sections

## 💳 Flux de Paiement

1. Utilisateur clique sur "Acheter le module"
2. Vérification de l'authentification
3. Création d'une session Stripe Checkout
4. Création d'un enregistrement `ModulePurchase` (statut: Pending)
5. Redirection vers Stripe
6. Utilisateur entre ses informations de carte
7. **Paiement réussi**:
   - Stripe envoie un webhook `checkout.session.completed`
   - Statut de l'achat passe à "Completed"
   - Redirection vers `/payment/success`
8. Utilisateur peut accéder au module

## 🧪 Tests

### Tests de Paiement (Mode Test)

Utilisez ces numéros de carte de test Stripe :

- **Succès**: `4242 4242 4242 4242`
- **Échec**: `4000 0000 0000 0002`
- Date: n'importe quelle date future
- CVC: n'importe quel 3 chiffres

### Stripe CLI

```bash
# Écouter les webhooks localement
stripe listen --forward-to https://localhost:5000/api/stripewebhook

# Déclencher un événement de test
stripe trigger checkout.session.completed
```

## 🚀 Déploiement

### Backend (ASP.NET)

```bash
# Build en mode production
dotnet publish -c Release -o ./publish

# Lancer l'application
cd publish
./Ilmanar
```

### Frontend (Nuxt.js)

```bash
# Build pour production
cd ClientApp
npm run build

# Copier le build dans wwwroot
cp -r .output/public ../wwwroot
```

L'application ASP.NET servira automatiquement le frontend depuis `wwwroot`.

## 📚 API Endpoints

### Authentification
- `POST /api/auth/register` - Inscription
- `POST /api/auth/login` - Connexion
- `POST /api/auth/refresh` - Rafraîchir le token
- `POST /api/auth/logout` - Déconnexion

### Utilisateur
- `GET /api/user/profile` - Récupérer le profil
- `PUT /api/user/profile` - Mettre à jour le profil
- `POST /api/user/profile/picture` - Upload photo de profil
- `DELETE /api/user/profile/picture` - Supprimer photo
- `POST /api/user/change-password` - Changer mot de passe

### Matières
- `GET /api/subject` - Liste des matières
- `GET /api/subject/{id}` - Détails d'une matière
- `POST /api/subject` - Créer une matière
- `PUT /api/subject/{id}` - Modifier une matière
- `DELETE /api/subject/{id}` - Supprimer une matière

### Modules
- `GET /api/module/subject/{subjectId}` - Modules d'une matière
- `GET /api/module/{id}` - Détails d'un module
- `POST /api/module` - Créer un module
- `PUT /api/module/{id}` - Modifier un module
- `DELETE /api/module/{id}` - Supprimer un module

### Sections
- `GET /api/section/module/{moduleId}` - Sections d'un module
- `GET /api/section/{id}` - Détails d'une section
- `POST /api/section` - Créer une section
- `PUT /api/section/{id}` - Modifier une section
- `DELETE /api/section/{id}` - Supprimer une section

### Paiements
- `POST /api/payment/create-checkout-session` - Créer session de paiement
- `GET /api/payment/status/{sessionId}` - Statut du paiement
- `GET /api/payment/my-purchases` - Historique des achats
- `GET /api/payment/has-access/{moduleId}` - Vérifier l'accès

### Webhooks
- `POST /api/stripewebhook` - Endpoint webhook Stripe

## 📖 Documentation Supplémentaire

- [Configuration Stripe](STRIPE_SETUP.md)
- [Swagger UI](https://localhost:5000/swagger) (en développement)

## 🤝 Contribution

Les contributions sont les bienvenues ! Veuillez suivre ces étapes :

1. Fork le projet
2. Créer une branche (`git checkout -b feature/AmazingFeature`)
3. Commit les changements (`git commit -m 'Add AmazingFeature'`)
4. Push vers la branche (`git push origin feature/AmazingFeature`)
5. Ouvrir une Pull Request

## 📝 License

Ce projet est sous licence MIT.

## 👥 Auteurs

- Votre nom - [@votre-username](https://github.com/votre-username)

## 🙏 Remerciements

- ASP.NET Core Team
- Nuxt.js Team
- Stripe
- Tous les contributeurs open source

---

**Ilmanar** - Apprenez, grandissez, réussissez 🚀

