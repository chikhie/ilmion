import { ApiClient } from './api.client'
import type { Game, Party, Quiz } from '~/types'

export class GameService {
    private client: ApiClient = new ApiClient()

    async getQuizzForSoloPlayer(questionNumber: number): Promise<Quiz[]> {
        return await this.client.get<Quiz[]>(`/games/quizzes/?count=${questionNumber}`)
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
