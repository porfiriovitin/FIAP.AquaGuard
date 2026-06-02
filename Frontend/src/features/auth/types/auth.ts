export interface AuthUser {
  id: string
  name: string
  email: string
  role: number
}

export interface LoginRequest {
  email: string
  password: string
}
