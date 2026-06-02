import { ChevronRight } from 'lucide-react'
import { useNavigate } from 'react-router-dom'
import { RiskCardCompact } from '../features/risk/components/RiskCardCompact'
import { isUrgent } from '../features/risk/data/risks'
import { useRisks } from '../features/risk/hooks/useRisks'
import { MapView } from '../features/map/components/MapView'
import { getFloodLayerCallback } from '../features/map/services/mapService'
import { SatelliteCard } from '../features/satellite/components/SatelliteCard'
import { SensorRow } from '../features/sensor/components/SensorRow'
import { useSensors } from '../features/sensor/hooks/useSensors'
import { SectionEyebrow } from '../shared/components/ui'

interface Props {
  onSelectSensor: (id: string) => void
  onSelectRisk:   (id: string) => void
}

export function HomePage({ onSelectSensor, onSelectRisk }: Props) {
  const navigate = useNavigate()

  const { data: risks   = [] } = useRisks()
  const { data: sensors = [] } = useSensors()

  const urgentRisks = risks.filter(r => isUrgent(r.level))

  return (
    <div className="flex flex-col gap-10 px-5 pt-5 pb-6 max-w-screen-xl mx-auto">

      {/* Section 1: active risk alerts */}
      <section className="flex flex-col gap-3">
        <header className="flex flex-col gap-1">
          <div className="flex items-baseline gap-2">
            <SectionEyebrow>RISCOS ATIVOS</SectionEyebrow>
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
        <div className="grid grid-cols-2 gap-3">
          {urgentRisks.slice(0, 3).map(r => (
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

          {/* 4th cell: ver todos button */}
          <div className="flex items-center justify-center">
            <button
              type="button"
              onClick={() => navigate('/app/risks')}
              className="w-[75%] flex flex-col items-center justify-center gap-1.5 py-5
                         bg-[var(--bg-elevated)] border border-[var(--border)]
                         rounded-[var(--radius-md)] shadow-[var(--shadow-xs)]
                         active:brightness-95 transition-[filter] duration-100"
            >
              <ChevronRight size={16} style={{ color: 'var(--accent)' }} />
              <span
                className="text-[10px] font-semibold tracking-[0.06em] uppercase"
                style={{ fontFamily: 'var(--font-sans)', color: 'var(--accent)' }}
              >
                VER TODOS
              </span>
            </button>
          </div>
        </div>
      </section>

      {/* Section 2: satellite coverage */}
      <section className="flex flex-col gap-3">
        <header className="flex flex-col gap-1">
          <SectionEyebrow>COBERTURA ORBITAL</SectionEyebrow>
          <p
            className="text-[15px] text-[var(--fg-muted)]"
            style={{ fontFamily: 'var(--font-sans)' }}
          >
            Imageamento SAR ativo via satélite Sentinel-1
          </p>
        </header>
        <SatelliteCard />
        <SatelliteCard
          name="Sentinel-1D"
          scheme="mint"
          stats={{ orbit: 847, altitude: '706 km', band: 'Modo IW' }}
        />
      </section>

      {/* Section 3: territorial map */}
      <section className="flex flex-col gap-3">
        <header className="flex flex-col gap-1">
          <SectionEyebrow>MAPA DE RISCOS</SectionEyebrow>
          <p
            className="text-[15px] text-[var(--fg-muted)]"
            style={{ fontFamily: 'var(--font-sans)' }}
          >
            Visão geográfica da situação atual na região monitorada
          </p>
        </header>
        <MapView
          center={[-46.6333, -23.5505]}
          zoom={10.75}
          onLoad={getFloodLayerCallback()}
        />
      </section>

      {/* Section 4: recent telemetry from monitoring stations */}
      <section className="flex flex-col gap-3">
        <header className="flex flex-col gap-1">
          <div className="flex items-baseline gap-2">
            <SectionEyebrow>TELEMETRIA RECENTE</SectionEyebrow>
            
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
          {sensors.map((s, i) => (
            <SensorRow
              key={s.id}
              name={s.stationId}
              location={s.location}
              level={s.currentLevel}
              trend={s.trend}
              status={s.status}
              isLast={i === sensors.length - 1}
              onSelect={() => onSelectSensor(s.id)}
            />
          ))}
        </div>
      </section>

    </div>
  )
}
