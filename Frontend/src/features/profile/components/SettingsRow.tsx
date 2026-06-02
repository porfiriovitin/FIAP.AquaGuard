import { type ComponentType } from 'react'
import { ChevronRight, type LucideProps } from 'lucide-react'

type BaseProps = {
  icon: ComponentType<LucideProps>
  label: string
}

type Props =
  | (BaseProps & { variant: 'link' })
  | (BaseProps & { variant: 'toggle'; checked: boolean; onToggle: () => void })
  | (BaseProps & { variant: 'value';  valueText: string })
  | (BaseProps & { variant: 'danger'; onPress?: () => void })

function Toggle({ checked, onToggle }: { checked: boolean; onToggle: () => void }) {
  return (
    <button
      type="button"
      role="switch"
      aria-checked={checked}
      onClick={onToggle}
      className="relative shrink-0 w-[44px] h-[24px] rounded-full
                 transition-colors duration-200 active:opacity-80"
      style={{ backgroundColor: checked ? 'var(--accent)' : 'var(--ink-300)' }}
    >
      <span
        className="absolute w-[20px] h-[20px] rounded-full bg-white top-[2px]
                   transition-[left] duration-200 shadow-[0_1px_2px_rgba(0,0,0,0.15)]"
        style={{ left: checked ? '22px' : '2px' }}
      />
    </button>
  )
}

export function SettingsRow(props: Props) {
  const { icon: Icon, label } = props
  const isDanger    = props.variant === 'danger'
  const iconColor   = isDanger ? 'var(--risk-critical)' : 'var(--fg-muted)'
  const labelColor  = isDanger ? 'var(--risk-critical)' : 'var(--navy-900)'
  const labelWeight = isDanger ? 'font-bold' : 'font-normal'

  return (
    <div
      role={isDanger ? 'button' : undefined}
      tabIndex={isDanger ? 0 : undefined}
      onClick={isDanger && props.variant === 'danger' ? props.onPress : undefined}
      onKeyDown={
        isDanger && props.variant === 'danger' && props.onPress
          ? e => { if (e.key === 'Enter' || e.key === ' ') props.onPress!() }
          : undefined
      }
      className={`flex items-center justify-between px-4 py-3 border-t border-[var(--ink-100)]
                 active:bg-[var(--bg-sunken)] transition-colors duration-75
                 ${isDanger ? 'cursor-pointer' : ''}`}
    >
      {/* Leading: icon + label */}
      <div className="flex items-center gap-3">
        <Icon size={18} style={{ color: iconColor, flexShrink: 0 }} />
        <span
          className={`text-[15px] ${labelWeight}`}
          style={{ fontFamily: 'var(--font-sans)', color: labelColor }}
        >
          {label}
        </span>
      </div>

      {/* Trailing slot */}
      {props.variant === 'link' && (
        <ChevronRight size={14} style={{ color: 'var(--fg-subtle)', flexShrink: 0 }} />
      )}
      {props.variant === 'toggle' && (
        <Toggle checked={props.checked} onToggle={props.onToggle} />
      )}
      {props.variant === 'value' && (
        <span
          className="text-[13px]"
          style={{ fontFamily: 'var(--font-mono)', color: 'var(--fg-muted)' }}
        >
          {props.valueText}
        </span>
      )}
    </div>
  )
}
