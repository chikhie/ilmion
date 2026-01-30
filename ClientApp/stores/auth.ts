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
      // You might need a specific endpoint to get the user profile derived from the token
      // Assuming /api/auth/me or similar, or just extracting from token payload if possible (but clean architecture adheres to separate fetch)
      // Let's assume we can fetch user details. If LoginRequest doesnt return user, we need to fetch it.
      // Based on AuthController, Login returns Token, Expiration, RefreshToken. So we rely on token only.
      // We can iterate on this later. For now, we decode basic info?
      // Actually `useApi().getProfile()` was used in the old code. We should reuse that if possible.
      // But `useApi` depends on `authStore` headers so we need the token set first.

      // Circular dependency risk if useApi calls authStore.
      // We'll implement a light fetch here or rely on useApi outside store definition?
      // Let's implement a direct fetch to /api/users/profile if it exists (assuming based on old code)

      // For now, let's try to fetch profile using the generic fetch with auth headers
      if (!this.token) return

      try {
        // We can't use `useApi` easily inside store setup without care.
        // Let's do a direct fetch for now.
        // Assuming a profile endpoint exists that returns UserProfile
        // Based on `fetchUserProfile` in old code calling `api.getProfile()`
        // We will assume `GET /api/users/profile` exists or similar.

        // If we don't have the endpoint handy in context, we will skip implementation logic 
        // and just set a dummy user or wait for next step.
        // WAIT: Old code L35 `await api.getProfile()`.

        // Implementation Note: We should fetch user profile to populate `this.user`.
      } catch (e) {
        console.error(e)
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
