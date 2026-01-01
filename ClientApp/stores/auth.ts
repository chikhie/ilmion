import { defineStore } from 'pinia'
import type { LoginRequest, RegisterRequest } from '~/types'

export const useAuthStore = defineStore('auth', {
  state: () => ({
    user: null as any | null,
    session: null as any | null,
    loading: false,
  }),

  getters: {
    getUser: (state) => state.user,
    isLoggedIn: (state) => !!state.user,
    isPremium: (state) => !!state.user?.isPremium, // Assuming user object might have this, or default to false
  },

  actions: {
    // Initialiser l'authentification (managed by Supabase automatically, but useful for hydration)
    async initAuth() {
      const supabase = useSupabaseClient()
      const { data } = await supabase.auth.getSession()
      this.session = data.session
      this.user = data.session?.user || null

      // Listen to state changes
      supabase.auth.onAuthStateChange((event, session) => {
        this.session = session
        this.user = session?.user || null
      })
    },

    async fetchUserProfile() {
      const api = useApi()
      try {
        const profile = await api.getProfile()
        if (profile) {
          // Merge with existing user or set
          this.user = { ...this.user, ...profile }
        }
      } catch (e) {
        console.error("Failed to fetch user profile", e)
      }
    },

    // Connexion
    async login(credentials: LoginRequest) {
      this.loading = true
      const supabase = useSupabaseClient()
      try {
        // Supabase Login
        const { data, error } = await supabase.auth.signInWithPassword({
          email: credentials.email || '',
          password: credentials.password
        })

        if (error) throw error

        this.user = data.user
        this.session = data.session
        return { success: true }

      } catch (error: any) {
        console.error('Login error:', error)
        return {
          success: false,
          message: error.message || 'Erreur de connexion.'
        }
      } finally {
        this.loading = false
      }
    },

    // Inscription
    async register(data: RegisterRequest) {
      this.loading = true
      const supabase = useSupabaseClient()
      try {
        const { data: result, error } = await supabase.auth.signUp({
          email: data.email,
          password: data.password,
          options: {
            data: {
              username: data.username,
              full_name: data.username
            }
          }
        })

        if (error) throw error

        return {
          success: true,
          message: 'Inscription réussie ! Vérifiez votre email pour confirmer.'
        }
      } catch (error: any) {
        console.error('Register error:', error)
        return {
          success: false,
          message: error.message || 'Erreur lors de l\'inscription.'
        }
      } finally {
        this.loading = false
      }
    },

    // Mot de passe oublié
    async forgotPassword(email: string) {
      this.loading = true
      const supabase = useSupabaseClient()
      try {
        const { error } = await supabase.auth.resetPasswordForEmail(email, {
          redirectTo: `${window.location.origin}/reset-password`,
        })

        if (error) throw error

        return { success: true }
      } catch (error: any) {
        console.error('Forgot password error:', error)
        return {
          success: false,
          message: error.message || "Erreur lors de l'envoi de l'email."
        }
      } finally {
        this.loading = false
      }
    },

    // Réinitialisation du mot de passe
    async resetPassword(password: string) {
      this.loading = true
      const supabase = useSupabaseClient()
      try {
        const { error } = await supabase.auth.updateUser({
          password: password
        })

        if (error) throw error

        return { success: true }
      } catch (error: any) {
        console.error('Reset password error:', error)
        return {
          success: false,
          message: error.message || "Erreur lors de la réinitialisation."
        }
      } finally {
        this.loading = false
      }
    },

    // Déconnexion
    async logout() {
      const supabase = useSupabaseClient()
      await supabase.auth.signOut()
      this.user = null
      this.session = null

      const router = useRouter()
      router.push('/login')
    },
    // Headers pour l'API Client
    getAuthHeaders() {
      if (this.session?.access_token) {
        return { Authorization: `Bearer ${this.session.access_token}` }
      }
      return {}
    },

    // Rafraîchir le token (delegated to Supabase)
    async refreshAccessToken() {
      const supabase = useSupabaseClient()
      const { data, error } = await supabase.auth.refreshSession()

      if (error || !data.session) {
        return false
      }

      this.session = data.session
      this.user = data.user
      return true
    },
  },
})


