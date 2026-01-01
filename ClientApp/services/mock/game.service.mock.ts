import { ApiClient } from '../api.client'
import type { Game, Party } from '~/types'
import { gamesData } from './games.data'

export class MockGameService {
    async getAll(): Promise<Game[]> {
        return gamesData
    }

    async getById(id: string): Promise<Game> {
        const games = await this.getAll()
        const game = games.find(g => g.id === id)
        if (!game) throw new Error('Game not found')
        return game
    }

    async createParty(gameId: string, hostUsername: string): Promise<Party> {
        const code = Math.random().toString(36).substring(2, 8).toUpperCase()
        return {
            code,
            gameId,
            hostUsername,
            status: 'Waiting',
            createdAt: new Date().toISOString(),
            players: []
        }
    }

    async joinParty(code: string, username: string): Promise<Party> {
        return {
            code,
            gameId: '',
            hostUsername: '',
            status: 'Waiting',
            createdAt: new Date().toISOString(),
            players: []
        }
    }

    async getParties(): Promise<Party[]> {
        return []
    }
}
