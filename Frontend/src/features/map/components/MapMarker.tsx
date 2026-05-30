// Factory helpers for Mapbox markers. Mapbox needs a raw HTMLElement (not JSX)
// to pass into `new mapboxgl.Marker({ element })`, so these are imperative DOM
// builders rather than React components.

export type MarkerSize = 'sm' | 'md' | 'lg'

export interface MarkerSpec {
  color:  string
  size?:  MarkerSize    // default 'md'
  pulse?: boolean       // default false; renders an animated ring around the dot
}

const DOT_PX:  Record<MarkerSize, number> = { sm: 10, md: 14, lg: 18 }
const RING_PX: Record<MarkerSize, number> = { sm: 22, md: 30, lg: 38 }

export function createMarkerElement({ color, size = 'md', pulse = false }: MarkerSpec): HTMLDivElement {
  const dotPx  = DOT_PX[size]
  const ringPx = RING_PX[size]

  const wrap = document.createElement('div')
  Object.assign(wrap.style, {
    position: 'relative',
    width:    `${dotPx}px`,
    height:   `${dotPx}px`,
  })

  if (pulse) {
    const ring = document.createElement('div')
    const offset = (dotPx - ringPx) / 2
    Object.assign(ring.style, {
      position:        'absolute',
      width:           `${ringPx}px`,
      height:          `${ringPx}px`,
      top:             `${offset}px`,
      left:            `${offset}px`,
      borderRadius:    '50%',
      backgroundColor: color,
      opacity:         '0.25',
      animation:       'ag-pulse 2s infinite',
      pointerEvents:   'none',
    })
    wrap.appendChild(ring)
  }

  const dot = document.createElement('div')
  Object.assign(dot.style, {
    position:        'relative',
    width:           '100%',
    height:          '100%',
    borderRadius:    '50%',
    backgroundColor: color,
    border:          '2px solid var(--navy-900)',
    boxShadow:       '0 0 8px rgba(0,0,0,0.3)',
  })
  wrap.appendChild(dot)

  return wrap
}

// Specialised "you are here" marker: cyan glow + navy border, distinct from
// generic data points so the device is unmistakable on the map.
export function createDeviceMarkerElement(): HTMLDivElement {
  const el = document.createElement('div')
  Object.assign(el.style, {
    width:        '14px',
    height:       '14px',
    borderRadius: '50%',
    background:   '#2dd4d4',                                            // --cyan-400
    border:       '2.5px solid #0b2545',                                // --navy-900
    boxShadow:    '0 0 0 4px rgba(45,212,212,0.25), 0 0 12px rgba(75,217,226,0.7)',
  })
  return el
}
