export default defineNuxtRouteMiddleware((to, from) => {
  const authStore = useAuthStore()

  // Si l'utilisateur n'est pas connecté, rediriger vers la page de connexion
  if (!authStore.isAuthenticated) {
    return navigateTo({
      path: '/login',
      query: { redirect: to.fullPath }
    })
  }
})

