// Threshold above which a rising trend is classified as a critical-level rate of change.
// Based on Defesa Civil São Paulo operational guidelines (>0.1 m/h = alert state).
export const CRITICAL_RISE_THRESHOLD = 0.1

export function resolveTrendColor(trend: number): string {
  if (trend > CRITICAL_RISE_THRESHOLD) return 'var(--risk-critical)'
  if (trend > 0)                       return 'var(--risk-high)'
  if (trend < 0)                       return 'var(--risk-low)'
  return 'var(--fg-muted)'
}

export function formatTrend(value: number): string {
  const sign = value > 0 ? '+' : ''
  return `${sign}${value.toFixed(2).replace('.', ',')}m/h`
}
