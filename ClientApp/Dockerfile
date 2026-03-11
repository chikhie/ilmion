# Utilisation de l'image officielle Bun (https://hub.docker.com/r/oven/bun)
FROM oven/bun:latest as builder

# Définition du répertoire de travail
WORKDIR /app

# Copie du fichier package.json uniquement (on ignore le lockfile pour éviter l'erreur 400)
COPY package.json ./

# Installation des dépendances avec Bun
# (Le postinstall va lancer "nuxt prepare" tel que défini dans package.json)
RUN bun install

# Copie du reste du code source
COPY . .

# Remplacement de l'URL de l'API pour la production
RUN sed -i 's|http://localhost:8080/api|https://api.ilmion.site/api|g' nuxt.config.ts

# Construction de l'application Nuxt pour la production
RUN bun run build

# --- Étape de Production ---
FROM oven/bun:slim as runner

# Définition du répertoire de travail
WORKDIR /app

# Copie uniquement du dossier de production depuis l'étape précédente
COPY --from=builder /app/.output ./.output

# Variables d'environnement pour la production
ENV NODE_ENV=production
ENV HOST=0.0.0.0
ENV PORT=3000

# Exposition du port d'écoute
EXPOSE 3000

# Démarrage du serveur Nuxt avec Bun
CMD ["bun", "run", ".output/server/index.mjs"]
