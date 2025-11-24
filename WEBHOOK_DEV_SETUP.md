# 🔧 Configuration Webhook Stripe en Développement

## ⚡ Solution rapide : Stripe CLI

### 1. Installation

**Windows** :
```bash
# Avec Scoop
scoop bucket add stripe https://github.com/stripe/scoop-stripe-cli.git
scoop install stripe

# OU télécharger directement
# https://github.com/stripe/stripe-cli/releases/latest
```

**Vérification** :
```bash
stripe --version
```

### 2. Connexion à votre compte Stripe

```bash
stripe login
```

Cela ouvrira votre navigateur pour vous connecter à Stripe.

### 3. Lancer le forwarding des webhooks

```bash
stripe listen --forward-to localhost:5050/api/StripeWebhook/webhook
```

**Vous verrez** :
```
> Ready! Your webhook signing secret is whsec_xxxxxxxxxxxxx
> (^C to quit)
```

### 4. Copier le webhook secret

Copiez le secret affiché et mettez-le dans `appsettings.json` :

```json
{
  "Stripe": {
    "WebhookSecret": "whsec_xxxxxxxxxxxxx"
  }
}
```

### 5. Tester un paiement

Dans un autre terminal :
```bash
# Simuler un checkout.session.completed
stripe trigger checkout.session.completed
```

Ou testez avec votre Payment Link : https://buy.stripe.com/test_7sY00kfmJeT9atS2qb0Fi00

---

## 📝 Workflow complet

**Terminal 1** : Backend
```bash
cd C:\Users\seydin\Desktop\MuslimAdsBack\KitabStock
dotnet run
```

**Terminal 2** : Stripe CLI
```bash
stripe listen --forward-to localhost:5050/api/StripeWebhook/webhook
```

**Terminal 3** : Frontend
```bash
cd ClientApp
npm run dev
```

---

## 🎯 Logs en temps réel

Le Stripe CLI affichera tous les webhooks reçus :

```
2024-01-15 10:23:45  --> checkout.session.completed [evt_xxx]
2024-01-15 10:23:45  <-- [200] POST http://localhost:5050/api/StripeWebhook/webhook
```

---

## 🔄 Alternative : ngrok (si vous voulez une URL publique)

### 1. Installation
```bash
choco install ngrok
# Ou télécharger : https://ngrok.com/download
```

### 2. Lancer le tunnel
```bash
ngrok http 5050
```

**Vous obtiendrez** :
```
Forwarding  https://abc123.ngrok.io -> http://localhost:5050
```

### 3. Configurer Stripe Dashboard

1. Allez sur : https://dashboard.stripe.com/test/webhooks
2. Cliquez "Add endpoint"
3. URL : `https://abc123.ngrok.io/api/StripeWebhook/webhook`
4. Événements : `checkout.session.completed`
5. Copiez le webhook secret dans `appsettings.json`

⚠️ **Attention** : L'URL ngrok change à chaque redémarrage (version gratuite)

---

## 🌐 Pour la production

Quand vous déployez votre API sur un serveur :

1. **URL du webhook** : `https://votre-domaine.com/api/StripeWebhook/webhook`
2. **Configurer dans Stripe Dashboard** (mode Live)
3. **Récupérer le webhook secret Live** (différent du mode test)
4. **Mettre à jour** `appsettings.json` en production

---

## 🐛 Troubleshooting

### Le webhook secret ne fonctionne pas
```bash
# Vérifiez que le secret dans appsettings.json correspond
# Redémarrez l'API après avoir changé le secret
```

### Le webhook n'arrive pas
```bash
# Vérifiez que Stripe CLI est bien lancé
# Vérifiez les logs du backend
# Testez manuellement :
stripe trigger checkout.session.completed
```

### Erreur 401/403 sur le webhook
```bash
# Le endpoint /webhook ne doit PAS avoir [Authorize]
# Vérifiez le StripeWebhookController.cs
```

---

## ✅ Checklist

- [ ] Stripe CLI installé
- [ ] `stripe login` effectué
- [ ] Webhook secret copié dans appsettings.json
- [ ] Backend redémarré
- [ ] `stripe listen --forward-to` lancé
- [ ] Test avec carte `4242 4242 4242 4242`
- [ ] Vérification des logs backend et Stripe CLI

**C'est prêt ! 🚀**

