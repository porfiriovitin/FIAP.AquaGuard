import { apiFetch } from '../../../shared/services/api'
import type { AuthUser, LoginRequest } from '../types/auth'
import type { PayloadResponse } from '../../../shared/services/api'

const MOCK = import.meta.env.VITE_MOCK_DATA === 'true'

// Manager (1) permite acessar todas as rotas protegidas do app durante desenvolvimento.
// Veja UserRole em .claude/resources/api/project-info.md.
const MOCK_USER: AuthUser = {
  id:    'mock-user-id',
  name:  'Dev User',
  email: 'dev@aquaguard.com',
  role:  1,
}

function mockLogin(): Promise<PayloadResponse<AuthUser>> {
  return new Promise(resolve =>
    setTimeout(() => resolve({ status: 'Success', data: MOCK_USER }), 600),
  )
}

export function login(req: LoginRequest): Promise<PayloadResponse<AuthUser>> {
  if (MOCK) return mockLogin()
  return apiFetch<AuthUser>('/api/login', {
    method: 'POST',
    body: JSON.stringify(req),
  })
}
