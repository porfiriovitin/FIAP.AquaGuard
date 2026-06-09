import { useState, type FormEvent } from 'react'
import { useNavigate } from 'react-router-dom'
import { Eye, EyeOff, AlertCircle } from 'lucide-react'
import { useAuth } from '../context/AuthContext'
import { LoadingSpinner } from '../../../shared/components/ui/LoadingSpinner'

function isValidEmail(value: string) {
  return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value)
}

export function LoginForm() {
  const { login, status } = useAuth()
  const navigate = useNavigate()

  const [email,        setEmail]        = useState('teste@email.com')
  const [password,     setPassword]     = useState('1ag35v')
  const [showPassword, setShowPassword] = useState(false)
  const [emailError,   setEmailError]   = useState('')
  const [serverError,  setServerError]  = useState('')

  const isLoading = status === 'loading'

  function validateEmail(): boolean {
    if (!email.trim()) {
      setEmailError('Informe seu e-mail.')
      return false
    }
    if (!isValidEmail(email)) {
      setEmailError('E-mail inválido.')
      return false
    }
    setEmailError('')
    return true
  }

  async function handleSubmit(e: FormEvent) {
    e.preventDefault()
    setServerError('')

    if (!validateEmail()) return
    if (!password) return

    const result = await login(email, password)
    if (result.ok) {
      navigate('/app', { replace: true })
    } else {
      setPassword('')
      setServerError(result.message ?? 'Erro ao autenticar.')
    }
  }

  const inputBase =
    'w-full h-12 px-4 rounded-[var(--radius-sm)] ' +
    'bg-[var(--bg-sunken)] border border-[var(--border)] ' +
    'text-[var(--fg)] text-[14px] font-[var(--font-sans)] ' +
    'placeholder:text-[var(--fg-subtle)] ' +
    'focus:outline-none focus:border-[var(--accent)] ' +
    'focus:shadow-[var(--ring-focus)] ' +
    'transition-colors duration-100 ' +
    'disabled:opacity-50'

  return (
    <form onSubmit={handleSubmit} noValidate className="flex flex-col gap-5">
      <div>
        <h2
          className="mb-1"
          style={{
            fontFamily:    'var(--font-display)',
            fontWeight:    700,
            fontSize:      '22px',
            color:         'var(--navy-900)',
            letterSpacing: '-0.02em',
          }}
        >
          Entrar
        </h2>
        <p style={{ fontSize: '14px', color: 'var(--fg-muted)' }}>
          Acesse sua conta AquaGuard
        </p>
      </div>

      {/* E-mail */}
      <div className="flex flex-col gap-1.5">
        <label
          htmlFor="email"
          style={{ fontSize: '13px', fontWeight: 500, color: 'var(--fg)' }}
        >
          E-mail
        </label>
        <input
          id="email"
          type="email"
          autoComplete="email"
          inputMode="email"
          placeholder="seu@email.com"
          value={email}
          disabled={isLoading}
          onChange={e => { setEmail(e.target.value); setEmailError('') }}
          onBlur={validateEmail}
          className={`${inputBase} ${emailError ? 'border-[var(--danger)] focus:border-[var(--danger)] focus:shadow-[var(--ring-danger)]' : ''}`}
          aria-describedby={emailError ? 'email-error' : undefined}
          aria-invalid={!!emailError}
        />
        {emailError && (
          <p id="email-error" className="flex items-center gap-1" style={{ fontSize: '12px', color: 'var(--danger)' }}>
            <AlertCircle size={12} />
            {emailError}
          </p>
        )}
      </div>

      {/* Senha */}
      <div className="flex flex-col gap-1.5">
        <label
          htmlFor="password"
          style={{ fontSize: '13px', fontWeight: 500, color: 'var(--fg)' }}
        >
          Senha
        </label>
        <div className="relative">
          <input
            id="password"
            type={showPassword ? 'text' : 'password'}
            autoComplete="current-password"
            placeholder="••••••••"
            value={password}
            disabled={isLoading}
            onChange={e => { setPassword(e.target.value); setServerError('') }}
            className={`${inputBase} pr-12`}
          />
          <button
            type="button"
            onClick={() => setShowPassword(v => !v)}
            disabled={isLoading}
            aria-label={showPassword ? 'Ocultar senha' : 'Mostrar senha'}
            className="absolute right-3 top-1/2 -translate-y-1/2 p-1 rounded
                       text-[var(--fg-subtle)] hover:text-[var(--fg)]
                       focus-visible:outline-none focus-visible:ring-2
                       focus-visible:ring-[var(--accent)]
                       transition-colors duration-100"
          >
            {showPassword ? <EyeOff size={18} /> : <Eye size={18} />}
          </button>
        </div>
      </div>

      {/* Erro do servidor */}
      {serverError && (
        <div
          className="flex items-start gap-2 px-3 py-2.5 rounded-[var(--radius-sm)]"
          style={{
            background: 'var(--risk-critical-tint)',
            border:     '1px solid var(--danger)',
            fontSize:   '13px',
            color:      'var(--danger)',
          }}
          role="alert"
        >
          <AlertCircle size={15} className="mt-px shrink-0" />
          <span>{serverError}</span>
        </div>
      )}

      {/* Submit */}
      <button
        type="submit"
        disabled={isLoading || !email || !password}
        className="h-12 w-full flex items-center justify-center gap-2
                   rounded-[var(--radius-sm)] font-semibold text-[15px] text-white
                   bg-[var(--accent)] active:bg-[var(--accent-press)]
                   active:scale-[0.98]
                   focus-visible:outline-none focus-visible:ring-2
                   focus-visible:ring-[var(--accent)]
                   disabled:opacity-50 disabled:pointer-events-none
                   transition-all duration-100"
      >
        {isLoading ? (
          <>
            <LoadingSpinner size="sm" className="text-white" />
            <span>Entrando…</span>
          </>
        ) : (
          'Entrar'
        )}
      </button>
    </form>
  )
}
