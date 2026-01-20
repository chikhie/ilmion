// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: '2024-11-01',
  devtools: { enabled: true },

  modules: [
    '@nuxtjs/tailwindcss',
    '@pinia/nuxt',
    '@vite-pwa/nuxt'
  ],

  pwa: {
    registerType: 'autoUpdate',
    manifest: {
      name: 'Ilmanar',
      short_name: 'Ilmanar',
      description: "Plateforme éducative pour apprendre l'histoire islamique et l'arabe",
      start_url: '/',
      display: 'standalone',
      background_color: '#0F172A',
      theme_color: '#0F172A',
      icons: [
        {
          src: '/pwa-192x192.png',
          sizes: '192x192',
          type: 'image/png',
          purpose: 'any maskable'
        },
        {
          src: '/pwa-512x512.png',
          sizes: '512x512',
          type: 'image/png',
          purpose: 'any maskable'
        }
      ],
    },
    workbox: {
      navigateFallback: '/',
      globPatterns: ['**/*.{js,css,html,png,svg,ico}'],
    },
    client: {
      installPrompt: true,
    },
    devOptions: {
      enabled: true,
      suppressWarnings: true,
      navigateFallback: '/',
      navigateFallbackAllowlist: [/^\/$/],
      type: 'module',
    },
  },

  css: [
    '~/assets/css/main.css'
  ],

  // Configuration pour SSG (génération statique)
  ssr: true, // SSR enabled for SEO

  // Configuration de l'API
  runtimeConfig: {
    public: {
      // Utilise la variable d'environnement NUXT_PUBLIC_API_BASE si définie, sinon valeur par défaut
      apiBase: process.env.NUXT_PUBLIC_API_BASE || 'http://localhost:8080/api',
    }
  },

  // Configuration du build
  // nitro: {
  //   output: {
  //     publicDir: '../wwwroot'
  //   }
  // },

  app: {
    head: {
      title: 'ILMANAR',
      meta: [
        { charset: 'utf-8' },
        { name: 'viewport', content: 'width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no, viewport-fit=cover' },
        { name: 'description', content: 'Plateforme éducative pour apprendre l\'histoire islamique et l\'arabe' },
        { name: 'theme-color', content: '#0F172A' },
        { name: 'apple-mobile-web-app-status-bar-style', content: 'black-translucent' }
      ],
      link: [
        { rel: 'icon', type: 'image/x-icon', href: '/favicon.ico' },
        { rel: 'preconnect', href: 'https://fonts.googleapis.com' },
        { rel: 'preconnect', href: 'https://fonts.gstatic.com', crossorigin: '' },
        { rel: 'stylesheet', href: 'https://fonts.googleapis.com/css2?family=Merriweather:wght@400;700&family=Libre+Baskerville:wght@400;700&family=Inter:wght@400;600&family=Source+Sans+3:wght@400;600&family=Amiri:wght@400;700&display=swap' },
        { rel: 'manifest', href: '/manifest.webmanifest' }
      ]
    }
  },

  vite: {
    server: {
      fs: {
        strict: false
      }
    }
  },

})

