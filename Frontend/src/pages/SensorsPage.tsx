import { isConnected } from '../features/sensor/data/sensors'
import { useSensors } from '../features/sensor/hooks/useSensors'
import { SensorCard } from '../features/sensor/components/SensorCard'
import { LoadingSpinner, SectionEyebrow } from '../shared/components/ui'

interface Props {
  onSelectSensor: (id: string) => void
}

export function SensorsPage({ onSelectSensor }: Props) {
  const { data: sensors = [], isLoading } = useSensors()

  if (isLoading) {
    return (
      <div className="flex items-center justify-center min-h-[40vh]">
        <LoadingSpinner size="md" />
      </div>
    )
  }

  const connectedCount = sensors.filter(s => isConnected(s.status)).length

  return (
    <div className="flex flex-col gap-6 px-5 pt-5 pb-6 max-w-screen-xl mx-auto">

      <header className="flex flex-col gap-1">
        <div className="flex items-baseline gap-2">
          <SectionEyebrow>REDE DE SENSORES</SectionEyebrow>
          {connectedCount > 0 && (
            <span
              className="text-[12px] font-semibold"
              style={{ fontFamily: 'var(--font-mono)', color: 'var(--risk-low)' }}
            >
              {connectedCount} ativos
            </span>
          )}
        </div>
        <p
          className="text-[15px] text-[var(--fg-muted)]"
          style={{ fontFamily: 'var(--font-sans)' }}
        >
          Monitoramento hidrológico em tempo real
        </p>
      </header>

      <section className="flex flex-col gap-4">
        {sensors.map(sensor => (
          <SensorCard
            key={sensor.id}
            stationId={sensor.stationId}
            location={sensor.location}
            status={sensor.status}
            onSelect={() => onSelectSensor(sensor.id)}
          />
        ))}
      </section>

    </div>
  )
}
