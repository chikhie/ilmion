# Configuration Stripe pour Ilmanar

Ce guide explique comment configurer les paiements Stripe pour la plateforme Ilmanar.

## Prérequis

1. Un compte Stripe (https://dashboard.stripe.com/register)
2. Les clés API Stripe (mode test pour le développement)

## Configuration Backend (ASP.NET)

### 1. Récupérer vos clés Stripe

Connectez-vous à votre dashboard Stripe et récupérez :
- **Publishable key** (pk_test_...)
- **Secret key** (sk_test_...)

### 2. Configurer appsettings.json

Mettez à jour le fichier `appsettings.json` avec vos clés :

```json
{
  "Stripe": {
    "PublishableKey": "pk_test_VOTRE_CLE_PUBLIQUE",
    "SecretKey": "sk_test_VOTRE_CLE_SECRETE",
    "WebhookSecret": "whsec_VOTRE_SECRET_WEBHOOK",
    "SuccessUrl": "http://localhost:5000/payment/success",
    "CancelUrl": "http://localhost:5000/payment/cancel"
  }
}
```

### 3. Créer une migration pour les achats

```bash
dotnet ef migrations add AddModulePurchases
dotnet ef database update
```

## Configuration du Webhook Stripe

Les webhooks permettent à Stripe de notifier votre application des événements de paiement.

### 1. Installation de Stripe CLI (pour le développement)

```bash
# Windows (avec Scoop)
scoop install stripe

# macOS (avec Homebrew)
brew install stripe/stripe-cli/stripe

# Linux
wget -qO- https://github.com/stripe/stripe-cli/releases/download/v1.19.4/stripe_1.19.4_linux_x86_64.tar.gz | tar -xz
```

### 2. Connecter Stripe CLI à votre compte

```bash
stripe login
```

### 3. Écouter les webhooks en local

```bash
stripe listen --forward-to https://localhost:5000/api/stripewebhook
```

Cette commande affichera un **webhook secret** (whsec_...). Copiez-le dans votre `appsettings.json`.

### 4. Tester un paiement

```bash
stripe trigger checkout.session.completed
```

## Configuration en Production

### 1. Configurer le webhook dans le dashboard Stripe

1. Allez sur https://dashboard.stripe.com/webhooks
2. Cliquez sur "Add endpoint"
3. URL du endpoint : `https://votre-domaine.com/api/stripewebhook`
4. Sélectionnez les événements à écouter :
   - `checkout.session.completed`
   - `checkout.session.expired`
   - `payment_intent.succeeded`
   - `payment_intent.payment_failed`
   - `charge.refunded`
5. Copiez le **Signing secret** (whsec_...) et ajoutez-le à votre configuration

### 2. Utiliser les clés de production

Remplacez les clés de test (pk_test_, sk_test_) par les clés de production (pk_live_, sk_live_) dans votre configuration.

**⚠️ Important : Ne commitez JAMAIS vos clés secrètes dans Git !**

Utilisez des variables d'environnement ou un service de gestion de secrets :

```bash
# Variables d'environnement
export Stripe__SecretKey="sk_live_..."
export Stripe__WebhookSecret="whsec_..."
```

Ou utilisez Azure Key Vault, AWS Secrets Manager, etc.

## Fonctionnalités Implémentées

### Backend

✅ **ModulePurchaseEntity** - Entité pour stocker les achats
✅ **ModulePurchaseRepo** - Repository pour gérer les achats
✅ **StripePaymentService** - Service de paiement Stripe
✅ **PaymentController** - API pour créer des sessions de paiement
✅ **StripeWebhookController** - Gestion des événements Stripe
✅ **Prix sur les modules** - Chaque module peut avoir un prix ou être gratuit

### Frontend (Nuxt.js)

✅ **PaymentButton.vue** - Composant de paiement
✅ **Intégration Stripe Checkout** - Redirection vers le formulaire de paiement Stripe
✅ **Pages de succès/annulation** - Gestion du retour après paiement
✅ **Historique des achats** - Page `/my-purchases`
✅ **Vérification d'accès** - Affichage conditionnel du contenu selon l'achat
✅ **Badges de prix** - Affichage des prix sur les cartes de modules

## Cartes de Test Stripe

Utilisez ces numéros de carte pour tester les paiements :

### Paiements réussis
- **4242 4242 4242 4242** - Visa
- **5555 5555 5555 4444** - Mastercard
- Date d'expiration : n'importe quelle date future
- CVC : n'importe quel 3 chiffres
- Code postal : n'importe quel code

### Paiements échoués
- **4000 0000 0000 0002** - Carte déclinée (insuffisance de fonds)
- **4000 0000 0000 9995** - Carte déclinée (expirée)

## API Routes

### Paiement

- `POST /api/payment/create-checkout-session` - Créer une session de paiement
- `GET /api/payment/status/{sessionId}` - Vérifier le statut d'un paiement
- `GET /api/payment/my-purchases` - Historique des achats de l'utilisateur
- `GET /api/payment/has-access/{moduleId}` - Vérifier l'accès à un module

### Webhook

- `POST /api/stripewebhook` - Endpoint pour recevoir les événements Stripe

## Workflow de Paiement

1. **Utilisateur clique sur "Acheter"**
   - Vérification de l'authentification
   - Création d'une session Stripe Checkout
   - Création d'un enregistrement `ModulePurchase` avec statut "Pending"

2. **Redirection vers Stripe**
   - L'utilisateur est redirigé vers le formulaire de paiement Stripe
   - Saisie des informations de carte

3. **Paiement réussi**
   - Stripe envoie un webhook `checkout.session.completed`
   - Le statut de l'achat passe à "Completed"
   - L'utilisateur est redirigé vers `/payment/success`

4. **Accès au module**
   - L'utilisateur peut maintenant accéder au contenu du module
   - Le module apparaît dans "Mes achats"

## Sécurité

- ✅ Vérification de la signature des webhooks
- ✅ Authentification JWT requise pour les achats
- ✅ Validation côté serveur du montant du paiement
- ✅ Protection contre les achats multiples du même module
- ✅ Logs des transactions

## Support et Dépannage

### Problème : Le webhook ne fonctionne pas

**Solution :** Vérifiez que :
1. Stripe CLI est en cours d'exécution (`stripe listen --forward-to...`)
2. Le webhook secret est correct dans `appsettings.json`
3. L'application ASP.NET est en cours d'exécution
4. Le pare-feu n'bloque pas les connexions

### Problème : Erreur "Invalid API Key"

**Solution :** Vérifiez que la clé secrète Stripe est correctement configurée dans `appsettings.json`

### Problème : Paiement réussi mais statut toujours "Pending"

**Solution :** Vérifiez les logs du webhook. Le webhook doit être correctement configuré et accessible.

## Documentation Stripe

- Guide Stripe Checkout : https://stripe.com/docs/payments/checkout
- Webhooks : https://stripe.com/docs/webhooks
- API Reference : https://stripe.com/docs/api

## Contact

Pour toute question technique, consultez la documentation Stripe ou contactez le support.

