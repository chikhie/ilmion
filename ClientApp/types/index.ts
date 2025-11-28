export interface Subject {
  id: number
  label: string
}

export interface Module {
  id: string
  label: string
  subjectId: number
  subjectName: string
  chapterCount: number
  createdAt: string
  updatedAt: string
}

export interface Chapter {
  id: string
  title: string
  description: string
  displayOrder: number
  durationMinutes: number
  hasAccess: boolean
  isFree: boolean // Premier chapitre gratuit
  moduleId: string
  moduleName: string
  subjectId: number // ID de la matière
  subjectName: string // Nom de la matière
  videoId?: string
  videoUrl?: string
  content?: string
  createdAt: string
  updatedAt: string
}

export interface Section {
  id: string
  title: string
  content: string
  sectionTypeId: number
  sectionTypeLabel: string // "Texte", "Image", "Vidéo", etc.
  displayOrder: number
  chapterId: string
  createdAt: string
  updatedAt: string
}

// Types de section disponibles
export const SectionTypes = {
  TEXT: 1,
  IMAGE: 2,
  VIDEO: 3,
  QUIZ: 4,
  CODE: 5,
  EXERCISE: 6
} as const

export interface CreateModuleDto {
  label: string
  subjectId: number
}

export interface CreateChapterDto {
  title: string
  description: string
  displayOrder: number
  durationMinutes: number
  moduleId: string
  videoId?: string
  content?: string
}

export interface CreateSectionDto {
  title: string
  content: string
  type: number
  displayOrder: number
  chapterId: string
}

// Auth Types
export interface LoginRequest {
  email: string
  password: string
}

export interface RegisterRequest {
  username: string
  email: string
  password: string
  firstName?: string
  lastName?: string
  dateOfBirth?: string
}

export interface AuthResponse {
  token: string
  expiration: string
  refreshToken: string
  refreshTokenExpiration: string
}

export interface User {
  id: string
  username: string
  email: string
  firstName?: string
  lastName?: string
  profilePicture?: string
  dateOfBirth?: string
}

// Payment & Subscription Types
export interface PaymentSession {
  sessionId: string
  sessionUrl: string
  subscriptionId: string
}

export interface Subscription {
  id: string
  type: string
  status: string
  amountPaid: number
  currency: string
  startDate: string
  endDate: string
  cancelledDate?: string
  isActive: boolean
  daysRemaining: number
}

// Stats Types
export interface FollowedModule {
  id: string
  label: string
  subjectName: string
  chapterCount: number
  progress: number
  imageUrl?: string
}

export interface DashboardStats {
  followedModules: FollowedModule[]
  globalProgress: number
  learningTimeSeconds: number
  formattedLearningTime: string
}

export interface UpdateProgressDto {
  sectionId: string
  isCompleted: boolean
}

export interface LogTimeDto {
  durationSeconds: number
  moduleId?: string
}
