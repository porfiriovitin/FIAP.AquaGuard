import { useEffect } from 'react'
import { motion } from 'framer-motion'
import { X, Clock, Droplet, CloudRain, AlertTriangle } from 'lucide-react'
import { RISKS, RISK_DETAILS } from '../data/risks'
import { RISK_COLOR, RISK_LABEL_LONG, calculateFillPercent } from '../utils/riskStyle'
import { formatTrend } from '../../sensor/utils/trend'

interface Props {
  riskId:  string
  onClose: () => void
}

function fmt(n: number, fractionDigits = 2): string {
  return n.toFixed(fractionDigits).replace('.', ',')
}

export function RiskDetailModal({ riskId, onClose }: Props) {
  const risk   = RISKS.find(r => r.id === riskId)
  const detail = RISK_DETAILS[riskId]

  useEffect(() => {
    const onKey = (e: KeyboardEvent) => { if (e.key === 'Escape') onClose() }
    document.addEventListener('keydown', onKey)
    document.body.style.overflow = 'hidden'
    return () => {
      document.removeEventListener('keydown', onKey)
      document.body.style.overflow = ''
    }
  }, [onClose])

  if (!risk || !detail) return null

  const color       = RISK_COLOR[risk.level]
  const fillPercent = calculateFillPercent(risk.currentLevel, risk.limitLevel)
  // Rising → risk-level color (severity), falling → recovery green, stable → muted.
  // Mirrors the rule in RiskCard.tsx to keep the same risk consistent across surfaces.
  const trendColor =
    risk.trend > 0 ? color :
    risk.trend < 0 ? 'var(--risk-low)' :
    'var(--fg-muted)'

  return (
    <motion.div
      className="fixed inset-0 z-50 flex items-center justify-center px-4"
      style={{ backdropFilter: 'blur(2px)', backgroundColor: 'rgba(6,26,51,0.40)' }}
      initial={{ opacity: 0 }}
      animate={{ opacity: 1 }}
      exit={{ opacity: 0 }}
      transition={{ duration: 0.18 }}
      onClick={onClose}
    >
      <motion.div
        className="relative w-full max-w-[400px] bg-white rounded-[12px] shadow-xl overflow-hidden max-h-[90dvh] flex flex-col"
        initial={{ y: 24, scale: 0.97, opacity: 0 }}
        animate={{ y: 0,  scale: 1,    opacity: 1 }}
        exit={{ y: 24,    scale: 0.97, opacity: 0 }}
        transition={{ duration: 0.22, ease: [0.25, 0.46, 0.45, 0.94] }}
        onClick={e => e.stopPropagation()}
      >
        {/* Header strip — outside the scroll region, anchored to the top of the modal.
            Padding + dot + font size mirror RiskCardCompact's header for height parity. */}
        <div
          className="flex items-center justify-between px-3 py-2 shrink-0"
          style={{ backgroundColor: color }}
        >
          <div className="flex items-center gap-1.5">
            <span className="size-1.5 rounded-full bg-white shrink-0" />
            <span
              className="text-[10px] font-medium tracking-[0.05em] text-white uppercase"
              style={{ fontFamily: 'var(--font-mono)' }}
            >
              {RISK_LABEL_LONG[risk.level]}
            </span>
          </div>
          <span
            className="text-[10px] text-white/90"
            style={{ fontFamily: 'var(--font-mono)' }}
          >
            {risk.lastUpdated}
          </span>
        </div>

        {/* Close button — anchored to the outer (non-scrolling) container */}
        <button
          type="button"
          className="absolute top-12 right-3 size-10 rounded-full flex items-center justify-center
                     bg-white/80 border border-[var(--border)] shadow-[var(--shadow-xs)]
                     active:brightness-90 transition-[filter] duration-75 z-10"
          style={{ backdropFilter: 'blur(2px)' }}
          onClick={onClose}
        >
          <X size={14} className="text-[var(--fg-muted)]" />
        </button>

        {/* Scrollable region — fills the remaining vertical space below the header */}
        <div className="flex-1 min-h-0 overflow-y-auto">

        {/* Hero map placeholder */}
        <div className="h-[210px] bg-[var(--ink-100)] relative overflow-hidden">
          <div className="absolute inset-0 bg-[rgba(0,87,192,0.10)]" />
          <div
            className="absolute inset-0 m-auto size-4 rounded-full border-2 border-white"
            style={{ backgroundColor: color }}
          />
        </div>

        {/* Content */}
        <div className="flex flex-col gap-5 p-5">

          {/* Header: matches RiskCard's title + subtitle */}
          <div className="flex flex-col gap-0.5">
            <span
              className="text-[20px] font-bold leading-[1.3] text-[var(--navy-900)]"
              style={{ fontFamily: 'var(--font-sans)' }}
            >
              {risk.riverName}
            </span>
            <span
              className="text-[12px] text-[var(--fg-muted)]"
              style={{ fontFamily: 'var(--font-sans)' }}
            >
              {risk.stationName} · {risk.stationCode}
            </span>
          </div>

          {/* Bento: Volume + Tendência (StatTile typography) */}
          <div className="grid grid-cols-2 gap-3">
            {/* Volume */}
            <div className="relative bg-white border border-[var(--border)] rounded-[8px] shadow-[var(--shadow-xs)] p-4 flex flex-col gap-1 overflow-hidden">
              <div className="absolute top-0 inset-x-0 h-[3px]" style={{ backgroundColor: 'var(--ink-700)' }} />
              <span
                className="text-[9px] uppercase tracking-[0.05em] text-[var(--fg-muted)]"
                style={{ fontFamily: 'var(--font-mono)' }}
              >
                VOLUME
              </span>
              <div className="flex items-baseline gap-1" style={{ fontFamily: 'var(--font-mono)' }}>
                <span className="text-[16px] font-medium text-[var(--fg)]">{fillPercent}</span>
                <span className="text-[12px] text-[var(--fg-muted)]">%</span>
              </div>
              <span
                className="text-[12px] text-[var(--fg-muted)]"
                style={{ fontFamily: 'var(--font-mono)' }}
              >
                {fmt(risk.currentLevel)}m / {fmt(risk.limitLevel)}m
              </span>
            </div>

            {/* Tendência */}
            <div className="relative bg-white border border-[var(--border)] rounded-[8px] shadow-[var(--shadow-xs)] p-4 flex flex-col gap-1 overflow-hidden">
              <div className="absolute top-0 inset-x-0 h-[3px]" style={{ backgroundColor: color }} />
              <span
                className="text-[9px] uppercase tracking-[0.05em] text-[var(--fg-muted)]"
                style={{ fontFamily: 'var(--font-mono)' }}
              >
                TENDÊNCIA
              </span>
              <span
                className="text-[16px] font-medium"
                style={{ fontFamily: 'var(--font-mono)', color: trendColor }}
              >
                {risk.trendLabel ?? formatTrend(risk.trend)}
              </span>
            </div>

            {/* Janela de Ação */}
            {risk.actionWindow && (
              <div className="col-span-2 relative bg-white border border-[var(--border)] rounded-[8px] shadow-[var(--shadow-xs)] p-4 flex flex-col gap-1 overflow-hidden">
                <div className="absolute top-0 inset-x-0 h-[3px]" style={{ backgroundColor: color }} />
                <div className="flex items-center gap-2">
                  <Clock size={10} className="text-[var(--fg-muted)]" />
                  <span
                    className="text-[9px] uppercase tracking-[0.05em] text-[var(--fg-muted)]"
                    style={{ fontFamily: 'var(--font-mono)' }}
                  >
                    JANELA DE AÇÃO
                  </span>
                </div>
                <span
                  className="text-[16px] font-medium text-[var(--navy-900)]"
                  style={{ fontFamily: 'var(--font-mono)' }}
                >
                  {risk.actionWindow}
                </span>
                <span
                  className="text-[12px] text-[var(--fg-muted)]"
                  style={{ fontFamily: 'var(--font-mono)' }}
                >
                  Até cota de transbordo
                </span>
              </div>
            )}
          </div>

          {/* Evolução do Nível */}
          <div className="bg-white border border-[var(--border)] rounded-[8px] p-4 flex flex-col gap-2">
            <span
              className="text-[12px] font-bold tracking-[0.12em] uppercase text-[var(--fg-muted)]"
              style={{ fontFamily: 'var(--font-sans)' }}
            >
              EVOLUÇÃO DO NÍVEL
            </span>
            <div className="relative">
              {detail.evolution.map((step, i) => {
                const isLast       = i === detail.evolution.length - 1
                const isPredicted  = step.kind === 'predicted'
                const isPast       = step.kind === 'past'
                const dotBg        = isPast ? 'var(--ink-300)' : color
                const dotOpacity   = isPredicted ? 0.55 : 1
                const valueColor   = isPast ? 'var(--navy-900)' : color
                const nextStep     = detail.evolution[i + 1]
                const isDashedLine = isPredicted && nextStep?.kind === 'now'

                return (
                  <div key={step.label} className="flex gap-4 py-3 relative">
                    {/* Connector line */}
                    {!isLast && (
                      <div
                        className={`absolute left-[11px] top-[34px] bottom-[4px] w-px ${isDashedLine ? 'border-l border-dashed' : ''}`}
                        style={
                          isDashedLine
                            ? { borderColor: color, opacity: 0.55 }
                            : { backgroundColor: 'var(--ink-200)' }
                        }
                      />
                    )}
                    {/* Dot */}
                    <div
                      className="relative size-6 rounded-full border-4 border-white shrink-0 flex items-center justify-center shadow-[0_1px_1px_rgba(0,0,0,0.05)]"
                      style={{ backgroundColor: dotBg, opacity: dotOpacity }}
                    >
                      <span className="size-1.5 rounded-full bg-white" />
                    </div>
                    {/* Row */}
                    <div className={`flex-1 flex items-baseline justify-between pb-3 ${!isLast ? 'border-b border-[var(--ink-100)]' : ''}`}>
                      <span
                        className={`text-[13px] font-medium ${isPredicted ? 'italic text-[var(--navy-900)]' : isPast ? 'text-[var(--fg-muted)]' : 'text-[var(--navy-900)]'}`}
                        style={{ fontFamily: 'var(--font-mono)' }}
                      >
                        {step.label}
                      </span>
                      <span
                        className="text-[13px] font-medium"
                        style={{ fontFamily: 'var(--font-mono)', color: valueColor }}
                      >
                        {fmt(step.level, 1)}m
                      </span>
                    </div>
                  </div>
                )
              })}
            </div>
          </div>

          {/* Telemetria local */}
          <div className="flex flex-col gap-2">
            <span
              className="text-[12px] font-bold tracking-[0.12em] uppercase text-[var(--fg-muted)]"
              style={{ fontFamily: 'var(--font-sans)' }}
            >
              TELEMETRIA LOCAL
            </span>
            <div className="bg-[var(--bg)] border border-[var(--border)] rounded-[4px] p-3 flex items-center justify-between">
              <div className="flex items-center gap-3">
                <Droplet size={15} className="text-[var(--ink-700)]" />
                <span
                  className="text-[14px] text-[var(--navy-900)]"
                  style={{ fontFamily: 'var(--font-sans)' }}
                >
                  Vazão (Q)
                </span>
              </div>
              <span
                className="text-[13px] font-medium text-[var(--navy-900)]"
                style={{ fontFamily: 'var(--font-mono)' }}
              >
                {detail.telemetry.flowRate} m³/s
              </span>
            </div>
            <div className="bg-[var(--bg)] border border-[var(--border)] rounded-[4px] p-3 flex items-center justify-between">
              <div className="flex items-center gap-3">
                <CloudRain size={15} className="text-[var(--ink-700)]" />
                <span
                  className="text-[14px] text-[var(--navy-900)]"
                  style={{ fontFamily: 'var(--font-sans)' }}
                >
                  Pluviometria (24h)
                </span>
              </div>
              <span
                className="text-[13px] font-medium text-[var(--navy-900)]"
                style={{ fontFamily: 'var(--font-mono)' }}
              >
                {detail.telemetry.rainfall} mm
              </span>
            </div>
          </div>

          {/* CTA Emitir Alerta */}
          <button
            type="button"
            className="w-full rounded-[8px] py-3 flex items-center justify-center gap-2
                       active:opacity-90 transition-opacity duration-100 shadow-[var(--shadow-xs)]"
            style={{ backgroundColor: 'var(--accent)' }}
          >
            <AlertTriangle size={13} className="text-white" />
            <span
              className="text-[15px] font-semibold text-white"
              style={{ fontFamily: 'var(--font-sans)' }}
            >
              Emitir Alerta
            </span>
          </button>

        </div>
        </div>
      </motion.div>
    </motion.div>
  )
}
