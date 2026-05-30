interface Props {
  label: string
  value: string
  unit?: string
  // Accepts a CSS color value or variable reference, e.g. 'var(--risk-critical)'
  valueColor?: string
}

export function StatTile({ label, value, unit, valueColor }: Props) {
  return (
    <div className="flex flex-col gap-1">
      <span
        className="text-[9px] uppercase tracking-[0.05em] text-[var(--fg-muted)]"
        style={{ fontFamily: 'var(--font-mono)' }}
      >
        {label}
      </span>
      <div className="flex items-baseline gap-1" style={{ fontFamily: 'var(--font-mono)' }}>
        <span
          className="text-[16px] font-medium text-[var(--fg)]"
          style={valueColor ? { color: valueColor } : undefined}
        >
          {value}
        </span>
        {unit && (
          <span className="text-[12px] text-[var(--fg-muted)]">{unit}</span>
        )}
      </div>
    </div>
  )
}
