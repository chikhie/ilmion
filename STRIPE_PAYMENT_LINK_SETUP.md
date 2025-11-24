# 🎯 Configuration Payment Link Stripe

Ce guide explique comment configurer le Payment Link Stripe pour votre plateforme.

## ✅ Ce qui a été fait

### Backend (`Api/Controllers/StripeWebhookController.cs`)
- ✅ Webhook complet pour recevoir les événements Stripe
- ✅ Récupération de l'ID utilisateur via `metadata["user_id"]` ou `ClientReferenceId`
- ✅ Création automatique de l'abonnement dans la base de données
- ✅ Logs détaillés pour le debug

### Frontend (`ClientApp/pages/subscribe.vue`)
- ✅ Redirection vers le Payment Link avec les metadata
- ✅ Passage de l'ID utilisateur et de l'email
- ✅ Design cohérent avec le site

### Page de succès (`ClientApp/pages/payment/success.vue`)
- ✅ Design dark/gold amélioré
- ✅ Vérification de l'abonnement
- ✅ Affichage des informations de validité

---

## 🔧 Configuration requise dans Stripe Dashboard

### 1. **Configurer le Payment Link**

Accédez à votre Payment Link : https://buy.stripe.com/test_7sY00kfmJeT9atS2qb0Fi00

Dans Stripe Dashboard → **Payment Links** :

1. **Success URL** : `http://192.168.1.25:3000/payment/success` (ou votre URL de production)
2. **Cancel URL** : `http://192.168.1.25:3000/payment/cancel`

### 2. **Configurer le Webhook**

Dans Stripe Dashboard → **Developers** → **Webhooks** :

1. **Créer un endpoint** : `http://192.168.1.25:5050/api/StripeWebhook/webhook`
   (ou votre URL de production)

2. **Événements à écouter** :
   - ✅ `checkout.session.completed` (obligatoire)
   - ✅ `payment_intent.succeeded` (recommandé)
   - ✅ `payment_intent.payment_failed` (recommandé)

3. **Récupérer le Webhook Secret** : `whsec_xxxxx`

4. **Mettre à jour `appsettings.json`** :
```json
{
  "Stripe": {
    "WebhookSecret": "whsec_votre_secret_ici"
  }
}
```

### 3. **Configurer les Metadata dans le Payment Link**

⚠️ **IMPORTANT** : Pour que l'ID utilisateur soit transmis, vous devez :

**Option A : Via l'URL (Recommandé)**
Le frontend ajoute automatiquement : `?client_reference_id=USER_ID&prefilled_email=EMAIL`

**Option B : Dans Stripe Dashboard**
Dans les paramètres du Payment Link, activez "Collect metadata" et ajoutez :
- Clé : `user_id`
- Valeur : À remplir par l'URL

---

## 🧪 Test du flux de paiement

### 1. **Démarrer les serveurs**

**Backend** :
```bash
dotnet run
# Doit tourner sur http://localhost:5050
```

**Frontend** :
```bash
cd ClientApp
npm run dev
# Doit tourner sur http://localhost:3000 ou http://192.168.1.25:3000
```

### 2. **Tester le paiement**

1. Connectez-vous sur : `http://192.168.1.25:3000/login`
2. Accédez à : `http://192.168.1.25:3000/subscribe`
3. Cliquez sur "S'abonner pour 10€/an"
4. Vous serez redirigé vers Stripe avec votre email pré-rempli
5. Utilisez une carte de test :
   - **Numéro** : `4242 4242 4242 4242`
   - **Expiration** : N'importe quelle date future
   - **CVC** : N'importe quel 3 chiffres
6. Complétez le paiement
7. Vous serez redirigé vers `/payment/success`
8. Vérifiez que votre abonnement est actif dans `/dashboard`

### 3. **Vérifier le webhook**

