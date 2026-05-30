import type { RiskLevel } from '../../../shared/components/ui/RiskBadge'

export interface RiskEntry {
  id: string
  level: RiskLevel
  riverName: string
  stationName: string
  stationCode: string
  currentLevel: number
  limitLevel: number
  trend: number
  trendLabel?: string
  lastUpdated: string
  actionWindow?: string
}

export function isUrgent(level: RiskLevel): boolean {
  return level === 'critical' || level === 'high' || level === 'moderate'
}

export const RISKS: RiskEntry[] = [
  {
    id: 'rh-204',
    level: 'critical',
    riverName: 'Rio Tietê',
    stationName: 'Ponte dos Remédios',
    stationCode: 'RH-204',
    currentLevel: 4.82,
    limitLevel: 5.10,
    trend: 0.18,
    lastUpdated: '2 min atrás',
    actionWindow: '2 h 40 min',
  },
  {
    id: 'rh-205',
    level: 'high',
    riverName: 'Rio Pinheiros',
    stationName: 'Marginal Pinheiros',
    stationCode: 'RH-205',
    currentLevel: 5.82,
    limitLevel: 6.50,
    trend: 0.05,
    lastUpdated: '5 min atrás',
    actionWindow: '4 h 15 min',
  },
  {
    id: 'rh-118',
    level: 'moderate',
    riverName: 'Rio Tamanduateí',
    stationName: 'Vila Prudente',
    stationCode: 'RH-118',
    currentLevel: 3.40,
    limitLevel: 4.20,
    trend: 0.03,
    lastUpdated: '12 min atrás',
    actionWindow: '8 h 00 min',
  },
  {
    id: 'rh-105',
    level: 'low',
    riverName: 'Represa Billings',
    stationName: 'Setor Sul',
    stationCode: 'RH-105',
    currentLevel: 2.10,
    limitLevel: 6.50,
    trend: -0.02,
    lastUpdated: '1 hora atrás',
  },
  {
    id: 'rh-08',
    level: 'normal',
    riverName: 'Córrego Ipiranga',
    stationName: 'Parque da Independência',
    stationCode: 'RH-08',
    currentLevel: 0.85,
    limitLevel: 2.50,
    trend: 0,
    trendLabel: 'Estável',
    lastUpdated: '3 horas atrás',
  },
]

export interface RiskDetail {
  evolution: Array<{
    label: string
    level: number
    kind:  'predicted' | 'now' | 'past'
  }>
  telemetry: {
    flowRate: number   // m³/s
    rainfall: number   // mm em 24h
  }
}

export const RISK_DETAILS: Partial<Record<string, RiskDetail>> = {
  'rh-204': {
    evolution: [
      { label: 'Em 1 hora (Previsto)', level: 5.00, kind: 'predicted' },
      { label: 'Agora',                level: 4.82, kind: 'now'       },
      { label: 'Há 1 hora',            level: 4.64, kind: 'past'      },
      { label: 'Há 2 horas',           level: 4.46, kind: 'past'      },
    ],
    telemetry: { flowRate: 850, rainfall: 112 },
  },
  'rh-205': {
    evolution: [
      { label: 'Em 1 hora (Previsto)', level: 5.87, kind: 'predicted' },
      { label: 'Agora',                level: 5.82, kind: 'now'       },
      { label: 'Há 1 hora',            level: 5.77, kind: 'past'      },
      { label: 'Há 2 horas',           level: 5.72, kind: 'past'      },
    ],
    telemetry: { flowRate: 620, rainfall: 78 },
  },
  'rh-118': {
    evolution: [
      { label: 'Em 1 hora (Previsto)', level: 3.43, kind: 'predicted' },
      { label: 'Agora',                level: 3.40, kind: 'now'       },
      { label: 'Há 1 hora',            level: 3.37, kind: 'past'      },
      { label: 'Há 2 horas',           level: 3.34, kind: 'past'      },
    ],
    telemetry: { flowRate: 310, rainfall: 45 },
  },
  'rh-105': {
    evolution: [
      { label: 'Em 1 hora (Previsto)', level: 2.08, kind: 'predicted' },
      { label: 'Agora',                level: 2.10, kind: 'now'       },
      { label: 'Há 1 hora',            level: 2.12, kind: 'past'      },
      { label: 'Há 2 horas',           level: 2.14, kind: 'past'      },
    ],
    telemetry: { flowRate: 95, rainfall: 12 },
  },
  'rh-08': {
    evolution: [
      { label: 'Em 1 hora (Previsto)', level: 0.85, kind: 'predicted' },
      { label: 'Agora',                level: 0.85, kind: 'now'       },
      { label: 'Há 1 hora',            level: 0.85, kind: 'past'      },
      { label: 'Há 2 horas',           level: 0.84, kind: 'past'      },
    ],
    telemetry: { flowRate: 18, rainfall: 3 },
  },
}
