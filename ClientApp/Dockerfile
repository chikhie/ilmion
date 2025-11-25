# Stage 1: Build
FROM node:latest AS build
WORKDIR /app

# Arguments de build
ARG NUXT_PUBLIC_API_BASE=http://localhost:5051/api
ENV NUXT_PUBLIC_API_BASE=${NUXT_PUBLIC_API_BASE}

# Copier les fichiers de dépendances
COPY package*.json ./

# Debug: Vérifier que package.json est bien là
RUN ls -la

# Installer les dépendances
RUN npm install

# Copier tous les fichiers source
COPY . .

# Générer les fichiers statiques
RUN npm run generate

# Stage 2: Serve with Caddy
FROM caddy:latest
WORKDIR /usr/share/caddy

# Copier les fichiers générés depuis le stage de build
COPY --from=build /app/.output/public /usr/share/caddy

# Copier la configuration Caddy
# Note: Le contexte de build est ./ClientApp, donc Caddyfile est à la racine du contexte
COPY Caddyfile /etc/caddy/Caddyfile

# Exposer le port 80
EXPOSE 80
