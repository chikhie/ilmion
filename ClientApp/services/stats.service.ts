import { ApiClient } from './api.client'
import type { DashboardStats } from '~/types'

export class StatsService {
    private client: ApiClient

    constructor(client: ApiClient) {
        this.client = client
    }

    async getDashboardStats(): Promise<DashboardStats> {
        return this.client.get<DashboardStats>('/stats/dashboard')
    }
}
