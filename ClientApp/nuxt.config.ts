// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: '2024-11-01',
  devtools: { enabled: true },

  modules: [
    '@nuxtjs/tailwindcss',
    '@pinia/nuxt',
    '@vite-pwa/nuxt',
    '@nuxtjs/i18n'
  ],

  i18n: {
    locales: [
      { code: 'fr', name: 'Français', file: 'fr.json' },
      { code: 'en', name: 'English', file: 'en.json' }
    ],
    defaultLocale: 'fr',
    langDir: 'locales',
    strategy: 'no_prefix',
    detectBrowserLanguage: {
      useCookie: true,
      cookieKey: 'i18n_redirected',
      redirectOn: 'root'
    }
  },

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
      maximumFileSizeToCacheInBytes: 5 * 1024 * 1024,
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
      posthogPublicKey: 'phc_D5CDrdjgn0dBxpXiMAfnIMjAV4wy1SxapFz8A6tKeqh',
      posthogHost: 'https://eu.i.posthog.com',
      posthogDefaults: '2025-11-30'
    }
  },

  // Configuration du build
  nitro: {
    routeRules: {
      '/ingest/static/**': { proxy: 'https://eu-assets.i.posthog.com/static/**', headers: { 'cache-control': 'public, max-age=3600, stale-while-revalidate=86400' } },
      '/ingest/array/**': { proxy: 'https://eu-assets.i.posthog.com/array/**', headers: { 'cache-control': 'public, max-age=3600, stale-while-revalidate=86400' } },
      '/ingest/**': { proxy: 'https://eu.i.posthog.com/**', headers: { 'cache-control': 'public, max-age=3600, stale-while-revalidate=86400' } },
      '/_nuxt/**': { headers: { 'cache-control': 'public, max-age=31536000, immutable' } },
      '/**/*.png': { headers: { 'cache-control': 'public, max-age=31536000, immutable' } },
      '/**/*.svg': { headers: { 'cache-control': 'public, max-age=31536000, immutable' } },
      '/**/*.js': { headers: { 'cache-control': 'public, max-age=31536000, immutable' } },
      '/**/*.css': { headers: { 'cache-control': 'public, max-age=31536000, immutable' } }
    },
    // output: {
    //   publicDir: '../wwwroot'
    // }
  },

  app: {
    head: {
      htmlAttrs: {
        lang: 'fr'
      },
      title: 'ILMANAR',
      meta: [
        { charset: 'utf-8' },
        { name: 'viewport', content: 'width=device-width, initial-scale=1, viewport-fit=cover' },
        { name: 'description', content: 'Plateforme éducative pour apprendre l\'histoire islamique et l\'arabe' },
        { name: 'theme-color', content: '#0F172A' },
        { name: 'apple-mobile-web-app-status-bar-style', content: 'black-translucent' }
      ],
      link: [
        { rel: 'icon', type: 'image/x-icon', href: '/favicon.ico' },
        { rel: 'manifest', href: '/manifest.webmanifest' }
      ]
    }
  },

  vite: {
    server: {
      fs: {
        strict: false
      }
    },
    build: {
      target: 'esnext',
      rollupOptions: {
        output: {
          manualChunks(id) {
            if (id.includes('node_modules')) {
              if (id.includes('posthog-js')) {
                return 'posthog';
              }
            }
          }
        }
      }
    }
  },

})

