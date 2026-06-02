import { isUrgent } from '../features/risk/data/risks'
import { useRisks } from '../features/risk/hooks/useRisks'
import { RiskCard } from '../features/risk/components/RiskCard'
import { LoadingSpinner, SectionEyebrow } from '../shared/components/ui'

interface Props {
  onSelectRisk: (id: string) => void
}

export function RisksPage({ onSelectRisk }: Props) {
  const { data: risks = [], isLoading } = useRisks()

  if (isLoading) {
    return (
      <div className="flex items-center justify-center min-h-[40vh]">
        <LoadingSpinner size="md" />
      </div>
    )
  }

  const activeCount = risks.filter(r => isUrgent(r.level)).length

  return (
    <div className="flex flex-col gap-4 px-5 pt-5 pb-6 max-w-screen-xl mx-auto">

      <header className="flex items-baseline gap-2">
        <SectionEyebrow>RISCOS ATUAIS</SectionEyebrow>
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
        {risks.map(risk => (
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
