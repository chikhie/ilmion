export interface User {
  id: string
  username: string
  email: string
  roles: string[]
}

export interface AuthResponse {
  token: string
  expiration: string
}

