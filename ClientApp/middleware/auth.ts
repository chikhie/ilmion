export default defineNuxtRouteMiddleware((to, from) => {
  const user = useSupabaseUser()
  const authStore = useAuthStore()
})

