import logoMark from '../../../assets/logos/logo-mark.png'

export function TopAppBar() {
  return (
    <header
      className="fixed top-0 inset-x-0 z-50 h-[65px] flex items-center justify-between
                 bg-[var(--bg-elevated)] border-b border-[var(--border)]
                 shadow-[var(--shadow-sm)]"
      style={{
        // Inline style owns all padding props to avoid specificity conflicts with Tailwind.
        // env() accounts for the iPhone notch area on horizontal edges.
        paddingLeft:  'calc(1.25rem + env(safe-area-inset-left))',
        paddingRight: 'calc(1.25rem + env(safe-area-inset-right))',
      }}
    >
      <div className="flex items-center gap-2.5">
        <img
          src={logoMark}
          alt="AquaGuard"
          className="h-8 w-auto"
        />
        <span
          style={{
            fontFamily:    'var(--font-display)',
            fontWeight:    800,
            fontSize:      '15px',
            letterSpacing: '0.14em',
            color:         'var(--navy-900)',
          }}
        >
          AQUAGUARD
        </span>
      </div>
    </header>
  )
}
