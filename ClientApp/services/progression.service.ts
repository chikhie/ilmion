import type { ProgressionViewModel, SaveQuizResultRequest } from '~/types'

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

    async saveResult(token: string, result: SaveQuizResultRequest): Promise<void> {
        await $fetch(`${this.apiBase}/progression`, {
            method: 'POST',
            headers: { Authorization: `Bearer ${token}` },
            body: result
        })
    }
}

export const progressionService = new ProgressionService()
