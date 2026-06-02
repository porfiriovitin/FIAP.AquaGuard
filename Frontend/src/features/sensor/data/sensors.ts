export type SensorStatus = 'online' | 'unstable' | 'offline'

export interface SensorEntry {
  id: string
  stationId: string        // ex: "P-12"
  location: string         // ex: "Marginal Pinheiros"
  status: SensorStatus
  currentLevel: number     // m
  trend: number            // m/h
  trendLabel?: string      // overrides computed trend text (ex: "Latência Alta", "--")
  lastReadingLabel?: string // ex: "Há 2h" — only for offline sensors
}

export function isConnected(status: SensorStatus): boolean {
  return status === 'online' || status === 'unstable'
}

export const SENSORS: SensorEntry[] = [
  {
    id: 'p-12',
    stationId: 'P-12',
    location: 'Marginal Pinheiros',
    status: 'online',
    currentLevel: 3.15,
    trend: 0.42,
  },
  {
    id: 't-04',
    stationId: 'T-04',
    location: 'Rio Tietê – Ponte das Bandeiras',
    status: 'unstable',
    currentLevel: 1.20,
    trend: 0.08,
    trendLabel: 'Latência Alta',
  },
  {
    id: 'g-01',
    stationId: 'G-01',
    location: 'Represa Guarapiranga',
    status: 'offline',
    currentLevel: 2.85,
    trend: 0,
    lastReadingLabel: 'Há 2h',
  },
]

export const CONNECTED_COUNT = SENSORS.filter(s => isConnected(s.status)).length

export interface SensorDetail {
  battery:     number
  signalDbm:   number
  coordinates: string            // DMS — para exibição
  coords:      [number, number]  // [lng, lat] decimal — para Mapbox
  telemetry:   Array<{ time: string; level: number; trend: number }>
}

export const SENSOR_DETAILS: Partial<Record<string, SensorDetail>> = {
  'p-12': {
    battery:     84,
    signalDbm:   -82,
    coordinates: "23°32'14\"S  46°38'08\"W",
    coords:      [-46.6356, -23.5372],
    telemetry: [
      { time: '14:45', level: 3.15, trend:  0.42 },
      { time: '14:30', level: 2.97, trend:  0.35 },
      { time: '14:15', level: 2.80, trend:  0.18 },
      { time: '14:00', level: 2.65, trend:  0.12 },
    ],
  },
  't-04': {
    battery:     61,
    signalDbm:   -94,
    coordinates: "23°31'48\"S  46°37'22\"W",
    coords:      [-46.6228, -23.5300],
    telemetry: [
      { time: '14:45', level: 1.20, trend:  0.08 },
      { time: '14:30', level: 1.15, trend:  0.05 },
      { time: '14:15', level: 1.10, trend:  0.04 },
      { time: '14:00', level: 1.06, trend:  0.02 },
    ],
  },
  'g-01': {
    battery:     12,
    signalDbm:   -110,
    coordinates: "23°42'11\"S  46°41'55\"W",
    coords:      [-46.6986, -23.7031],
    telemetry: [
      { time: '12:30', level: 2.85, trend:  0.00 },
      { time: '12:15', level: 2.85, trend:  0.00 },
      { time: '12:00', level: 2.84, trend: -0.01 },
      { time: '11:45', level: 2.84, trend: -0.01 },
    ],
  },
}
