// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: '2024-11-01',
  devtools: { enabled: true },
  modules: ['@nuxt/ui'],
  devServer: {
    port: 3001
  },
  runtimeConfig: {
    public: {
      apiBase: 'http://localhost:8080/api'
    }
  }
})
