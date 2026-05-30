import { Clock } from 'lucide-react'
import { StatTile } from '../../../shared/components/ui/StatTile'
import type { RiskLevel } from '../../../shared/components/ui/RiskBadge'
import { RISK_COLOR, RISK_LABEL_LONG, calculateFillPercent } from '../utils/riskStyle'
import { formatTrend } from '../../sensor/utils/trend'

interface Props {
  level: RiskLevel
  riverName: string
  stationName: string
  stationCode: string
  currentLevel: number
  limitLevel: number
  trend: number        // m/h; positive = rising, negative = falling
  trendLabel?: string  // overrides computed trend text (e.g. "Estável")
  lastUpdated: string
  actionWindow?: string // absent → no footer rendered
  onSelect?: () => void
}

// Per-level visual tokens. RGBA values mirror the CSS variables for cases where
// alpha-blended tints are needed (CSS color-mix() not yet universal enough).
const RISK_STYLE: Record<RiskLevel, { bg: string; footerBg: string; footerBorder: string }> = {
  critical: {
    bg:           RISK_COLOR.critical,
    footerBg:     'rgba(214,58,58,0.08)',
    footerBorder: 'rgba(214,58,58,0.15)',
  },
  high: {
    bg:           RISK_COLOR.high,
    footerBg:     'rgba(239,122,26,0.08)',
    footerBorder: 'rgba(239,122,26,0.15)',
  },
  moderate: {
    bg:           RISK_COLOR.moderate,
    footerBg:     'rgba(241,181,10,0.08)',
    footerBorder: 'rgba(241,181,10,0.15)',
  },
  low: {
    bg:           RISK_COLOR.low,
    footerBg:     'rgba(27,184,146,0.08)',
    footerBorder: 'rgba(27,184,146,0.15)',
  },
  normal: {
    bg:           RISK_COLOR.normal,
    footerBg:     'rgba(44,121,191,0.08)',
    footerBorder: 'rgba(44,121,191,0.15)',
  },
}

export function RiskCard({
  level,
  riverName,
  stationName,
  stationCode,
  currentLevel,
  limitLevel,
  trend,
  trendLabel,
  lastUpdated,
  actionWindow,
  onSelect,
}: Props) {
  // Fallback to normal style if an unknown level arrives from a future API field
  const riskStyle = RISK_STYLE[level] ?? RISK_STYLE.normal

  // Derived from source values — keeps gauge and badge always consistent with stats
  const fillPercent = calculateFillPercent(currentLevel, limitLevel)

  // The gauge fills from the bottom: `top` is set to the empty fraction so the
  // colored fill occupies (fillPercent)% of the track from the bottom up.
  const gaugeEmptyFraction = `${100 - fillPercent}%`

  // Rising → risk color, falling → recovery green, stable → neutral
  const trendColor =
    trend > 0 ? riskStyle.bg :
    trend < 0 ? 'var(--risk-low)' :
    'var(--fg-muted)'

  return (
    <article
      className="bg-[var(--bg-elevated)] border border-[var(--border)] rounded-[var(--radius-md)]
                 overflow-hidden shadow-[var(--shadow-xs)] flex flex-col cursor-pointer
                 active:scale-[0.99] active:brightness-95 transition-transform duration-100"
      onClick={onSelect}
    >
      {/* Colored header strip */}
      <div
        className="flex items-center justify-between px-4 py-2"
        style={{ backgroundColor: riskStyle.bg }}
      >
        <div className="flex items-center gap-1.5">
          <span className="size-1.5 rounded-full bg-white shrink-0" />
          <span
            className="text-[10px] font-medium tracking-[0.05em] text-white uppercase"
            style={{ fontFamily: 'var(--font-mono)' }}
          >
            {RISK_LABEL_LONG[level]}
          </span>
        </div>
        <span
          className="text-[10px] text-white/90"
          style={{ fontFamily: 'var(--font-mono)' }}
        >
          {lastUpdated}
        </span>
      </div>

      {/* Body: river name, vertical gauge, stats */}
      <div className="flex flex-col gap-4 px-4 pt-4 pb-8">
        <div className="flex items-start justify-between">
          <div className="flex flex-col gap-0.5">
            <span
              className="text-[20px] font-bold leading-[1.3] text-[var(--navy-900)]"
              style={{ fontFamily: 'var(--font-sans)' }}
            >
              {riverName}
            </span>
            <span
              className="text-[12px] text-[var(--fg-muted)]"
              style={{ fontFamily: 'var(--font-sans)' }}
            >
              {stationName} · {stationCode}
            </span>
          </div>

          {/* Vertical fill gauge */}
          <div className="flex flex-col items-center gap-1 shrink-0">
            <div className="w-1.5 h-16 bg-[var(--ink-100)] rounded-full overflow-hidden relative">
              <div
                className="absolute inset-x-0 bottom-0 rounded-full transition-all duration-700"
                style={{ top: gaugeEmptyFraction, backgroundColor: riskStyle.bg }}
              />
            </div>
            <span
              className="text-[10px] font-medium"
              style={{ fontFamily: 'var(--font-mono)', color: riskStyle.bg }}
            >
              {fillPercent}%
            </span>
          </div>
        </div>

        {/* Telemetry stats */}
        <div className="grid grid-cols-3 gap-2">
          <StatTile
            label="NÍVEL ATUAL"
            value={currentLevel.toFixed(2).replace('.', ',')}
            unit="m"
          />
          <StatTile
            label="LIMITE"
            value={limitLevel.toFixed(2).replace('.', ',')}
            unit="m"
          />
          <StatTile
            label="TENDÊNCIA"
            value={trendLabel ?? formatTrend(trend)}
            valueColor={trendColor}
          />
        </div>
      </div>

      {/* Footer: action window countdown — only for urgent levels */}
      {actionWindow && (
        <div
          className="flex items-center gap-2 px-4 py-2 border-t"
          style={{ backgroundColor: riskStyle.footerBg, borderColor: riskStyle.footerBorder }}
        >
          <Clock size={13} style={{ color: riskStyle.bg, flexShrink: 0 }} />
          <span
            className="text-[12px] font-semibold"
            style={{ fontFamily: 'var(--font-sans)', color: riskStyle.bg }}
          >
            Janela de ação: {actionWindow}
          </span>
        </div>
      )}
    </article>
  )
}
