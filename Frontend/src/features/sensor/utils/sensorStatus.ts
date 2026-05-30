import { type ComponentType } from 'react'
import { Wifi, WifiOff, AlertTriangle, type LucideProps } from 'lucide-react'
import type { SensorStatus } from '../data/sensors'

export interface SensorStatusStyle {
  barColor:    string
  badgeColor:  string
  label:       string
  Icon:        ComponentType<LucideProps>
  actionLabel: string
  actionColor: string
  opacity:     number
}

export const STATUS_STYLE: Record<SensorStatus, SensorStatusStyle> = {
  online: {
    barColor:    'var(--risk-low)',
    badgeColor:  'var(--risk-low)',
    label:       'ONLINE',
    Icon:        Wifi,
    actionLabel: 'VER DADOS',
    actionColor: 'var(--accent)',
    opacity:     1,
  },
  unstable: {
    barColor:    'var(--risk-moderate)',
    badgeColor:  'var(--risk-moderate)',
    label:       'INSTÁVEL',
    Icon:        AlertTriangle,
    actionLabel: 'VER DADOS',
    actionColor: 'var(--accent)',
    opacity:     1,
  },
  offline: {
    barColor:    'var(--ink-300)',
    badgeColor:  '#ba1a1a',
    label:       'DESCONECTADO',
    Icon:        WifiOff,
    actionLabel: 'VER LOGS',
    actionColor: 'var(--fg-muted)',
    opacity:     0.75,
  },
}
