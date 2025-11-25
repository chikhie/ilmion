// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: '2024-11-01',
  devtools: { enabled: true },
  
  modules: [
    '@nuxtjs/tailwindcss',
    '@pinia/nuxt'
  ],

  // Configuration pour SSG (génération statique)
  ssr: false, // SPA mode pour simplicité
  
  // Configuration de l'API
  runtimeConfig: {
    public: {
      apiBase:'https://ilmanar.ibrahim-chikhi.cv:8080/api'
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
        { name: 'description', content: 'Plateforme éducative pour apprendre les mathématiques, la physique, la chimie et la biologie' }
      ],
      link: [
        { rel: 'icon', type: 'image/x-icon', href: '/favicon.ico' }
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

