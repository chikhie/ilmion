import { ApiClient } from '~/services/api.client'
import { UserService } from '~/services/user.service'
import { GameService } from '~/services/game.service'
import { PaymentService } from '~/services/payment.service'
import { StatsService } from '~/services/stats.service'

export const useApi = () => {
  const client = new ApiClient()

  const userService = new UserService(client)
  const gameService = new GameService()

  const paymentService = new PaymentService(client)
  const statsService = new StatsService(client)

  return {
    // Services access
    user: userService,
    game: gameService,
    payment: paymentService,
    stats: statsService,

    // Backward compatibility (Facade)
    // Subscription & Payment
    createSubscription: () => paymentService.createSubscription(),
    getPaymentStatus: (sessionId: string) => paymentService.getPaymentStatus(sessionId),
    getMySubscription: () => paymentService.getMySubscription(),
    hasAccess: () => paymentService.hasAccess(),
    cancelSubscription: () => paymentService.cancelSubscription(),

    // User Profile
    getProfile: () => userService.getProfile(),
    updateProfile: (data: any) => userService.updateProfile(data),
    uploadProfilePicture: (file: File) => userService.uploadProfilePicture(file),
    deleteProfilePicture: () => userService.deleteProfilePicture(),
    changePassword: (data: { currentPassword: string; newPassword: string }) => userService.changePassword(data),

    // Stats
    getDashboardStats: () => statsService.getDashboardStats(),
  }
}

