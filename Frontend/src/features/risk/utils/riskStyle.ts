import type { RiskLevel } from '../../../shared/components/ui/RiskBadge'

export const RISK_COLOR: Record<RiskLevel, string> = {
  critical: 'var(--risk-critical)',
  high:     'var(--risk-high)',
  moderate: 'var(--risk-moderate)',
  low:      'var(--risk-low)',
  normal:   'var(--risk-normal)',
}

// Labels are intentionally in Portuguese — domain terminology from Defesa Civil
export const RISK_LABEL_LONG: Record<RiskLevel, string> = {
  critical: 'RISCO CRÍTICO',
  high:     'RISCO ALTO',
  moderate: 'RISCO MODERADO',
  low:      'RISCO BAIXO',
  normal:   'SEM RISCO',
}

export function calculateFillPercent(currentLevel: number, limitLevel: number): number {
  if (limitLevel <= 0) return 0
  return Math.min(100, Math.round((currentLevel / limitLevel) * 100))
}
