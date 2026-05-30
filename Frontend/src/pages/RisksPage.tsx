import { RISKS, isUrgent } from '../features/risk/data/risks'
import { RiskCard } from '../features/risk/components/RiskCard'

interface Props {
  onSelectRisk: (id: string) => void
}

export function RisksPage({ onSelectRisk }: Props) {
  const activeCount = RISKS.filter(r => isUrgent(r.level)).length

  return (
    <div className="flex flex-col gap-4 px-5 pt-5 pb-6 max-w-screen-xl mx-auto">

      <header className="flex items-baseline gap-2">
        <span
          className="text-[12px] font-semibold tracking-[0.12em] uppercase text-[var(--cyan-600)]"
          style={{ fontFamily: 'var(--font-sans)' }}
        >
          RISCOS ATUAIS
        </span>
        {activeCount > 0 && (
          <span
            className="text-[12px] font-semibold"
            style={{ fontFamily: 'var(--font-mono)', color: 'var(--risk-critical)' }}
          >
            {activeCount} ativo{activeCount !== 1 ? 's' : ''}
          </span>
        )}
      </header>

      <section className="flex flex-col gap-4">
        {RISKS.map(risk => (
          <RiskCard
            key={risk.id}
            level={risk.level}
            riverName={risk.riverName}
            stationName={risk.stationName}
            stationCode={risk.stationCode}
            currentLevel={risk.currentLevel}
            limitLevel={risk.limitLevel}
            trend={risk.trend}
            trendLabel={risk.trendLabel}
            lastUpdated={risk.lastUpdated}
            actionWindow={risk.actionWindow}
            onSelect={() => onSelectRisk(risk.id)}
          />
        ))}
      </section>

    </div>
  )
}
