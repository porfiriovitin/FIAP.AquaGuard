import { useEffect, useMemo, useRef, useState } from 'react'
import mapboxgl from 'mapbox-gl'
// Required by Mapbox GL JS — scoped to .mapboxgl-* classes, no Tailwind conflicts
import 'mapbox-gl/dist/mapbox-gl.css'
// Hides Mapbox logo + attribution (see styles.css for legal caveat)
import '../styles.css'
import { AlertTriangle, MapPin } from 'lucide-react'
import { useDeviceLocation } from '../hooks/useDeviceLocation'
import {
  createMarkerElement,
  createDeviceMarkerElement,
  type MarkerSize,
} from './MapMarker'

// ─── Types ───────────────────────────────────────────────────────────────

export interface MapPoint {
  id:     string
  lng:    number
  lat:    number
  color:  string                 // CSS color or `var(--token)`
  size?:  MarkerSize             // default 'md'
  pulse?: boolean                // default false; renders animated ring
}

export interface InteractionFlags {
  drag?:        boolean   // dragPan
  zoom?:        boolean   // scrollZoom + touchZoomRotate
  rotate?:      boolean   // dragRotate
  doubleClick?: boolean   // doubleClickZoom
  pitch?:       boolean   // touchPitch (two-finger pinch)
  keyboard?:    boolean
}

interface Props {
  center?:            [number, number]                  // [lng, lat]
  zoom?:              number                            // default: 15 with device, 11 without
  pitch?:             number                            // 0-85, default 0
  bearing?:           number                            // default 0
  /** Markers to render. Memoize with useMemo to avoid unnecessary re-syncs. */
  points?:            MapPoint[]
  useDeviceLocation?: boolean                           // default false
  interactions?:      boolean | InteractionFlags        // default false (locked)
  mapStyle?:          string                            // default 'mapbox://styles/mapbox/dark-v11'
  className?:         string                            // overrides container size
}

// ─── Constants ──────────────────────────────────────────────────────────

// Fallback center when geolocation is unavailable/disabled — São Paulo city center
const SAO_PAULO_COORDS: [number, number] = [-46.6333, -23.5505]

// Zoom 15 = ~500m radius (street level); zoom 11 = ~25km radius (city level)
const ZOOM_DEVICE   = 15
const ZOOM_FALLBACK = 11

const DEFAULT_MAP_STYLE = 'mapbox://styles/mapbox/dark-v11'

// Glassmorphism style shared across all HUD panels
const hudPanelStyle: React.CSSProperties = {
  backdropFilter:  'blur(6px)',
  backgroundColor: 'rgba(6,26,51,0.82)',
}

// ─── Helpers ────────────────────────────────────────────────────────────

// Resolves the public `interactions` prop into a flat shape — used both at
// construction and to sync handlers on a live map (via applyInteractions).
interface ResolvedInteractions {
  drag:        boolean
  zoom:        boolean
  rotate:      boolean
  doubleClick: boolean
  pitch:       boolean
  keyboard:    boolean
}

function resolveInteractions(interactions: Props['interactions']): ResolvedInteractions {
  if (interactions === undefined || interactions === false) {
    return { drag: false, zoom: false, rotate: false, doubleClick: false, pitch: false, keyboard: false }
  }
  if (interactions === true) {
    return { drag: true, zoom: true, rotate: true, doubleClick: true, pitch: true, keyboard: true }
  }
  return {
    drag:        interactions.drag        ?? false,
    zoom:        interactions.zoom        ?? false,
    rotate:      interactions.rotate      ?? false,
    doubleClick: interactions.doubleClick ?? false,
    pitch:       interactions.pitch       ?? false,
    keyboard:    interactions.keyboard    ?? false,
  }
}

// Applies enable/disable to each handler. Kept off the constructor so the
// `interactions` prop can be toggled after mount without recreating the map.
function applyInteractions(map: mapboxgl.Map, r: ResolvedInteractions): void {
  const toggle = (handler: { enable: () => void; disable: () => void } | undefined, on: boolean) => {
    if (!handler) return
    on ? handler.enable() : handler.disable()
  }
  toggle(map.dragPan,         r.drag)
  toggle(map.scrollZoom,      r.zoom)
  toggle(map.touchZoomRotate, r.zoom)
  toggle(map.dragRotate,      r.rotate)
  toggle(map.doubleClickZoom, r.doubleClick)
  toggle(map.touchPitch,      r.pitch)
  toggle(map.keyboard,        r.keyboard)
}

