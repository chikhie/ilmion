import type { User, AuthResponse } from '~/types'

export const useAuth = () => {
  const token = useCookie('auth_token', { maxAge: 60 * 60 * 24 * 7 }) // 7 days
  const user = useState<User | null>('auth_user', () => null)
  const config = useRuntimeConfig()
  const apiBase = config.public.apiBase

  const login = async (email: string, password: string) => {
    try {
      const response = await $fetch<AuthResponse>(`${apiBase}/auth/login`, {
        method: 'POST',
        body: { email, password }
      })
      
      token.value = response.token
      // On pourrait décoder le token ici pour avoir les infos user, ou faire un appel /user/me
      // Pour l'instant on stocke juste le token.
      
      return true
    } catch (error) {
      console.error('Login error:', error)
      return false
    }
  }

  const logout = () => {
    token.value = null
    user.value = null
    navigateTo('/login')
  }

  return {
    token,
    user,
    login,
    logout,
    isAuthenticated: computed(() => !!token.value)
  }
}

