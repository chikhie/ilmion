import { ApiClient } from './api.client'
import type { User } from '~/types'

export class UserService {
    private client: ApiClient

    constructor(client: ApiClient) {
        this.client = client
    }

    async getProfile(): Promise<User> {
        return this.client.get<User>('/user/me')
    }

    async updateProfile(data: any): Promise<any> {
        return this.client.put<any>('/user/profile', data)
    }

    async uploadProfilePicture(file: File): Promise<any> {
        const formData = new FormData()
        formData.append('file', file)
        return this.client.upload<any>('/user/profile/picture', formData)
    }

    async deleteProfilePicture(): Promise<any> {
        return this.client.delete<any>('/user/profile/picture')
    }

    async changePassword(data: { currentPassword: string; newPassword: string }): Promise<any> {
        return this.client.post<any>('/user/change-password', data)
    }
}
