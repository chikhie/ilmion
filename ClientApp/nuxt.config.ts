// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: '2024-11-01',
  devtools: { enabled: true },

  modules: ['@nuxtjs/tailwindcss', '@pinia/nuxt', '@vite-pwa/nuxt', '@nuxtjs/i18n', '@nuxtjs/sitemap', '@nuxtjs/google-fonts'],

  googleFonts: {
    families: {
      'Merriweather': [300, 400, 700, 900],
      'Libre+Baskerville': [400, 700],
      'Inter': [100, 300, 400, 500, 700, 900],
      'Source+Sans+3': [300, 400, 600, 700],
      'Amiri': [400, 700],
    },
    display: 'swap',
    prefetch: true,
    preconnect: true,
    download: false,
  },

  site: {
    url: 'https://ilmion.site'
  },

  i18n: {
    locales: [
      { code: 'fr', name: 'Français', file: 'fr.json' },
      { code: 'en', name: 'English', file: 'en.json' }
    ],
    defaultLocale: 'fr',
    langDir: 'locales',
    strategy: 'prefix_except_default',
    detectBrowserLanguage: {
      useCookie: true,
      cookieKey: 'i18n_redirected',
      redirectOn: 'root'
    }
  },

  pwa: {
    registerType: 'autoUpdate',
    manifest: {
      name: 'Ilmion',
      short_name: 'Ilmion',
      description: "Plateforme éducative pour apprendre les sciences et les maths",
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
      globPatterns: ['**/*.{js,css,html,png,svg,ico}'],
      maximumFileSizeToCacheInBytes: 5 * 1024 * 1024,
    },
    client: {
      installPrompt: true,
    },
    devOptions: {
      enabled: true,
      suppressWarnings: true,
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
      apiBase: 'http://localhost:8080/api',
      posthogPublicKey: 'phc_EkRPpU3y5ViAqfURfPclVavjS5XfSS1p8KucKANbQp6',
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
      title: 'ILMION',
      meta: [
        { charset: 'utf-8' },
        { name: 'viewport', content: 'width=device-width, initial-scale=1, viewport-fit=cover' },
        { name: 'description', content: 'Plateforme éducative pour apprendre l\'histoire islamique et l\'arabe' },
        { name: 'theme-color', content: '#0F172A' },
        { name: 'apple-mobile-web-app-status-bar-style', content: 'black-translucent' }
      ],
      link: [
        { rel: 'icon', type: 'image/x-icon', href: '/favicon.ico' },
        { rel: 'manifest', href: '/manifest.webmanifest' },
        { rel: 'stylesheet', href: 'https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.1/css/all.min.css', integrity: 'sha512-DTOQO9RWCH3ppGqcWaEA1BIZOC6xxalwEsw9c2QQeAIftl+Vegovlnee1c9QX4TctnWMn13TZye+giMm8e2LwA==', crossorigin: 'anonymous', referrerpolicy: 'no-referrer', media: 'print', onload: "this.media='all'" }
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