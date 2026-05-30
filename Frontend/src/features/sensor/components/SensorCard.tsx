import type { SensorEntry } from '../data/sensors'
import { STATUS_STYLE } from '../utils/sensorStatus'

interface Props extends Pick<SensorEntry, 'stationId' | 'location' | 'status'> {
  onSelect?: () => void
}

export function SensorCard({ stationId, location, status, onSelect }: Props) {
  const style     = STATUS_STYLE[status]
  const isOffline = status === 'offline'

  return (
    <article
      className="bg-[var(--bg-elevated)] border border-[var(--border)] rounded-[var(--radius-md)]
                 overflow-hidden shadow-[var(--shadow-xs)] cursor-pointer
                 active:brightness-95 transition-[filter] duration-100"
      style={{ opacity: style.opacity }}
      onClick={onSelect}
    >
      {/* 3px status accent bar */}
      <div className="h-[3px] w-full" style={{ backgroundColor: style.barColor }} />

      <div className="flex flex-col gap-2 p-4">

        {/* Header: station ID + location · status badge */}
        <div className="flex items-start justify-between gap-3">
          <div className="flex flex-col gap-1">
            <span
              className="text-[12px] font-bold tracking-[0.12em] uppercase"
              style={{ fontFamily: 'var(--font-sans)', color: 'var(--fg-muted)' }}
            >
              SENSOR {stationId}
            </span>
            <span
              className="text-[15px]"
              style={{
                fontFamily: 'var(--font-sans)',
                color: isOffline ? 'var(--fg-muted)' : 'var(--navy-900)',
              }}
            >
              {location}
            </span>
          </div>

          <div
            className="flex items-center gap-1.5 shrink-0 pt-0.5"
            style={{ color: style.badgeColor }}
          >
            <style.Icon size={13} />
            <span
              className="text-[11px] font-medium uppercase"
              style={{ fontFamily: 'var(--font-mono)' }}
            >
              {style.label}
            </span>
          </div>
        </div>

        {/* Action label — card is the interactive element; no nested button */}
        <span
          className="text-[13px] font-medium mt-1"
          style={{
            fontFamily: 'var(--font-mono)',
            color: style.actionColor,
          }}
        >
          {style.actionLabel}
        </span>

      </div>
    </article>
  )
}
