export default defineNuxtRouteMiddleware((to, from) => {
  const authStore = useAuthStore()

  // Si l'utilisateur est déjà connecté, rediriger vers le dashboard
  if (authStore.isAuthenticated) {
    return navigateTo('/dashboard')
  }
})

