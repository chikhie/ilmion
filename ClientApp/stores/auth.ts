import { defineStore } from 'pinia'
import type { LoginRequest, RegisterRequest, AuthResponse } from '~/types'

export const useAuthStore = defineStore('auth', {
  state: () => ({
    user: null as any | null,
    token: null as string | null,
    refreshToken: null as string | null, // Added refreshToken to state
    loading: false,
  }),

  getters: {
    getUser: (state) => state.user,
    isLoggedIn: (state) => !!state.token,
    isPremium: (state) => !!state.user?.isPremium,
  },

  actions: {
    // Initialiser l'authentification depuis les cookies
    initAuth() {
      const tokenCookie = useCookie('auth_token')
      const refreshTokenCookie = useCookie('auth_refresh_token') // Load refresh token
      const userCookie = useCookie('auth_user')

      if (tokenCookie.value) {
        this.token = tokenCookie.value
      }

      if (refreshTokenCookie.value) {
        this.refreshToken = refreshTokenCookie.value // Set refresh token from cookie
      }

      if (userCookie.value) {
        this.user = userCookie.value
      } else if (this.token) {
        // If we have token but no user cookie (e.g. cleared), try to fetch
        this.fetchUserProfile()
      }
    },

    // Connexion
    async login(credentials: LoginRequest) {
      this.loading = true
      const config = useRuntimeConfig()

      try {
        const response = await $fetch<AuthResponse>(`${config.public.apiBase}/auth/login`, {
          method: 'POST',
          body: credentials
        })

        this.setSession(response)

        return { success: true }

      } catch (error: any) {
        console.error('Login error:', error)
        return {
          success: false,
          status: error.status,
          message: error.data?.message || 'Erreur de connexion.'
        }
      } finally {
        this.loading = false
      }
    },

    // Inscription
    async register(data: RegisterRequest) {
      this.loading = true
      const config = useRuntimeConfig()

      try {
        await $fetch(`${config.public.apiBase}/auth/register`, {
          method: 'POST',
          body: data
        })

        return {
          success: true,
          message: 'Inscription réussie ! Vérifiez votre email pour confirmer.'
        }
      } catch (error: any) {
        console.error('Register error:', error)
        return {
          success: false,
          message: error.data?.message || 'Erreur lors de l\'inscription.'
        }
      } finally {
        this.loading = false
      }
    },

    // Mot de passe oublié
    async forgotPassword(email: string) {
      this.loading = true
      const config = useRuntimeConfig()

      try {
        await $fetch(`${config.public.apiBase}/auth/forgot-password`, {
          method: 'POST',
          body: { email }
        })

        return { success: true }
      } catch (error: any) {
        console.error('Forgot password error:', error)
        return {
          success: false,
          message: error.data?.message || "Erreur lors de l'envoi de l'email."
        }
      } finally {
        this.loading = false
      }
    },

    // Réinitialisation du mot de passe
    async resetPassword(password: string, token: string, email: string) {
      this.loading = true
      const config = useRuntimeConfig()

      try {
        await $fetch(`${config.public.apiBase}/auth/reset-password`, {
          method: 'POST',
          body: { password, token, email }
        })

        return { success: true }
      } catch (error: any) {
        console.error('Reset password error:', error)
        return {
          success: false,
          message: error.data?.message || "Erreur lors de la réinitialisation."
        }
      } finally {
        this.loading = false
      }
    },

    // Déconnexion
    async logout() {
      const config = useRuntimeConfig()

      try {
        if (this.token) {
          await $fetch(`${config.public.apiBase}/auth/logout`, {
            method: 'POST',
            headers: { Authorization: `Bearer ${this.token}` }
          })
        }
      } catch (e) {
        console.warn("Logout endpoint failed", e)
      }

      this.clearSession()

      const router = useRouter()
      router.push('/login')
    },

    // Headers pour l'API Client
    getAuthHeaders() {
      if (this.token) {
        return { Authorization: `Bearer ${this.token}` }
      }
      return {}
    },

    setSession(authData: AuthResponse) {
      this.token = authData.token
      this.refreshToken = authData.refreshToken // Update state

      // Store in cookies
      const tokenCookie = useCookie('auth_token', { maxAge: 60 * 60 * 24 * 7 }) // 7 days
      tokenCookie.value = authData.token

      const refreshTokenCookie = useCookie('auth_refresh_token', { maxAge: 60 * 60 * 24 * 7 })
      refreshTokenCookie.value = authData.refreshToken

      // Decode user from token or fetch profile
      // Simple decoding for now if claims are present, otherwise might need profile fetch
      // For now we assume we fetch profile separately or rely on init
      if (authData.token) {
        this.fetchUserProfile()
      }
    },

    clearSession() {
      this.user = null
      this.token = null
      this.refreshToken = null // Clear state

      const tokenCookie = useCookie('auth_token')
      tokenCookie.value = null

      const refreshTokenCookie = useCookie('auth_refresh_token')
      refreshTokenCookie.value = null

      const userCookie = useCookie('auth_user')
      userCookie.value = null
    },

    async fetchUserProfile() {
      const config = useRuntimeConfig()

      if (!this.token) return

      try {
        const userProfile = await $fetch(`${config.public.apiBase}/User/me`, {
          headers: { Authorization: `Bearer ${this.token}` }
        })

        if (userProfile) {
          this.user = userProfile
          // Update cookie in case of refresh
          const userCookie = useCookie<any>('auth_user', { maxAge: 60 * 60 * 24 * 7 })
          userCookie.value = userProfile
        }
      } catch (e) {
        console.error('Error fetching user profile:', e)
        // Optionally clear session if token is invalid, but let's be safe for now
      }
    },

    async refreshAccessToken() {
      const refreshTokenCookie = useCookie('auth_refresh_token')
      const token = refreshTokenCookie.value
      const config = useRuntimeConfig()

      if (!token) return false

      try {
        const response = await $fetch<AuthResponse>(`${config.public.apiBase}/auth/refresh`, {
          method: 'POST',
          body: { refreshToken: token }
        })

        this.setSession(response)
        return true
      } catch (e) {
        this.clearSession()
        return false
      }
    }
  },
})
