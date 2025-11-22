export default defineNuxtPlugin(() => {
  const authStore = useAuthStore()
  
  // Initialiser l'authentification depuis le localStorage
  authStore.initAuth()
})

