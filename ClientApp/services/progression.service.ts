import type { ProgressionViewModel } from '~/types'

export class ProgressionService {
    private get apiBase(): string {
        return useRuntimeConfig().public.apiBase as string
    }

    async getProgression(token: string): Promise<ProgressionViewModel> {
        return $fetch<ProgressionViewModel>(`${this.apiBase}/progression`, {
            method: 'GET',
            headers: { Authorization: `Bearer ${token}` },
        })
    }
}

export const progressionService = new ProgressionService()
