import {
  createContext,
  useContext,
  useEffect,
  useReducer,
  type ReactNode,
} from 'react'
import type { AuthUser } from '../types/auth'
import * as authService from '../services/authService'

// ─── State ───────────────────────────────────────────────────────────────────

type AuthState =
  | { status: 'idle';            user: null }
  | { status: 'loading';         user: null }
  | { status: 'authenticated';   user: AuthUser }
  | { status: 'unauthenticated'; user: null }

type AuthAction =
  | { type: 'RESTORE';       user: AuthUser }
  | { type: 'NO_SESSION' }
  | { type: 'LOGIN_START' }
  | { type: 'LOGIN_SUCCESS'; user: AuthUser }
  | { type: 'LOGIN_FAIL' }
  | { type: 'LOGOUT' }

function authReducer(_: AuthState, action: AuthAction): AuthState {
  switch (action.type) {
    case 'RESTORE':       return { status: 'authenticated',   user: action.user }
    // 'idle' guards against premature redirects during localStorage check (see ProtectedRoute).
    // 'NO_SESSION' resolves that guard to 'unauthenticated' so the route can redirect.
    case 'NO_SESSION':    return { status: 'unauthenticated', user: null }
    case 'LOGIN_START':   return { status: 'loading',         user: null }
    case 'LOGIN_SUCCESS': return { status: 'authenticated',   user: action.user }
    case 'LOGIN_FAIL':    return { status: 'unauthenticated', user: null }
    case 'LOGOUT':        return { status: 'unauthenticated', user: null }
  }
}

// ─── Context ─────────────────────────────────────────────────────────────────

type AuthContextValue = AuthState & {
  login(email: string, password: string): Promise<{ ok: boolean; message?: string }>
  logout(): void
}

const AuthContext = createContext<AuthContextValue | null>(null)

const STORAGE_KEY = 'aquaguard:user'

// ─── Provider ────────────────────────────────────────────────────────────────

export function AuthProvider({ children }: { children: ReactNode }) {
  const [state, dispatch] = useReducer(authReducer, { status: 'idle', user: null })

  // Restore session from localStorage on mount.
  // We use 'idle' (not 'loading') so ProtectedRoute can distinguish "not checked yet"
  // from "checked and confirmed unauthenticated", preventing a premature redirect flash.
  useEffect(() => {
    try {
      const saved = localStorage.getItem(STORAGE_KEY)
      if (saved) dispatch({ type: 'RESTORE', user: JSON.parse(saved) as AuthUser })
      else        dispatch({ type: 'NO_SESSION' })
    } catch {
      dispatch({ type: 'NO_SESSION' })
    }
  }, [])

  // Automatic logout when any apiFetch receives a 401 (expired/invalid token).
  // Inline instead of calling logout() to avoid capturing a stale function reference
  // in the empty-dep closure — dispatch and STORAGE_KEY are both stable.
  useEffect(() => {
    const handle = () => {
      localStorage.removeItem(STORAGE_KEY)
      dispatch({ type: 'LOGOUT' })
    }
    window.addEventListener('auth:unauthorized', handle)
    return () => window.removeEventListener('auth:unauthorized', handle)
  }, [])

  async function login(email: string, password: string): Promise<{ ok: boolean; message?: string }> {
    dispatch({ type: 'LOGIN_START' })
    try {
      const res = await authService.login({ email, password })
      if (res.status === 'Success' && res.data) {
        localStorage.setItem(STORAGE_KEY, JSON.stringify(res.data))
        dispatch({ type: 'LOGIN_SUCCESS', user: res.data })
        return { ok: true }
      }
      dispatch({ type: 'LOGIN_FAIL' })
      return { ok: false, message: res.message ?? 'Credenciais inválidas.' }
    } catch (err) {
      dispatch({ type: 'LOGIN_FAIL' })
      const isUnauth = err instanceof Error && err.message === 'Unauthorized'
      return {
        ok: false,
        message: isUnauth ? 'Credenciais inválidas.' : 'Erro de conexão. Tente novamente.',
      }
    }
  }

  function logout() {
    localStorage.removeItem(STORAGE_KEY)
    dispatch({ type: 'LOGOUT' })
    // Navigation to /login is handled by ProtectedRoute reacting to user = null,
    // keeping the auth module decoupled from the router.
  }

  return (
    <AuthContext.Provider value={{ ...state, login, logout }}>
      {children}
    </AuthContext.Provider>
  )
}

// ─── Hook ────────────────────────────────────────────────────────────────────

export function useAuth(): AuthContextValue {
  const ctx = useContext(AuthContext)
  if (!ctx) throw new Error('useAuth must be used inside <AuthProvider>')
  return ctx
}
