import type { RiskLevel } from '../../../shared/components/ui/RiskBadge'
import { RISK_COLOR, RISK_LABEL_LONG, calculateFillPercent } from '../utils/riskStyle'

interface Props {
  level:        RiskLevel
  riverName:    string
  stationCode:  string
  currentLevel: number
  limitLevel:   number
  onSelect?:    () => void
}

export function RiskCardCompact({ level, riverName, stationCode, currentLevel, limitLevel, onSelect }: Props) {
  const color       = RISK_COLOR[level]
  const label       = RISK_LABEL_LONG[level]
  const fillPercent = calculateFillPercent(currentLevel, limitLevel)

  return (
    <article
      className="bg-[var(--bg-elevated)] border border-[var(--border)] rounded-[var(--radius-md)]
                 overflow-hidden shadow-[var(--shadow-xs)] cursor-pointer
                 active:scale-[0.99] active:brightness-95 transition-transform duration-100"
      onClick={onSelect}
    >
      {/* Colored header */}
      <div
        className="flex items-center px-3 py-2"
        style={{ backgroundColor: color }}
      >
        <div className="flex items-center gap-1.5">
          <span className="size-1.5 rounded-full bg-white shrink-0" />
          <span
            className="text-[10px] font-medium tracking-[0.05em] text-white uppercase"
            style={{ fontFamily: 'var(--font-mono)' }}
          >
            {label}
          </span>
        </div>
      </div>

      {/* Body */}
      <div className="px-3 pt-3 pb-3 flex flex-col">
        <span
          className="text-[11px] text-[var(--fg-subtle)] leading-tight"
          style={{ fontFamily: 'var(--font-mono)' }}
        >
          {stationCode}
        </span>
        <span
          className="text-[14px] font-semibold leading-snug text-[var(--navy-900)] line-clamp-1"
          style={{ fontFamily: 'var(--font-sans)' }}
        >
          {riverName}
        </span>

        {/* Horizontal fill gauge */}
        <div className="mt-2 mb-1.5 h-1 rounded-full bg-[var(--ink-100)] overflow-hidden">
          <div
            className="h-full rounded-full"
            style={{ width: `${fillPercent}%`, backgroundColor: color }}
          />
        </div>

        {/* Level values */}
        <div
          className="flex items-baseline gap-1 text-[12px]"
          style={{ fontFamily: 'var(--font-mono)' }}
        >
          <span className="font-semibold text-[var(--navy-900)]">
            {currentLevel.toFixed(2).replace('.', ',')}m
          </span>
          <span className="text-[var(--fg-muted)]">
            / {limitLevel.toFixed(2).replace('.', ',')}m
          </span>
        </div>
      </div>
    </article>
  )
}
