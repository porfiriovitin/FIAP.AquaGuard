import { useEffect } from 'react'
import { X, BarChart2, Battery, Signal } from 'lucide-react'
import { motion } from 'framer-motion'
import { useSensors }      from '../hooks/useSensors'
import { useSensorDetail } from '../hooks/useSensorDetail'
import { STATUS_STYLE } from '../utils/sensorStatus'
import { resolveTrendColor, formatTrend } from '../utils/trend'
import { MapView } from '../../map/components/MapView'

interface Props {
  sensorId: string
  onClose:  () => void
}

export function SensorDetailModal({ sensorId, onClose }: Props) {
  const { data: sensors = [] }       = useSensors()
  const { data: detail, isLoading } = useSensorDetail(sensorId)
  const sensor = sensors.find(s => s.id === sensorId)

  useEffect(() => {
    const onKey = (e: KeyboardEvent) => { if (e.key === 'Escape') onClose() }
    document.addEventListener('keydown', onKey)
    document.body.style.overflow = 'hidden'
    return () => {
      document.removeEventListener('keydown', onKey)
      document.body.style.overflow = ''
    }
  }, [onClose])

  if (isLoading || !sensor || !detail) return null

  const coords = detail.coords

  const style = STATUS_STYLE[sensor.status]

  return (
    <motion.div
      className="fixed inset-0 z-50 flex items-center justify-center px-4"
      style={{ backdropFilter: 'blur(2px)', backgroundColor: 'rgba(6,26,51,0.70)' }}
      initial={{ opacity: 0 }}
      animate={{ opacity: 1 }}
      exit={{ opacity: 0 }}
      transition={{ duration: 0.18 }}
      onClick={onClose}
    >
      <motion.div
        className="relative w-full max-w-[400px] bg-white rounded-[12px] shadow-xl overflow-y-auto max-h-[90dvh] border-t-4"
        style={{ borderTopColor: style.barColor }}
        initial={{ y: 24, scale: 0.97, opacity: 0 }}
        animate={{ y: 0,  scale: 1,    opacity: 1 }}
        exit={{ y: 24,    scale: 0.97, opacity: 0 }}
        transition={{ duration: 0.22, ease: [0.25, 0.46, 0.45, 0.94] }}
        onClick={e => e.stopPropagation()}
      >
        {/* Close button */}
        <button
          type="button"
          className="absolute top-3 right-3 size-8 rounded-full flex items-center justify-center
                     bg-[var(--ink-100)] active:brightness-90 transition-[filter] duration-75 z-10"
          onClick={onClose}
        >
          <X size={16} className="text-[var(--fg-muted)]" />
        </button>

        {/* Header */}
        <div className="px-4 pt-5 pb-4 flex flex-col gap-1">
          <div className="flex items-center gap-1.5" style={{ color: style.badgeColor }}>
            <style.Icon size={13} />
            <span
              className="text-[11px] font-medium uppercase"
              style={{ fontFamily: 'var(--font-mono)' }}
            >
              {style.label}
            </span>
          </div>
          <span
            className="text-[24px] font-semibold leading-tight text-[var(--navy-900)]"
            style={{ fontFamily: 'var(--font-sans)' }}
          >
            {sensor.stationId}
          </span>
          <span
            className="text-[15px] text-[var(--fg-muted)]"
            style={{ fontFamily: 'var(--font-sans)' }}
          >
            {sensor.location}
          </span>
        </div>

        {/* Map */}
        <div className="mx-4 mb-5 h-36 rounded-[8px] overflow-hidden">
          <MapView
            center={coords}
            zoom={14}
            points={[{ id: sensorId, lng: coords[0], lat: coords[1], color: style.barColor, size: 'md', pulse: sensor.status !== 'offline' }]}
            className="w-full h-36"
          />
        </div>

        {/* Hardware bento */}
        <div className="grid grid-cols-2 gap-2 px-4 mb-5">
          <div className="bg-[var(--bg)] rounded-[8px] p-3 flex flex-col gap-1">
            <div className="flex items-center justify-between">
              <span
                className="text-[12px] font-bold tracking-[0.08em] uppercase text-[var(--fg-muted)]"
                style={{ fontFamily: 'var(--font-sans)' }}
              >
                BATERIA
              </span>
              <Battery size={15} className="text-[var(--fg-muted)]" />
            </div>
            <div className="flex items-baseline gap-1">
              <span
                className="text-[16px] font-medium text-[var(--navy-900)]"
                style={{ fontFamily: 'var(--font-mono)' }}
              >
                {detail.battery}
              </span>
              <span
                className="text-[12px] text-[var(--fg-muted)]"
                style={{ fontFamily: 'var(--font-mono)' }}
              >
                %
              </span>
            </div>
          </div>
          <div className="bg-[var(--bg)] rounded-[8px] p-3 flex flex-col gap-1">
            <div className="flex items-center justify-between">
              <span
                className="text-[12px] font-bold tracking-[0.08em] uppercase text-[var(--fg-muted)]"
                style={{ fontFamily: 'var(--font-sans)' }}
              >
                SINAL LTE
              </span>
              <Signal size={15} className="text-[var(--fg-muted)]" />
            </div>
            <div className="flex items-baseline gap-1">
              <span
                className="text-[16px] font-medium text-[var(--navy-900)]"
                style={{ fontFamily: 'var(--font-mono)' }}
              >
                {detail.signalDbm}
              </span>
              <span
                className="text-[12px] text-[var(--fg-muted)]"
                style={{ fontFamily: 'var(--font-mono)' }}
              >
                dBm
              </span>
            </div>
          </div>
        </div>

        {/* Telemetry */}
        <div className="px-4 mb-5">
          <div className="pb-2 mb-2 border-b border-[var(--border)]">
            <span
              className="text-[12px] font-bold tracking-[0.08em] uppercase text-[var(--fg-muted)]"
              style={{ fontFamily: 'var(--font-sans)' }}
            >
              TELEMETRIA RECENTE
            </span>
          </div>

          <div className="grid grid-cols-3 mb-1.5" style={{ fontFamily: 'var(--font-mono)' }}>
            <span className="text-[10px] text-[var(--fg-muted)]">HORA</span>
            <span className="text-[10px] text-[var(--fg-muted)]">NÍVEL (M)</span>
            <span className="text-[10px] text-[var(--fg-muted)]">TEND.</span>
          </div>

          <div className="flex flex-col">
            {detail.telemetry.map((row, i) => {
              const trendColor = resolveTrendColor(row.trend)
              const arrow = row.trend > 0 ? '↑' : row.trend < 0 ? '↓' : '→'
              const isLast = i === detail.telemetry.length - 1

              return (
                <div
                  key={row.time}
                  className={`grid grid-cols-3 text-[13px] text-[var(--navy-900)] py-2
                              ${!isLast ? 'border-b border-[var(--ink-100)]' : ''}`}
                  style={{ fontFamily: 'var(--font-mono)' }}
                >
                  <span>{row.time}</span>
                  <span>{row.level.toFixed(2).replace('.', ',')}</span>
                  <span style={{ color: trendColor }}>
                    {arrow} {formatTrend(row.trend)}
                  </span>
                </div>
              )
            })}
          </div>
        </div>

        {/* Action link */}
        <div className="px-4 pb-6 flex justify-center">
          <button
            type="button"
            className="flex items-center gap-1.5 active:opacity-70 transition-opacity duration-75"
          >
            <BarChart2 size={14} style={{ color: 'var(--accent)' }} />
            <span
              className="text-[13px] font-medium"
              style={{ fontFamily: 'var(--font-mono)', color: 'var(--accent)' }}
            >
              VER GRÁFICO COMPLETO
            </span>
          </button>
        </div>
      </motion.div>
    </motion.div>
  )
}
