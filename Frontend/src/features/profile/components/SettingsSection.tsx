import { type ReactNode } from 'react'

interface Props {
  title?: string
  children: ReactNode
}

export function SettingsSection({ title, children }: Props) {
  return (
    <div
      className={`bg-[var(--bg-elevated)] border border-[var(--border)]
                 rounded-[var(--radius-md)] shadow-[var(--shadow-xs)] overflow-hidden
                 ${!title ? '[&>*:first-child]:border-t-0' : ''}`}
    >
      {title && (
        <div className="px-4 pt-4 pb-2">
          <span
            className="text-[12px] font-bold tracking-[0.12em] uppercase text-[var(--fg-muted)]"
            style={{ fontFamily: 'var(--font-sans)' }}
          >
            {title}
          </span>
        </div>
      )}
      {children}
    </div>
  )
}
