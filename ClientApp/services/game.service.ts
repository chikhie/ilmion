import { ApiClient } from './api.client'
import type { Game } from '~/types'

export class GameService {
    private client: ApiClient

    constructor(client: ApiClient) {
        this.client = client
    }

    async getAll(): Promise<Game[]> {
        return this.client.get<Game[]>('/game')
    }

    async getById(id: string): Promise<Game> {
        return this.client.get<Game>(`/game/${id}`)
    }
}
