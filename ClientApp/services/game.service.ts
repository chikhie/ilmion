import { ApiClient } from './api.client'
import type { Game, Party } from '~/types'

export class GameService {
    private client: ApiClient

    constructor(client: ApiClient) {
        this.client = client
    }

    async getAll(): Promise<Game[]> {
        return await this.client.get<Game[]>('/games')
    }

    async getById(id: string): Promise<Game> {
        const games = await this.getAll()
        const game = games.find(g => g.id === id)
        if (!game) throw new Error('Game not found')
        return game
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
