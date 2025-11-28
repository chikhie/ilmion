export const useApi = () => {
  const config = useRuntimeConfig()
  const apiBase = config.public.apiBase as string
  const auth = useAuth()

  const fetchApi = async <T>(endpoint: string, options?: RequestInit): Promise<T> => {
    const url = `${apiBase}${endpoint}`
    
    // Récupérer le token depuis le composable useAuth
    const token = auth.token.value

    try {
      const response = await $fetch<T>(url, {
        ...options,
        headers: {
          'Content-Type': 'application/json',
          ...(token ? { 'Authorization': `Bearer ${token}` } : {}),
          ...options?.headers,
        },
      })
      return response
    } catch (error: any) {
      // Gérer l'expiration du token (401)
      if (error.status === 401) {
        auth.logout()
      }
      console.error('API Error:', error)
      throw error
    }
  }

  return {
    // Subjects
    getSubjects: () => fetchApi<any[]>('/subject'),
    getSubject: (id: number) => fetchApi<any>(`/subject/${id}`),
    
    // Modules
    getModulesBySubject: (subjectId: number) => fetchApi<any[]>(`/module/subject/${subjectId}`),
    getModule: (id: string) => fetchApi<any>(`/module/${id}`),
    createModule: (data: any) => fetchApi<any>('/module', { method: 'POST', body: JSON.stringify(data) }),
    updateModule: (id: string, data: any) => fetchApi<any>(`/module/${id}`, { method: 'PUT', body: JSON.stringify(data) }),
    deleteModule: (id: string) => fetchApi<void>(`/module/${id}`, { method: 'DELETE' }),

    // Stats (Dashboard)
    getDashboardStats: () => fetchApi<any>('/stats/dashboard'),
  }
}

