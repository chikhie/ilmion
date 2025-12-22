export interface User {
  id: string
  username: string
  email: string
  firstName?: string
  lastName?: string
  profilePicture?: string
  dateOfBirth?: string
  bio?: string
}

export interface UserProfile extends User { }

export interface DashboardStats {
  globalProgress: number
  learningTimeSeconds: number
  formattedLearningTime: string
}

export enum GameType {
  MapPlacement = 0,
  Translation = 1,
  Genealogy = 2,
  Quiz = 3
}

export interface Game {
  id: string
  title: string
  description?: string
  type: GameType
  difficulty: string
  isPremium: boolean
  thumbnailPath?: string
}

export enum SubscriptionType {
  Annual = 0
}

export enum SubscriptionStatus {
  Active = 0,
  Expired = 1,
  Cancelled = 2,
  Pending = 3
}

export interface Subscription {
  id: string
  userId: string
  type: SubscriptionType
  amountPaid: number
  currency: string
  status: SubscriptionStatus
  startDate: string
  endDate: string
  cancelledDate?: string
}

