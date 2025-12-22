export class ApiClient {
    private apiBase: string

    constructor() {
        const config = useRuntimeConfig()
        this.apiBase = config.public.apiBase as string
    }

    async get<T>(endpoint: string, options?: RequestInit): Promise<T> {
        return this.request<T>(endpoint, { ...options, method: 'GET' })
    }

    async post<T>(endpoint: string, body?: any, options?: RequestInit): Promise<T> {
        return this.request<T>(endpoint, { ...options, method: 'POST', body: JSON.stringify(body) })
    }

    async put<T>(endpoint: string, body?: any, options?: RequestInit): Promise<T> {
        return this.request<T>(endpoint, { ...options, method: 'PUT', body: JSON.stringify(body) })
    }

    async delete<T>(endpoint: string, options?: RequestInit): Promise<T> {
        return this.request<T>(endpoint, { ...options, method: 'DELETE' })
    }

    async upload<T>(endpoint: string, formData: FormData, options?: RequestInit): Promise<T> {
        const authStore = useAuthStore()
        const url = `${this.apiBase}${endpoint}`

        return await $fetch<T>(url, {
            ...options,
            method: 'POST' as any,
            headers: {
                ...authStore.getAuthHeaders(),
                ...options?.headers,
                // Content-Type is left alone for FormData to set boundary
            } as any,
            body: formData
        })
    }

    private async request<T>(endpoint: string, options?: RequestInit): Promise<T> {
        const authStore = useAuthStore()
        const url = `${this.apiBase}${endpoint}`

        try {
            const response = await $fetch<T>(url, {
                ...options,
                method: options?.method as any,
                headers: {
                    'Content-Type': 'application/json',
                    ...authStore.getAuthHeaders(),
                    ...options?.headers,
                } as any,
            })
            return response
        } catch (error: any) {
            if (error.status === 401 && authStore.refreshToken) {
                const refreshed = await authStore.refreshAccessToken()
                if (refreshed) {
                    return await $fetch<T>(url, {
                        ...options,
                        method: options?.method as any,
                        headers: {
                            'Content-Type': 'application/json',
                            ...authStore.getAuthHeaders(),
                            ...options?.headers,
                        } as any,
                    })
                } else {
                    if (import.meta.client) {
                        navigateTo('/login')
                    }
                }
            }
            throw error
        }
    }
}
