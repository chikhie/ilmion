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
  thumbnailPath?: string
}
export interface Quiz {
  text: string
  type: string
  correctAnswer: string | number
  explanation: string
  options: string[] | null
}

export interface Player {
  username: string
  score: number
  connectionId?: string // Kept for compatibility if needed, but less relevant with Supabase
  hasAnswered?: boolean // Helper for frontend state
}

export interface Party {
  code: string
  gameId: string
  hostUsername: string
  status: 'Waiting' | 'Playing' | 'Finished'
  createdAt: string
  players: Player[]
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
  daysRemaining?: number
}


export interface LoginRequest {
  email?: string
  username?: string
  password: string
}

export interface RegisterRequest {
  username: string
  full_name: string
  email: string
  password: string
  firstName?: string
  lastName?: string
  dateOfBirth?: string
}

export interface AuthResponse {
  token: string
  refreshToken: string
}

export interface Module {
  id: string
  label: string
  subjectName: string
  chapterCount: number
  isFree?: boolean
  hasAccess?: boolean
  price?: number
}

export interface PartProgressionDto {
  partId: string
  score: number
  totalQuestions: number
  isMastered: boolean
  lastPlayed: string
  attempts: number
}

// Progression Types
export interface ModuleProgressionDto {
  moduleId: string
  label: string
  totalPoints: number
  isMastered: boolean
  currentMasteryLabel: string
  lastPlayed: string | null
  questionsAnswered: number
  totalQuestions: number
  parts: PartProgressionDto[]
}

export interface ProgressionViewModel {
  globalPoints: number
  modulesMasteredCount: number
  modules: ModuleProgressionDto[]
}

export interface SaveQuizResultRequest {
  score: number
  totalQuestions: number
  themeId?: string
  partId?: string
  durationSeconds: number
}
