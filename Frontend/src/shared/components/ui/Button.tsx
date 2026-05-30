import { type ButtonHTMLAttributes } from 'react'

type Variant = 'primary' | 'ghost' | 'link'
type Size = 'sm' | 'md'

interface Props extends ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: Variant
  size?: Size
}

const baseClasses =
  'inline-flex items-center justify-center gap-1.5 font-medium transition-all duration-100 ' +
  'focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-[var(--accent)] ' +
  'disabled:opacity-50 disabled:pointer-events-none'

const variantClasses: Record<Variant, string> = {
  primary: 'bg-[var(--accent)] text-white rounded-[var(--radius-sm)] active:bg-[var(--accent-press)] active:scale-[0.98]',
  ghost:   'bg-transparent border border-[var(--border)] text-[var(--fg)] rounded-[var(--radius-sm)] active:bg-[var(--bg-sunken)]',
  // Link variant intentionally omits size padding — it renders as inline text
  link:    'bg-transparent text-[var(--accent)] uppercase tracking-[0.06em] text-[11px] font-semibold active:opacity-70',
}

const sizeClasses: Record<Size, string> = {
  sm: 'px-3 py-1.5 text-[13px]',
  md: 'px-4 py-2 text-[14px]',
}

export function Button({
  variant = 'primary',
  size = 'md',
  // Explicit default prevents accidental form submission when nested inside <form>
  type = 'button',
  className = '',
  children,
  ...props
}: Props) {
  return (
    <button
      type={type}
      className={`${baseClasses} ${variantClasses[variant]} ${variant !== 'link' ? sizeClasses[size] : ''} ${className}`}
      {...props}
    >
      {children}
    </button>
  )
}
