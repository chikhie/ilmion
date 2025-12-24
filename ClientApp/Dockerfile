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
FROM node:24.12.0
WORKDIR /app

# Copier le build et les fichiers nécessaires
COPY --from=build /app ./

# Installer uniquement les dépendances de production
RUN npm install --omit=dev

# Exposer le port Nuxt SSR (3000 par défaut)
EXPOSE 3000

# Lancer Nuxt SSR
CMD ["npm", "run", "start"]
