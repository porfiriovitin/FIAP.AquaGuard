import { NavLink } from 'react-router-dom'
import { NAV_TABS } from '../../config/navTabs'

interface Props {
  alertPaths?: Set<string>
}

export function BottomNavBar({ alertPaths }: Props) {
  return (
    <nav
      className="fixed bottom-0 inset-x-0 z-50 bg-[var(--bg-elevated)] border-t border-[var(--border)]
                 shadow-[0_-1px_0_var(--border)]"
      style={{ paddingBottom: 'env(safe-area-inset-bottom)' }}
    >
      <div className="h-[75px] flex items-stretch">
        {NAV_TABS.map(({ path, label, icon: Icon, canAlert, iconSize = 19 }) => (
          <NavLink
            key={path}
            to={path}
            end={path === '/app'}
            aria-label={label}
            className="flex-1 flex flex-col items-center justify-center gap-1 relative
                       active:bg-[var(--bg-sunken)] transition-colors duration-100"
          >
            {({ isActive }) => (
              <>
                <div className="relative">
                  <Icon
                    size={iconSize}
                    strokeWidth={isActive ? 2.5 : 1.75}
                    style={{ color: isActive ? 'var(--navy-900)' : 'var(--fg-muted)' }}
                  />
                  {canAlert && alertPaths?.has(path) && (
                    <span
                      aria-hidden="true"
                      className="absolute -top-0.5 -right-0.5 size-2 rounded-full bg-[var(--risk-critical)]"
                    />
                  )}
                </div>
                <span
                  className="text-[9px] font-medium tracking-[0.05em]"
                  style={{
                    fontFamily: 'var(--font-mono)',
                    color: isActive ? 'var(--navy-900)' : 'var(--fg-muted)',
                  }}
                >
                  {label}
                </span>
              </>
            )}
          </NavLink>
        ))}
      </div>
    </nav>
  )
}
