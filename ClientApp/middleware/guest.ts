export default defineNuxtRouteMiddleware((to, from) => {
  const authStore = useAuthStore()

  // Si l'utilisateur est déjà connecté, rediriger vers la page d'accueil
  if (authStore.isAuthenticated) {
    return navigateTo('/')
  }
})

