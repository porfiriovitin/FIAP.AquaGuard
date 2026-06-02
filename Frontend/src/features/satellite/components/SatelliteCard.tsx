import { Satellite } from 'lucide-react'

type Scheme = 'cyan' | 'mint'

const SCHEMES: Record<Scheme, {
  bg:         string
  bgCircle:   string
  border:     string
  bar:        string
  glow:       string
  orbitTop:   string
  orbitRight: string
  icon:       string
  signal:     string
}> = {
  cyan: {
    bg:         'var(--ink-950)',
    bgCircle:   'rgba(32,191,200,0.12)',
    border:     'rgba(32,191,200,0.20)',
    bar:        'linear-gradient(90deg, var(--cyan-500), var(--blue-600))',
    glow:       'radial-gradient(ellipse at 85% 50%, rgba(31,111,229,0.14) 0%, transparent 65%)',
    orbitTop:   'var(--cyan-500)',
    orbitRight: 'rgba(32,191,200,0.22)',
    icon:       'var(--cyan-400)',
    signal:     'var(--signal)',
  },
  mint: {
    bg:         'var(--ink-950)',
    bgCircle:   'rgba(27,184,146,0.12)',
    border:     'rgba(27,184,146,0.20)',
    bar:        'linear-gradient(90deg, var(--mint-500), var(--cyan-500))',
    glow:       'radial-gradient(ellipse at 85% 50%, rgba(27,184,146,0.14) 0%, transparent 65%)',
    orbitTop:   'var(--mint-400)',
    orbitRight: 'rgba(70,220,195,0.20)',
    icon:       'var(--mint-400)',
    signal:     'var(--mint-500)',
  },
}

interface Stats {
  orbit:    number | string
  altitude: string
  band:     string
}

interface Props {
  name?:   string
  scheme?: Scheme
  stats?:  Stats
}

const DEFAULT_STATS: Stats = {
  orbit:    231,
  altitude: '693 km',
  band:     'Banda-C',
}

export function SatelliteCard({ name = 'Sentinel-1C', scheme = 'cyan', stats = DEFAULT_STATS }: Props) {
  const s = SCHEMES[scheme]

  return (
    <article
      className="overflow-hidden rounded-[var(--radius-md)] shadow-[var(--shadow-md)]"
      style={{ background: s.bg, border: `1px solid ${s.border}` }}
    >
      {/* Gradient top bar */}
      <div className="h-[3px] w-full" style={{ background: s.bar }} />

      {/* Card body */}
      <div className="p-3.5 flex flex-col gap-2.5" style={{ background: s.glow }}>

        {/* Main row: orbital icon + identity · live badge */}
        <div className="flex items-center justify-between gap-3">

          {/* Left: icon + name */}
          <div className="flex items-center gap-2.5">
            <div
              className="relative flex items-center justify-center w-9 h-9 rounded-full shrink-0"
              style={{ background: s.bgCircle }}
            >
              <div
                className="absolute inset-[-6px] rounded-full border-[1.5px] border-transparent animate-orbit"
                style={{ borderTopColor: s.orbitTop, borderRightColor: s.orbitRight }}
              />
              <Satellite size={15} style={{ color: s.icon }} />
            </div>

            <div className="flex flex-col gap-0.5">
              <span
                className="text-[10px] font-medium tracking-[0.14em] uppercase"
                style={{ fontFamily: 'var(--font-mono)', color: 'var(--fg-onDarkMuted)' }}
              >
                SATÉLITE
              </span>
              <span
                className="text-[14px] font-normal leading-none"
                style={{ fontFamily: 'var(--font-sans)', color: 'var(--fg-onDark)' }}
              >
                {name}
              </span>
            </div>
          </div>

          {/* Live badge */}
          <div className="flex items-center gap-1.5 shrink-0" style={{ color: s.signal }}>
            <div
              className="w-[5px] h-[5px] rounded-full animate-live"
              style={{ background: s.signal }}
            />
            <span
              className="text-[10px] font-medium uppercase tracking-[0.1em]"
              style={{ fontFamily: 'var(--font-mono)' }}
            >
              ONLINE
            </span>
          </div>
        </div>

        {/* Divider */}
        <div className="h-px" style={{ background: s.border, opacity: 0.6 }} />

        {/* Stats row */}
        <div
          className="flex items-center gap-2 text-[10px]"
          style={{ fontFamily: 'var(--font-mono)', color: 'var(--fg-onDarkMuted)' }}
        >
          <span>Órbita {stats.orbit}</span>
          <span style={{ opacity: 0.3 }}>·</span>
          <span>Alt. {stats.altitude}</span>
          <span style={{ opacity: 0.3 }}>·</span>
          <span>{stats.band}</span>
        </div>
      </div>
    </article>
  )
}
