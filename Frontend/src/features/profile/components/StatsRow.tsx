interface Props {
  alertCount:  number
  sensorCount: number
}

const labelCls = "text-[12px] font-bold tracking-[0.12em] uppercase text-[var(--fg-muted)]"
const valueCls = "text-[13px] font-medium text-[var(--navy-900)]"

export function StatsRow({ alertCount, sensorCount }: Props) {
  return (
    <div
      className="grid grid-cols-2 bg-[var(--bg-elevated)] border border-[var(--border)]
                 rounded-[var(--radius-md)] shadow-[var(--shadow-xs)] px-2"
    >
      <div className="flex flex-col items-center gap-1 py-2 border-r border-[var(--border)]">
        <span className={labelCls} style={{ fontFamily: 'var(--font-sans)' }}>ALERTAS</span>
        <span className={valueCls} style={{ fontFamily: 'var(--font-mono)' }}>{alertCount}</span>
      </div>
      <div className="flex flex-col items-center gap-1 py-2">
        <span className={labelCls} style={{ fontFamily: 'var(--font-sans)' }}>SENSORES</span>
        <span className={valueCls} style={{ fontFamily: 'var(--font-mono)' }}>{sensorCount}</span>
      </div>
    </div>
  )
}
