export default defineNuxtRouteMiddleware((to) => {
  const auth = useAuth()

  // Si l'utilisateur n'est pas authentifié et essaie d'accéder à une page autre que login
  if (!auth.isAuthenticated.value && to.path !== '/login') {
    return navigateTo('/login')
  }

  // Si l'utilisateur est authentifié et essaie d'accéder à la page de login
  if (auth.isAuthenticated.value && to.path === '/login') {
    return navigateTo('/')
  }
})

