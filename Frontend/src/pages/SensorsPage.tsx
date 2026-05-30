import { SENSORS, CONNECTED_COUNT } from '../features/sensor/data/sensors'
import { SensorCard } from '../features/sensor/components/SensorCard'

interface Props {
  onSelectSensor: (id: string) => void
}

export function SensorsPage({ onSelectSensor }: Props) {
  return (
    <div className="flex flex-col gap-6 px-5 pt-5 pb-6 max-w-screen-xl mx-auto">

      <header className="flex flex-col gap-1">
        <div className="flex items-baseline gap-2">
          <span
            className="text-[12px] font-semibold tracking-[0.12em] uppercase text-[var(--cyan-600)]"
            style={{ fontFamily: 'var(--font-sans)' }}
          >
            REDE DE SENSORES
          </span>
          {CONNECTED_COUNT > 0 && (
            <span
              className="text-[12px] font-semibold"
              style={{ fontFamily: 'var(--font-mono)', color: 'var(--risk-low)' }}
            >
              {CONNECTED_COUNT} ativos
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
        {SENSORS.map(sensor => (
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