// ─── Overlays ───────────────────────────────────────────────────────────

function LoadingOverlay() {
  return (
    <div className="absolute inset-0 flex items-center justify-center bg-[var(--navy-900)]/80 z-10">
      <div className="flex flex-col items-center gap-2">
        <div className="size-6 rounded-full border-2 border-[var(--cyan-400)] border-t-transparent animate-spin" />
        <span
          className="text-[10px] text-[var(--fg-onDarkMuted)] uppercase tracking-[0.08em]"
          style={{ fontFamily: 'var(--font-mono)' }}
        >
          Localizando...
        </span>
      </div>
    </div>
  )
}

function LocationDeniedBanner() {
  return (
    <div className="absolute bottom-4 left-4 z-10">
      <div
        className="flex items-center gap-1.5 px-2.5 py-1.5 rounded-[var(--radius-xs)]
                   border border-[var(--navy-700)]"
        style={hudPanelStyle}
      >
        <MapPin size={11} className="text-[var(--fg-onDarkMuted)] shrink-0" />
        <span
          className="text-[10px] text-[var(--fg-onDarkMuted)]"
          style={{ fontFamily: 'var(--font-mono)' }}
        >
          Localização não disponível
        </span>
      </div>
    </div>
  )
}

function MapErrorOverlay({ message }: { message: string }) {
  return (
    <div className="absolute inset-0 flex items-center justify-center z-10">
      <div
        className="flex flex-col items-center gap-2 px-4 py-3 rounded-[var(--radius-sm)]
                   border border-[var(--navy-700)] max-w-[260px] text-center"
        style={hudPanelStyle}
      >
        <AlertTriangle size={16} className="text-[var(--risk-high)] shrink-0" />
        <span
          className="text-[10px] text-[var(--fg-onDarkMuted)] leading-relaxed"
          style={{ fontFamily: 'var(--font-mono)' }}
        >
          {message}
        </span>
      </div>
    </div>
  )
}

// ─── Component ──────────────────────────────────────────────────────────

const EMPTY_POINTS: MapPoint[] = []

