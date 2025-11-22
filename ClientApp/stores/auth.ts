import { defineStore } from 'pinia'
import type { User, LoginRequest, RegisterRequest, AuthResponse } from '~/types'

export const useAuthStore = defineStore('auth', {
  state: () => ({
    user: null as User | null,
    token: null as string | null,
    refreshToken: null as string | null,
    isAuthenticated: false,
    loading: false,
  }),

  getters: {
    getUser: (state) => state.user,
    getToken: (state) => state.token,
    isLoggedIn: (state) => state.isAuthenticated,
  },

  actions: {
    // Initialiser l'authentification depuis le localStorage
    async initAuth() {
      if (process.client) {
        const token = localStorage.getItem('token')
        const refreshToken = localStorage.getItem('refreshToken')
        const userStr = localStorage.getItem('user')

        if (token) {
          try {
            this.token = token
            this.refreshToken = refreshToken
            this.isAuthenticated = true
            
            if (userStr) {
              const parsedUser = JSON.parse(userStr)
              this.user = parsedUser
            }
            
            // Rafraîchir les données utilisateur depuis l'API
            await this.fetchUserProfile()
          } catch (error) {
            console.error('Error parsing user from localStorage:', error)
            this.clearAuth()
          }
        }
      }
    },

    // Connexion
    async login(credentials: LoginRequest) {
      this.loading = true
      try {
        const config = useRuntimeConfig()
        const response = await $fetch<AuthResponse>(`${config.public.apiBase}/auth/login`, {
          method: 'POST',
          body: credentials,
        })

        this.setAuth(response)
        
        // Récupérer les données complètes de l'utilisateur depuis /api/user/me
        await this.fetchUserProfile()
        
        return { success: true }
      } catch (error: any) {
        console.error('Login error:', error)
        return { 
          success: false, 
          message: error.data?.message || 'Erreur de connexion. Vérifiez vos identifiants.' 
        }
      } finally {
        this.loading = false
      }
    },

    // Inscription
    async register(data: RegisterRequest) {
      this.loading = true
      try {
        const config = useRuntimeConfig()
        await $fetch(`${config.public.apiBase}/auth/register`, {
          method: 'POST',
          body: data,
        })

        return { 
          success: true, 
          message: 'Inscription réussie ! Vérifiez votre email pour confirmer votre compte.' 
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

    // Déconnexion
    async logout() {
      try {
        const config = useRuntimeConfig()
        await $fetch(`${config.public.apiBase}/auth/logout`, {
          method: 'POST',
          headers: this.getAuthHeaders(),
        })
      } catch (error) {
        console.error('Logout error:', error)
      } finally {
        this.clearAuth()
      }
    },

    // Rafraîchir le token
    async refreshAccessToken() {
      if (!this.refreshToken) return false

      try {
        const config = useRuntimeConfig()
        const response = await $fetch<AuthResponse>(`${config.public.apiBase}/auth/refresh`, {
          method: 'POST',
          body: { refreshToken: this.refreshToken },
        })

        this.setAuth(response)
        return true
      } catch (error) {
        console.error('Refresh token error:', error)
        this.clearAuth()
        return false
      }
    },

    // Définir l'authentification
    setAuth(authData: AuthResponse) {
      this.token = authData.token
      this.refreshToken = authData.refreshToken
      this.isAuthenticated = true

      // Sauvegarder dans localStorage
      if (process.client) {
        localStorage.setItem('token', authData.token)
        localStorage.setItem('refreshToken', authData.refreshToken)
      }
    },

    // Nettoyer l'authentification
    clearAuth() {
      this.user = null
      this.token = null
      this.refreshToken = null
      this.isAuthenticated = false

      if (process.client) {
        localStorage.removeItem('token')
        localStorage.removeItem('refreshToken')
        localStorage.removeItem('user')
      }
    },

    // Obtenir les headers d'authentification
    getAuthHeaders() {
      return this.token ? { Authorization: `Bearer ${this.token}` } : {}
    },

    // Récupérer le profil utilisateur depuis /api/user/me
    async fetchUserProfile() {
      if (!this.token) return

      try {
        const config = useRuntimeConfig()
        const userProfile = await $fetch<User>(`${config.public.apiBase}/user/me`, {
          headers: this.getAuthHeaders(),
        })

        this.user = userProfile
        
        // Mettre à jour le localStorage avec les vraies données
        if (process.client) {
          localStorage.setItem('user', JSON.stringify(userProfile))
        }
      } catch (error) {
        console.error('Error fetching user profile:', error)
      }
    },
  },
})

