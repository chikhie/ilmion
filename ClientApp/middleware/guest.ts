export default defineNuxtRouteMiddleware((to, from) => {
  const authStore = useAuthStore()

  // Si l'utilisateur est déjà connecté, rediriger vers les jeux
  if (authStore.user) {
    return navigateTo('/games')
  }
})
