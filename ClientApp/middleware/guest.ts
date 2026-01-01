export default defineNuxtRouteMiddleware((to, from) => {
  const user = useSupabaseUser()

  // Si l'utilisateur est déjà connecté, rediriger vers les jeux
  if (user.value) {
    return navigateTo('/games')
  }
})
