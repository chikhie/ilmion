export const useApi = () => {
  const config = useRuntimeConfig()
  const apiBase = config.public.apiBase as string
  const authStore = useAuthStore()

  const fetchApi = async <T>(endpoint: string, options?: RequestInit): Promise<T> => {
    const url = `${apiBase}${endpoint}`
    
    try {
      const response = await $fetch<T>(url, {
        ...options,
        headers: {
          'Content-Type': 'application/json',
          ...authStore.getAuthHeaders(),
          ...options?.headers,
        },
      })
      return response
    } catch (error: any) {
      // Si erreur 401, essayer de rafraîchir le token
      if (error.status === 401 && authStore.refreshToken) {
        const refreshed = await authStore.refreshAccessToken()
        if (refreshed) {
          // Réessayer la requête avec le nouveau token
          return await $fetch<T>(url, {
            ...options,
            headers: {
              'Content-Type': 'application/json',
              ...authStore.getAuthHeaders(),
              ...options?.headers,
            },
          })
        } else {
          // Rediriger vers la page de connexion
          if (process.client) {
            navigateTo('/login')
          }
        }
      }
      console.error('API Error:', error)
      throw error
    }
  }

  return {
    // Subjects
    getSubjects: () => fetchApi<Subject[]>('/subject'),
    getSubject: (id: number) => fetchApi<Subject>(`/subject/${id}`),

    // Modules
    getModulesBySubject: (subjectId: string) =>
      fetchApi<Module[]>(`/module/subject/${subjectId}`),
    getModule: (id: string) => fetchApi<Module>(`/module/${id}`),
    createModule: (data: CreateModuleDto) =>
      fetchApi<Module>('/module', {
        method: 'POST',
        body: JSON.stringify(data),
      }),
    updateModule: (id: string, data: Partial<CreateModuleDto>) =>
      fetchApi<Module>(`/module/${id}`, {
        method: 'PUT',
        body: JSON.stringify(data),
      }),
    deleteModule: (id: string) =>
      fetchApi<void>(`/module/${id}`, { method: 'DELETE' }),

    // Sections
    getSectionsByModule: (moduleId: string) =>
      fetchApi<Section[]>(`/section/module/${moduleId}`),
    getSection: (id: string) => fetchApi<Section>(`/section/${id}`),
    createSection: (data: CreateSectionDto) =>
      fetchApi<Section>('/section', {
        method: 'POST',
        body: JSON.stringify(data),
      }),
    updateSection: (id: string, data: Partial<CreateSectionDto>) =>
      fetchApi<Section>(`/section/${id}`, {
        method: 'PUT',
        body: JSON.stringify(data),
      }),
    deleteSection: (id: string) =>
      fetchApi<void>(`/section/${id}`, { method: 'DELETE' }),

    // Payment
    createCheckoutSession: (moduleId: string) => 
      fetchApi<PaymentSession>('/payment/create-checkout-session', { 
        method: 'POST', 
        body: JSON.stringify({ moduleId }) 
      }),
    getPaymentStatus: (sessionId: string) => 
      fetchApi<any>(`/payment/status/${sessionId}`),
    getMyPurchases: () => 
      fetchApi<ModulePurchase[]>('/payment/my-purchases'),
    hasAccess: (moduleId: string) => 
      fetchApi<{ hasAccess: boolean; reason: string }>(`/payment/has-access/${moduleId}`),

    // User Profile
    getProfile: () => 
      fetchApi<User>('/user/me'),
    updateProfile: (data: any) => 
      fetchApi<any>('/user/profile', { method: 'PUT', body: JSON.stringify(data) }),
    uploadProfilePicture: async (file: File) => {
      const formData = new FormData()
      formData.append('file', file)
      const url = `${apiBase}/user/profile/picture`
      const response = await $fetch<any>(url, {
        method: 'POST',
        headers: authStore.getAuthHeaders(),
        body: formData,
      })
      return response
    },
    deleteProfilePicture: () => 
      fetchApi<any>('/user/profile/picture', { method: 'DELETE' }),
    changePassword: (data: { currentPassword: string; newPassword: string }) => 
      fetchApi<any>('/user/change-password', { method: 'POST', body: JSON.stringify(data) }),

    // Protected Components
    getProtectedComponent: (sectionId: string) =>
      fetchApi<{ componentCode: string; sectionId: string; sectionTitle: string; accessedAt: string }>
        (`/protected-component/component/${sectionId}`),
    uploadProtectedComponent: async (file: File, componentName: string, sectionId?: string) => {
      const formData = new FormData()
      formData.append('file', file)
      const params = new URLSearchParams({ componentName })
      if (sectionId) params.append('sectionId', sectionId)
      const url = `${apiBase}/protected-component/upload?${params.toString()}`
      const response = await $fetch<any>(url, {
        method: 'POST',
        headers: authStore.getAuthHeaders(),
        body: formData,
      })
      return response
    },
  }
}

