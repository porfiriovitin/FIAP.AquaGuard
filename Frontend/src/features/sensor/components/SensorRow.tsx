import { TrendingUp, TrendingDown, Minus, Radio } from 'lucide-react'
import type { SensorStatus } from '../data/sensors'
import { resolveTrendColor, formatTrend } from '../utils/trend'

interface Props {
  name:      string
  location:  string
  level:     number        // water level in meters
  trend:     number        // rate of change in m/h; positive = rising, negative = falling
  status?:   SensorStatus
  isLast?:   boolean
  onSelect?: () => void
}

function TrendIcon({ value }: { value: number }) {
  if (value > 0) return <TrendingUp size={9} />
  if (value < 0) return <TrendingDown size={9} />
  return <Minus size={9} />
}

export function SensorRow({ name, location, level, trend, status = 'online', isLast = false, onSelect }: Props) {
  return (
    <div
      className={`flex items-center justify-between px-4 py-4 cursor-pointer
                  active:bg-[var(--bg-sunken)] transition-colors duration-75
                  ${!isLast ? 'border-b border-[var(--ink-100)]' : ''}`}
      style={{ opacity: status === 'offline' ? 0.6 : 1 }}
      onClick={onSelect}
    >
      {/* Left: sensor icon + identification */}
      <div className="flex items-center gap-4">
        <div className="size-10 rounded-[var(--radius-sm)] flex items-center justify-center shrink-0">
          <Radio size={16} className="text-[var(--fg-muted)]" />
        </div>
        <div className="flex flex-col">
          <span
            className="text-[16px] text-[var(--navy-900)] leading-5"
            style={{ fontFamily: 'var(--font-sans)' }}
          >
            {name}
          </span>
          <span
            className="text-[12px] text-[var(--fg-muted)]"
            style={{ fontFamily: 'var(--font-mono)' }}
          >
            {location}
          </span>
        </div>
      </div>

      {/* Right: current level + rate of change */}
      <div className="flex flex-col items-end gap-0.5 shrink-0">
        <span
          className="text-[16px] font-medium text-[var(--navy-900)] tabular-nums"
          style={{ fontFamily: 'var(--font-mono)' }}
        >
          {level.toFixed(2).replace('.', ',')}m
        </span>
        <div
          className="flex items-center gap-1"
          style={{ color: resolveTrendColor(trend) }}
        >
          <TrendIcon value={trend} />
          <span
            className="text-[11px] font-medium tabular-nums"
            style={{ fontFamily: 'var(--font-mono)' }}
          >
            {formatTrend(trend)}
          </span>
        </div>
      </div>
    </div>
  )
}
