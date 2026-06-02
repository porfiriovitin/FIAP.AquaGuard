import type { ReactNode } from 'react'
import { Navigate } from 'react-router-dom'
import { useAuth } from '../../../features/auth/context/AuthContext'
import { LoadingSpinner } from '../ui/LoadingSpinner'

interface Props {
  children: ReactNode
}

export function ProtectedRoute({ children }: Props) {
  const { status, user } = useAuth()

  // 'idle' means localStorage hasn't been checked yet. Rendering a spinner prevents
  // a premature redirect to /login that would flash even for authenticated users.
  if (status === 'idle') {
    return (
      <div
        className="min-h-[100dvh] flex items-center justify-center"
        style={{ background: 'var(--bg)' }}
      >
        <LoadingSpinner size="lg" className="text-[var(--accent)]" />
      </div>
    )
  }

  if (!user) return <Navigate to="/login" replace />

  return <>{children}</>
}