export function MapView({
  center,
  zoom,
  pitch = 0,
  bearing = 0,
  points = EMPTY_POINTS,
  useDeviceLocation: useDeviceLocationProp = false,
  interactions,
  mapStyle = DEFAULT_MAP_STYLE,
  className = 'w-full h-[300px] md:h-[360px]',
}: Props) {
  const containerRef = useRef<HTMLDivElement>(null)
  const mapRef       = useRef<mapboxgl.Map | null>(null)
  const [mapError, setMapError] = useState<string | null>(null)

  const { coords: deviceCoords, status: locStatus } = useDeviceLocation({
    enabled: useDeviceLocationProp,
  })

  // Center resolution: explicit prop > device coords > SP fallback
  const resolvedCenter = useMemo<[number, number]>(() => {
    if (center) return center
    if (useDeviceLocationProp && deviceCoords) return deviceCoords
    return SAO_PAULO_COORDS
  }, [center, useDeviceLocationProp, deviceCoords])

  // Zoom resolution: explicit prop > auto (15 with device coords, 11 without)
  const resolvedZoom = zoom ?? ((useDeviceLocationProp && deviceCoords) ? ZOOM_DEVICE : ZOOM_FALLBACK)

  // ─── Effect 1: init — creates the map ONCE (re-runs only on style change) ───
  useEffect(() => {
    setMapError(null)
    if (!containerRef.current) return

    const token = import.meta.env.VITE_MAPBOX_TOKEN
    if (!token) {
      console.error('[MapView] VITE_MAPBOX_TOKEN não encontrado. Reinicie o servidor após editar o .env.')
      setMapError('Token do mapa não configurado. Reinicie o servidor de desenvolvimento.')
      return
    }

    mapboxgl.accessToken = token

    // The constructor itself throws on missing WebGL — the try/catch below
    // surfaces it. (mapboxgl.supported() was removed in v3.)
    let map: mapboxgl.Map
    try {
      map = new mapboxgl.Map({
        container:          containerRef.current,
        style:              mapStyle,
        center:             resolvedCenter,
        zoom:               resolvedZoom,
        pitch,
        bearing,
        attributionControl: false,
        // Handlers are managed via applyInteractions so the `interactions`
        // prop can be toggled after mount without re-init.
      })
    } catch (err) {
      const msg = err instanceof Error ? err.message : 'Falha ao inicializar o mapa'
      console.error('[MapView] constructor threw:', msg)
      setMapError(msg)
      return
    }

    mapRef.current = map
    applyInteractions(map, resolveInteractions(interactions))

    map.on('error', (e) => {
      const msg = e.error?.message ?? 'Erro desconhecido'
      console.error('[MapView] runtime error:', msg)
      setMapError(`Mapbox: ${msg}`)
    })

    // React 18 StrictMode mounts → unmounts → remounts in dev. map.remove()
    // prevents duplicate canvas and event listener leaks.
    return () => {
      map.remove()
      mapRef.current = null
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [mapStyle])

  // ─── Effect 2: markers — syncs when `points` or device coords change ───
  useEffect(() => {
    const map = mapRef.current
    if (!map) return

    // Cancellation flag handles the race where the effect re-runs (or
    // unmounts) before map 'load' fires. Without it, late-fired `addAll()`
    // would create orphan markers the cleanup closure never sees.
    let cancelled = false
    const markers: mapboxgl.Marker[] = []

    const addAll = () => {
      if (cancelled) return
      points.forEach(p => {
        markers.push(
          new mapboxgl.Marker({
            element: createMarkerElement({ color: p.color, size: p.size, pulse: p.pulse }),
          })
            .setLngLat([p.lng, p.lat])
            .addTo(map),
        )
      })
      if (useDeviceLocationProp && deviceCoords) {
        markers.push(
          new mapboxgl.Marker({ element: createDeviceMarkerElement() })
            .setLngLat(deviceCoords)
            .addTo(map),
        )
      }
    }

    if (map.loaded()) addAll()
    else map.once('load', addAll)

    return () => {
      cancelled = true
      markers.forEach(m => m.remove())
    }
  }, [points, deviceCoords, useDeviceLocationProp])

  // ─── Effect 3: view-state — animates camera when center/zoom/pitch change ───
  useEffect(() => {
    const map = mapRef.current
    if (!map) return
    map.easeTo({
      center:    resolvedCenter,
      zoom:      resolvedZoom,
      pitch,
      bearing,
      duration:  600,
      essential: true,
    })
  }, [resolvedCenter[0], resolvedCenter[1], resolvedZoom, pitch, bearing])

  // ─── Effect 4: interactions — syncs handlers when the prop changes ───
  useEffect(() => {
    const map = mapRef.current
    if (!map) return
    applyInteractions(map, resolveInteractions(interactions))
  }, [interactions])

  // ─── Render ─────────────────────────────────────────────────────────
  return (
    <div
      className={`relative isolate rounded-[12px] overflow-hidden border border-[var(--navy-800)] ${className}`}
      style={{ boxShadow: 'var(--inset-soft)' }}
    >
      {/* Inline style prevents .mapboxgl-map { position: relative } from overriding absolute positioning */}
      <div ref={containerRef} style={{ position: 'absolute', inset: 0 }} />

      {/* State overlays — only relevant when device location was requested */}
      {useDeviceLocationProp && locStatus === 'loading' && <LoadingOverlay />}
      {useDeviceLocationProp && (locStatus === 'denied' || locStatus === 'unavailable') && <LocationDeniedBanner />}
      {mapError && <MapErrorOverlay message={mapError} />}

      {/* Edge vignette — softens the border on mobile */}
      <div
        aria-hidden="true"
        className="absolute inset-0 pointer-events-none rounded-[inherit] z-10"
        style={{ boxShadow: 'inset 0 2px 4px 1px rgba(0,0,0,0.12)' }}
      />
    </div>
  )
}
