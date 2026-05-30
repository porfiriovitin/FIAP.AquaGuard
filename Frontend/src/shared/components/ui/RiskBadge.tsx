export type RiskLevel = 'critical' | 'high' | 'moderate' | 'low' | 'normal'

interface Props {
  level: RiskLevel
  className?: string
}

// Maps each risk level to its design-token color and PT-BR display label.
// Labels are intentionally in Portuguese — this is domain-specific terminology
// defined by Brazilian civil defense (Defesa Civil) standards.
const RISK_DISPLAY: Record<RiskLevel, { color: string; label: string }> = {
  critical: { color: 'var(--risk-critical)', label: 'RISCO CRÍTICO'  },
  high:     { color: 'var(--risk-high)',     label: 'RISCO ALTO'     },
  moderate: { color: 'var(--risk-moderate)', label: 'RISCO MODERADO' },
  low:      { color: 'var(--risk-low)',      label: 'RISCO BAIXO'    },
  normal:   { color: 'var(--risk-normal)',   label: 'NORMAL'         },
}

export function RiskBadge({ level, className = '' }: Props) {
  const { color, label } = RISK_DISPLAY[level]
  return (
    <span
      className={`inline-flex items-center gap-1.5 px-2.5 py-1 rounded-full text-white text-[10px] font-medium tracking-[0.05em] uppercase ${className}`}
      style={{ backgroundColor: color, fontFamily: 'var(--font-mono)' }}
    >
      <span className="size-1.5 rounded-full bg-white shrink-0" />
      {label}
    </span>
  )
}
