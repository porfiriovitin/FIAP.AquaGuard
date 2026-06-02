import { type ComponentType } from 'react'
import { Home, Map, Bell, Radio, User, type LucideProps } from 'lucide-react'

export interface NavTab {
  path: string
  label: string
  icon: ComponentType<LucideProps>
  canAlert?: boolean
  iconSize?: number
}

export const NAV_TABS: NavTab[] = [
  { path: '/app',          label: 'INÍCIO',   icon: Home                    },
  { path: '/app/map',      label: 'MAPA',     icon: Map                     },
  { path: '/app/risks',    label: 'RISCOS',   icon: Bell,  canAlert: true   },
  { path: '/app/sensors',  label: 'SENSORES', icon: Radio, iconSize: 20     },
  { path: '/app/profile',  label: 'PERFIL',   icon: User                    },
]

// Single source of truth for animation direction — derived from nav order above
export const TAB_ORDER = NAV_TABS.map(t => t.path)