Dans les logs du backend, vous devriez voir :
```
📥 Webhook Stripe reçu: checkout.session.completed
👤 User ID trouvé dans Metadata: [ID_UTILISATEUR]
✅ Abonnement créé avec succès pour l'utilisateur [ID_UTILISATEUR]
```

---

## 🐛 Troubleshooting

### Le webhook ne reçoit rien
- ✅ Vérifiez que l'URL du webhook est accessible depuis Internet
- ✅ Utilisez **ngrok** ou **localtunnel** en développement
- ✅ Vérifiez les logs Stripe Dashboard → Webhooks → Attempts

### L'ID utilisateur n'est pas trouvé
- ✅ Vérifiez les logs : `User ID trouvé dans Metadata`
- ✅ Vérifiez que l'utilisateur est bien connecté avant de payer
- ✅ Inspectez la session Stripe dans le Dashboard

### L'abonnement n'apparaît pas
- ✅ Attendez 2-3 secondes après le paiement
- ✅ Vérifiez les logs du backend
- ✅ Vérifiez la table `Subscriptions` dans la base de données

---

## 🌐 Configuration pour la production

### 1. **Mettre à jour les URLs**

**`appsettings.json`** :
```json
{
  "Stripe": {
    "PublishableKey": "pk_live_VOTRE_CLE",
    "SecretKey": "sk_live_VOTRE_CLE",
    "WebhookSecret": "whsec_VOTRE_SECRET",
    "SuccessUrl": "https://votre-domaine.com/payment/success",
    "CancelUrl": "https://votre-domaine.com/payment/cancel"
  },
  "FrontendUrl": "https://votre-domaine.com"
}
```

**`ClientApp/pages/subscribe.vue`** :
```typescript
// Ligne 197
const stripePaymentLink = 'https://buy.stripe.com/VOTRE_LIEN_LIVE'
```

### 2. **Créer un Payment Link en mode Live**

Dans Stripe Dashboard (mode Live) :
1. Créez un nouveau Product "Premium Annuel" à 10€
2. Créez un Payment Link pour ce produit
3. Configurez les URLs de succès et d'annulation
4. Récupérez le nouveau lien

### 3. **Configurer le webhook en production**

URL du webhook : `https://votre-domaine.com/api/StripeWebhook/webhook`

---

## 📊 Flux complet

```
[Utilisateur]
    ↓ Clique sur "S'abonner"
[Frontend] subscribe.vue
    ↓ Récupère user.id + email
    ↓ Redirige vers Payment Link Stripe
    ↓ Avec ?client_reference_id=USER_ID&prefilled_email=EMAIL
[Stripe]
    ↓ Utilisateur paie
    ↓ Stripe envoie webhook checkout.session.completed
[Backend] StripeWebhookController
    ↓ Récupère user_id depuis metadata
    ↓ Crée SubscriptionEntity
    ↓ Sauvegarde dans DB
[Stripe]
    ↓ Redirige vers /payment/success
[Frontend] success.vue
    ↓ Vérifie l'abonnement via API
    ↓ Affiche confirmation + date de fin
[Utilisateur]
    ✅ Abonnement actif !
```

---

## 🎯 Résumé des changements

| Fichier | Changement |
|---------|-----------|
| `StripeWebhookController.cs` | Webhook complet avec gestion metadata |
| `subscribe.vue` | Redirection Payment Link au lieu d'API |
| `success.vue` | Design amélioré dark/gold |
| `appsettings.json` | URLs de redirection configurées |

---

## ✅ Checklist finale

- [ ] Webhook Stripe configuré avec l'URL correcte
- [ ] Secret du webhook ajouté dans `appsettings.json`
- [ ] Payment Link avec URLs de succès/annulation
- [ ] Test avec carte `4242 4242 4242 4242`
- [ ] Vérification des logs backend
- [ ] Vérification de l'abonnement dans `/dashboard`
- [ ] Test de l'accès aux chapitres premium

**Bon courage ! 🚀**

