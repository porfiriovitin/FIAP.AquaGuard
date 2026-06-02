import { useCallback } from 'react'
import mapboxgl from 'mapbox-gl'
import { MapView } from '../features/map/components/MapView'
import { getFloodLayerCallback } from '../features/map/services/mapService'

// Morumbi — Estádio Cícero Pompeu de Toledo
const MORUMBI: [number, number] = [-46.7197, -23.5989]

const LEVEL_COLOR: Record<string, string> = {
  critical: 'var(--risk-critical)',
  high:     'var(--risk-high)',
  moderate: 'var(--risk-moderate)',
  low:      'var(--risk-low)',
  normal:   'var(--risk-normal)',
}

const LEGEND_ITEMS = [
  { level: 'critical', label: 'Crítico'  },
  { level: 'high',     label: 'Alto'     },
  { level: 'moderate', label: 'Moderado' },
  { level: 'low',      label: 'Baixo'    },
  { level: 'normal',   label: 'Normal'   },
]

// Adds fill-extrusion buildings from the composite tileset bundled in dark-v11
// (streets-based style — includes 'building' source-layer out of the box).
function add3dBuildings(map: mapboxgl.Map): void {
  try {
    const layers = map.getStyle()?.layers ?? []
    const firstSymbolId = layers.find(l => l.type === 'symbol')?.id

    map.addLayer(
      {
        id:             '3d-buildings',
        source:         'composite',
        'source-layer': 'building',
        filter:         ['==', 'extrude', 'true'],
        type:           'fill-extrusion',
        minzoom:        13,
        paint: {
          'fill-extrusion-color': '#0d1f33',
          'fill-extrusion-height': [
            'interpolate', ['linear'], ['zoom'],
            13, 0,
            13.5, ['get', 'height'],
          ],
          'fill-extrusion-base': [
            'interpolate', ['linear'], ['zoom'],
            13, 0,
            13.5, ['get', 'min_height'],
          ],
          'fill-extrusion-opacity': 0.80,
        },
      },
      firstSymbolId,
    )
  } catch {
    // Style doesn't include building data — silently skip
  }
}

export function MapPage() {
  const handleLoad = useCallback((map: mapboxgl.Map) => {
    const floodCb = getFloodLayerCallback()
    if (floodCb) floodCb(map)
    add3dBuildings(map)
  }, [])

  return (
    // bg-[var(--navy-900)] matches MapView's own container bg — prevents
    // a contrasting flash behind the rounded corners at the page edges.
    <div className="relative bg-[var(--navy-900)]">
      <MapView
        center={MORUMBI}
        zoom={14}
        pitch={65}
        useDeviceLocation
        interactions
        onLoad={handleLoad}
        className="w-full h-[calc(100dvh-65px-75px)]"
      />

      {/* Risk level legend */}
      <div
        className="absolute bottom-3 right-3 z-20 flex flex-col gap-1.5
                   px-3 py-2.5 rounded-[var(--radius-xs)]
                   border border-[var(--navy-700)]"
        style={{ backdropFilter: 'blur(6px)', backgroundColor: 'rgba(6,26,51,0.82)' }}
      >
        {LEGEND_ITEMS.map(({ level, label }) => (
          <div key={level} className="flex items-center gap-2">
            <span
              className="size-2 rounded-full shrink-0"
              style={{ backgroundColor: LEVEL_COLOR[level] }}
            />
            <span
              className="text-[10px] text-[var(--fg-onDarkMuted)] leading-none"
              style={{ fontFamily: 'var(--font-mono)' }}
            >
              {label}
            </span>
          </div>
        ))}
      </div>
    </div>
  )
}
