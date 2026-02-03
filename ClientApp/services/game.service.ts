import { ApiClient } from './api.client'
import type { Game, Party, Quiz } from '~/types'

export class GameService {
    private client: ApiClient = new ApiClient()

    async getQuizzForSoloPlayer(questionNumber?: number, lang: string = 'fr', themeId?: string, partId?: string): Promise<Quiz[]> {
        let url = `/games/quizzes?lang=${lang}`
        if (questionNumber) url += `&count=${questionNumber}`
        if (themeId) url += `&themeId=${themeId}`
        if (partId) url += `&partId=${partId}`

        return await this.client.get<Quiz[]>(url)
    }

    async createParty(gameId: string, hostUsername: string): Promise<Party> {
        throw new Error("Multiplayer disabled")
    }

    async joinParty(code: string, username: string): Promise<Party> {
        throw new Error("Multiplayer disabled")
    }

    async getParties(): Promise<Party[]> {
        return []
    }
}
