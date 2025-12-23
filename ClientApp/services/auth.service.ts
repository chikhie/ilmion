
import type { LoginRequest, RegisterRequest, AuthResponse, User } from '~/types'

export class AuthService {
    private apiBase: string

    constructor() {
        const config = useRuntimeConfig()
        this.apiBase = config.public.apiBase as string
    }

    async login(credentials: LoginRequest): Promise<AuthResponse> {
        return $fetch<AuthResponse>(`${this.apiBase}/auth/login`, {
            method: 'POST',
            body: credentials,
        })
    }

    async register(data: RegisterRequest): Promise<void> {
        return $fetch<void>(`${this.apiBase}/auth/register`, {
            method: 'POST',
            body: data,
        })
    }

    async logout(token: string | null): Promise<void> {
        if (!token) return
        try {
            await $fetch(`${this.apiBase}/auth/logout`, {
                method: 'POST',
                headers: { Authorization: `Bearer ${token}` },
            })
        } catch (error) {
            console.error('Logout error:', error)
            // Ignore error on logout
        }
    }

    async refreshToken(refreshToken: string): Promise<AuthResponse> {
        return $fetch<AuthResponse>(`${this.apiBase}/auth/refresh`, {
            method: 'POST',
            body: { refreshToken },
        })
    }

    async getProfile(token: string): Promise<User> {
        return $fetch<User>(`${this.apiBase}/user/me`, {
            headers: { Authorization: `Bearer ${token}` },
        })
    }

    async checkSubscriptionAccess(token: string): Promise<boolean> {
        try {
            const response = await $fetch<{ hasAccess: boolean }>(`${this.apiBase}/payment/has-access`, {
                headers: { Authorization: `Bearer ${token}` },
            })
            return response.hasAccess
        } catch (error) {
            return false
        }
    }
}

export const authService = new AuthService()
