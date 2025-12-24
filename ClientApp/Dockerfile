# Stage 1: Build
FROM node:24.12.0 AS build
WORKDIR /app

# Copier les fichiers de dépendances
COPY package*.json ./
RUN npm install

# Copier tout le code source
COPY . .

# Build Nuxt pour SSR
RUN npm run build

# Stage 2: Production
FROM node:24.12.0-slim
WORKDIR /app

# Copier uniquement le dossier de sortie du build (.output)
COPY --from=build /app/.output ./.output

# Variables d'environnement pour Nuxt Nitro
ENV PORT=3000
ENV HOST=0.0.0.0

# Exposer le port
EXPOSE 3000

# Lancer le serveur Nuxt
CMD ["node", ".output/server/index.mjs"]
