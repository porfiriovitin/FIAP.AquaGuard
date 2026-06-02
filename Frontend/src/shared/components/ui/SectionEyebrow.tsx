import type { ReactNode } from 'react'

interface Props {
  children: ReactNode
  className?: string
}

export function SectionEyebrow({ children, className = '' }: Props) {
  return (
    <span
      className={`t-eyebrow self-start ${className}`.trim()}
      style={{
        backgroundImage:      'linear-gradient(90deg, var(--cyan-400), var(--blue-600))',
        WebkitBackgroundClip: 'text',
        WebkitTextFillColor:  'transparent',
        backgroundClip:       'text',
      }}
    >
      {children}
    </span>
  )
}
