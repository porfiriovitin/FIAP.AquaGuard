import { Navigate } from 'react-router-dom'
import logoMark from '../assets/logos/logo-mark.png'
import { LoginForm } from '../features/auth/components/LoginForm'
import { useAuth } from '../features/auth/context/AuthContext'

export function LoginPage() {
  const { user } = useAuth()

  // Prevent rendering the page content at all for authenticated users.
  if (user) return <Navigate to="/" replace />

  return (
    /*
     * Safari viewport fix: `100dvh` accounts for collapsible chrome (Safari 15.4+).
     * `min-h-screen` (100vh) stays as a CSS-cascade fallback for older Safari via
     * the Tailwind class; the inline style overrides it in supporting browsers.
     */
    <div
      className="min-h-screen flex flex-col bg-[var(--bg-elevated)]"
      style={{ minHeight: '100dvh' }}
    >
      {/* ── Brand section ── */}
      <section
        className="flex-1 flex flex-col items-center justify-center gap-4 px-6 pb-10"
        style={{ paddingTop: 'max(3rem, env(safe-area-inset-top))' }}
      >
        <img src={logoMark} alt="AquaGuard" className="h-32 w-auto" />

        {/* Wordmark — navy-950 over white */}
        <span
          style={{
            fontFamily:    'var(--font-display)',
            fontWeight:    800,
            fontSize:      '22px',
            letterSpacing: '0.14em',
            color:         'var(--navy-950)',
          }}
        >
          AQUAGUARD
        </span>

        {/* Tagline */}
        <p
          className="text-center max-w-[240px]"
          style={{ fontSize: '13px', lineHeight: 1.6, color: 'var(--fg-muted)' }}
        >
          Monitoramento inteligente de riscos de enchentes
        </p>
      </section>

      {/* ── Form card ──
          Mobile:  full-width, top border, white — anchored to bottom.
          Desktop: floating card with border + shadow, centered.
      ── */}
      <section
        className="
          w-full bg-[var(--bg-elevated)] px-6 pt-8
          border-t border-[var(--border)]

          sm:max-w-[420px] sm:mx-auto sm:mb-12
          sm:rounded-[var(--radius-2xl)]
          sm:border sm:shadow-[var(--shadow-lg)]
          sm:px-8 sm:py-10
        "
        style={{
          paddingBottom: 'calc(2rem + env(safe-area-inset-bottom))',
        }}
      >
        <LoginForm />
      </section>
    </div>
  )
}
