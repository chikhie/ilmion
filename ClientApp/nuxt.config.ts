// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: '2024-11-01',
  devtools: { enabled: true },

  modules: [
    '@nuxtjs/tailwindcss',
    '@pinia/nuxt'
  ],

  css: [
    '~/assets/css/main.css'
  ],

  // Configuration pour SSG (génération statique)
  ssr: true, // SSR enabled for SEO

  // Configuration de l'API
  runtimeConfig: {
    public: {
      // Utilise la variable d'environnement NUXT_PUBLIC_API_BASE si définie, sinon valeur par défaut
      apiBase: process.env.NUXT_PUBLIC_API_BASE || 'http://localhost:8080/api'
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
        { name: 'viewport', content: 'width=device-width, initial-scale=1' },
        { name: 'description', content: 'Plateforme éducative pour apprendre l\'histoire islamique et l\'arabe' }
      ],
      link: [
        { rel: 'icon', type: 'image/x-icon', href: '/favicon.ico' },
        { rel: 'preconnect', href: 'https://fonts.googleapis.com' },
        { rel: 'preconnect', href: 'https://fonts.gstatic.com', crossorigin: '' },
        { rel: 'stylesheet', href: 'https://fonts.googleapis.com/css2?family=Merriweather:wght@400;700&family=Libre+Baskerville:wght@400;700&family=Inter:wght@400;600&family=Source+Sans+3:wght@400;600&family=Amiri:wght@400;700&display=swap' }
      ]
    }
  },

  vite: {
    server: {
      fs: {
        strict: false
      }
    }
  }
})

