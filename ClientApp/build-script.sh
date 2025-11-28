#!/bin/bash
# Script pour modifier la configuration pendant le build

# Remplacer l'URL de l'API dans nuxt.config.ts
if [ -n "$NUXT_PUBLIC_API_BASE" ]; then
  sed -i "s|apiBase:.*|apiBase: '${NUXT_PUBLIC_API_BASE}'|g" nuxt.config.ts
  echo "Configuration mise à jour avec: ${NUXT_PUBLIC_API_BASE}"
fi

