import { ChevronRight } from 'lucide-react'
import { useNavigate } from 'react-router-dom'
import { RiskCardCompact } from '../features/risk/components/RiskCardCompact'
import { RISKS, isUrgent } from '../features/risk/data/risks'
import { MapView } from '../features/map/components/MapView'
import { SensorRow } from '../features/sensor/components/SensorRow'
import { SENSORS } from '../features/sensor/data/sensors'

interface Props {
  onSelectSensor: (id: string) => void
  onSelectRisk:   (id: string) => void
}

export function HomePage({ onSelectSensor, onSelectRisk }: Props) {
  const navigate    = useNavigate()
  const urgentRisks = RISKS.filter(r => isUrgent(r.level))

  return (
    <div className="flex flex-col gap-10 px-5 pt-5 pb-6 max-w-screen-xl mx-auto">

      {/* Section 1: active risk alerts */}
      <section className="flex flex-col gap-3">
        <header className="flex flex-col gap-1">
          <div className="flex items-baseline gap-2">
            <span
              className="text-[12px] font-semibold tracking-[0.12em] uppercase text-[var(--cyan-600)]"
              style={{ fontFamily: 'var(--font-sans)' }}
            >
              RISCOS ATIVOS
            </span>
            {/* urgentRisks.length > 0 && (
              <span
                className="text-[12px] font-semibold"
                style={{ fontFamily: 'var(--font-mono)', color: 'var(--risk-critical)' }}
              >
                {urgentRisks.length} ativos
              </span>
            ) */}
          </div>
          <p
            className="text-[15px] text-[var(--fg-muted)]"
            style={{ fontFamily: 'var(--font-sans)' }}
          >
            Estações com previsão de transbordamento
          </p>
        </header>
        {
        <div className="grid grid-cols-2 gap-3">
          {urgentRisks.slice(0, 4).map(r => (
            <RiskCardCompact
              key={r.id}
              level={r.level}
              riverName={r.riverName}
              stationCode={r.stationCode}
              currentLevel={r.currentLevel}
              limitLevel={r.limitLevel}
              onSelect={() => onSelectRisk(r.id)}
            />
          ))}
        </div>
        }

        <div className="flex justify-end px-1">
          <button
            type="button"
            className="flex items-center gap-1 text-[var(--accent)] active:opacity-70
                       transition-opacity duration-100"
            onClick={() => navigate('/riscos')}
          >
            <span
              className="text-[12px] font-semibold tracking-[0.06em] uppercase"
              style={{ fontFamily: 'var(--font-sans)' }}
            >
              VER TODOS OS RISCOS
            </span>
            <ChevronRight size={12} />
          </button>
        </div>
      </section>

      {/* Section 2: territorial map */}
      <section className="flex flex-col gap-3">
        <header className="flex flex-col gap-1">
          <span
            className="text-[12px] font-semibold tracking-[0.12em] uppercase text-[var(--cyan-600)]"
            style={{ fontFamily: 'var(--font-sans)' }}
          >
            MAPA DE RISCOS
          </span>
          <p
            className="text-[15px] text-[var(--fg-muted)]"
            style={{ fontFamily: 'var(--font-sans)' }}
          >
            Visão geográfica da situação atual na região monitorada
          </p>
        </header>
        <MapView useDeviceLocation />
      </section>

      {/* Section 3: recent telemetry from monitoring stations */}
      <section className="flex flex-col gap-3">
        <header className="flex flex-col gap-1">
          <div className="flex items-baseline gap-2">
            <span
              className="text-[12px] font-semibold tracking-[0.12em] uppercase text-[var(--cyan-600)]"
              style={{ fontFamily: 'var(--font-sans)' }}
            >
              TELEMETRIA RECENTE
            </span>
            
          </div>
          <p
            className="text-[15px] text-[var(--fg-muted)]"
            style={{ fontFamily: 'var(--font-sans)' }}
          >
            Últimas medições registradas
          </p>
        </header>
        <div
          className="bg-[var(--bg-elevated)] border border-[var(--border)]
                     rounded-[12px] shadow-[var(--shadow-xs)] overflow-hidden"
        >
          {SENSORS.map((s, i) => (
            <SensorRow
              key={s.id}
              name={s.stationId}
              location={s.location}
              level={s.currentLevel}
              trend={s.trend}
              status={s.status}
              isLast={i === SENSORS.length - 1}
              onSelect={() => onSelectSensor(s.id)}
            />
          ))}
        </div>
      </section>

    </div>
  )
}
