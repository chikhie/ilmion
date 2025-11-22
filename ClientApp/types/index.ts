export interface Subject {
  id: number
  label: string
}

export interface Module {
  id: string
  title: string
  description: string
  displayOrder: number
  durationMinutes: number
  price: number
  isFree: boolean
  hasAccess: boolean
  subjectId: number
  subjectName: string
  videoId?: string
  sectionCount: number
  createdAt: string
  updatedAt: string
}

export enum SectionType {
  Text = 0,
  Interactive = 1,
  Quiz = 2,
  Exercise = 3,
  Summary = 4
}

export interface Section {
  id: string
  title: string
  content: string
  type: SectionType
  typeLabel: string
  displayOrder: number
  moduleId: string
  componentPath?: string
  hasProtectedComponent: boolean
  createdAt: string
  updatedAt: string
}

export interface CreateModuleDto {
  title: string
  description: string
  displayOrder: number
  durationMinutes: number
  price: number
  isFree: boolean
  subjectId: number
  videoId?: string
}

export interface CreateSectionDto {
  title: string
  content: string
  type: SectionType
  displayOrder: number
  moduleId: string
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

// Payment Types
export interface PaymentSession {
  sessionId: string
  sessionUrl: string
  purchaseId: string
}

export interface ModulePurchase {
  id: string
  moduleId: string
  moduleTitle: string
  subjectName: string
  amountPaid: number
  currency: string
  status: string
  purchaseDate: string
  completedDate?: string
}

