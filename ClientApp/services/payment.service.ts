import { ApiClient } from './api.client'
import type { Subscription, SubscriptionType } from '~/types'

export class PaymentService {
    private client: ApiClient

    constructor(client: ApiClient) {
        this.client = client
    }

    async createSubscription(): Promise<any> {
        return this.client.post<any>('/payment/create-subscription')
    }

    async getPaymentStatus(sessionId: string): Promise<any> {
        return this.client.get<any>(`/payment/status/${sessionId}`)
    }

    async getMySubscription(): Promise<{ hasSubscription: boolean; subscription?: Subscription }> {
        return this.client.get<{ hasSubscription: boolean; subscription?: Subscription }>('/payment/my-subscription')
    }

    async hasAccess(): Promise<{ hasAccess: boolean }> {
        return this.client.get<{ hasAccess: boolean }>('/payment/has-access')
    }

    async cancelSubscription(): Promise<{ message: string }> {
        return this.client.post<{ message: string }>('/payment/cancel-subscription')
    }
}
