
// Satellites are placed at 40–85% of the kernel radius away from center (~0.015–0.032°),
// so the Gaussian kernels deform visibly without splitting into separate blobs.
// Each station is biased toward a different direction to give each blob a unique silhouette.
const FLOOD_POINTS = {
  type: 'FeatureCollection' as const,
  features: [
    // rh-204 — critical  (bias: elongated toward NE, sparse tail to SW)
    { type: 'Feature' as const, properties: { riskLevel: 'critical', w: 1.00 },
      geometry: { type: 'Point' as const, coordinates: [-46.7158, -23.5155] } },
    { type: 'Feature' as const, properties: { riskLevel: 'critical', w: 0.72 },
      geometry: { type: 'Point' as const, coordinates: [-46.7005, -23.5082] } }, // NE medium
    { type: 'Feature' as const, properties: { riskLevel: 'critical', w: 0.55 },
      geometry: { type: 'Point' as const, coordinates: [-46.6928, -23.5158] } }, // E far
    { type: 'Feature' as const, properties: { riskLevel: 'critical', w: 0.62 },
      geometry: { type: 'Point' as const, coordinates: [-46.7072, -23.5018] } }, // N medium
    { type: 'Feature' as const, properties: { riskLevel: 'critical', w: 0.48 },
      geometry: { type: 'Point' as const, coordinates: [-46.6982, -23.5048] } }, // NE far
    { type: 'Feature' as const, properties: { riskLevel: 'critical', w: 0.38 },
      geometry: { type: 'Point' as const, coordinates: [-46.7312, -23.5248] } }, // SW medium
    { type: 'Feature' as const, properties: { riskLevel: 'critical', w: 0.30 },
      geometry: { type: 'Point' as const, coordinates: [-46.7392, -23.5108] } }, // W sparse

    // rh-205 — high  (bias: fat belly to SE, thin tail to NW)
    { type: 'Feature' as const, properties: { riskLevel: 'high', w: 1.00 },
      geometry: { type: 'Point' as const, coordinates: [-46.7310, -23.5892] } },
    { type: 'Feature' as const, properties: { riskLevel: 'high', w: 0.68 },
      geometry: { type: 'Point' as const, coordinates: [-46.7148, -23.5958] } }, // SE medium
    { type: 'Feature' as const, properties: { riskLevel: 'high', w: 0.52 },
      geometry: { type: 'Point' as const, coordinates: [-46.7042, -23.5895] } }, // E far
    { type: 'Feature' as const, properties: { riskLevel: 'high', w: 0.58 },
      geometry: { type: 'Point' as const, coordinates: [-46.7238, -23.6028] } }, // S medium
    { type: 'Feature' as const, properties: { riskLevel: 'high', w: 0.44 },
      geometry: { type: 'Point' as const, coordinates: [-46.7118, -23.6058] } }, // SE far
    { type: 'Feature' as const, properties: { riskLevel: 'high', w: 0.36 },
      geometry: { type: 'Point' as const, coordinates: [-46.7468, -23.5842] } }, // NW sparse
    { type: 'Feature' as const, properties: { riskLevel: 'high', w: 0.42 },
      geometry: { type: 'Point' as const, coordinates: [-46.7388, -23.5992] } }, // SW medium

    // rh-118 — moderate  (bias: stretched E, rounded on W)
    { type: 'Feature' as const, properties: { riskLevel: 'moderate', w: 1.00 },
      geometry: { type: 'Point' as const, coordinates: [-46.5869, -23.5875] } },
    { type: 'Feature' as const, properties: { riskLevel: 'moderate', w: 0.65 },
      geometry: { type: 'Point' as const, coordinates: [-46.5702, -23.5918] } }, // E medium
    { type: 'Feature' as const, properties: { riskLevel: 'moderate', w: 0.50 },
      geometry: { type: 'Point' as const, coordinates: [-46.5608, -23.5858] } }, // E far
    { type: 'Feature' as const, properties: { riskLevel: 'moderate', w: 0.57 },
      geometry: { type: 'Point' as const, coordinates: [-46.5845, -23.6008] } }, // S medium
    { type: 'Feature' as const, properties: { riskLevel: 'moderate', w: 0.40 },
      geometry: { type: 'Point' as const, coordinates: [-46.5672, -23.6022] } }, // SE far
    { type: 'Feature' as const, properties: { riskLevel: 'moderate', w: 0.33 },
      geometry: { type: 'Point' as const, coordinates: [-46.6005, -23.5945] } }, // W sparse

    // rh-105 — low  (bias: NE lobe, S dip)
    { type: 'Feature' as const, properties: { riskLevel: 'low', w: 1.00 },
      geometry: { type: 'Point' as const, coordinates: [-46.6480, -23.7440] } },
    { type: 'Feature' as const, properties: { riskLevel: 'low', w: 0.62 },
      geometry: { type: 'Point' as const, coordinates: [-46.6312, -23.7372] } }, // NE medium
    { type: 'Feature' as const, properties: { riskLevel: 'low', w: 0.46 },
      geometry: { type: 'Point' as const, coordinates: [-46.6238, -23.7448] } }, // E far
    { type: 'Feature' as const, properties: { riskLevel: 'low', w: 0.52 },
      geometry: { type: 'Point' as const, coordinates: [-46.6522, -23.7305] } }, // N medium
    { type: 'Feature' as const, properties: { riskLevel: 'low', w: 0.40 },
      geometry: { type: 'Point' as const, coordinates: [-46.6448, -23.7572] } }, // S medium
    { type: 'Feature' as const, properties: { riskLevel: 'low', w: 0.34 },
      geometry: { type: 'Point' as const, coordinates: [-46.6618, -23.7512] } }, // SW sparse
  ],
}

type Level = 'critical' | 'high' | 'moderate' | 'low'

// Color cascade per level: each level's heatmap ramps from transparent → green → … → level max.
// Critical reaches red, high reaches orange, moderate reaches yellow, low stays green.
// This mirrors how a weather radar works — green perimeter, saturated core at max severity.
type ColorStop = [number, string]

const LEVEL_COLOR_RAMP: Record<Level, ColorStop[]> = {
  critical: [
    [0,    'rgba(0,0,0,0)'],
    [0.12, 'rgba(27,184,146,0.10)'],   // faint green edge
    [0.30, 'rgba(27,184,146,0.65)'],   // green
    [0.52, '#f1b50a'],                 // yellow
    [0.72, '#ef7a1a'],                 // orange
    [1.0,  '#c0222a'],                 // red core
  ],
  high: [
    [0,    'rgba(0,0,0,0)'],
    [0.15, 'rgba(27,184,146,0.10)'],
    [0.38, 'rgba(27,184,146,0.65)'],   // green
    [0.68, '#f1b50a'],                 // yellow
    [1.0,  '#ef7a1a'],                 // orange core
  ],
  moderate: [
    [0,    'rgba(0,0,0,0)'],
    [0.20, 'rgba(27,184,146,0.10)'],
    [0.50, 'rgba(27,184,146,0.65)'],   // green
    [1.0,  '#f1b50a'],                 // yellow core
  ],
  low: [
    [0,    'rgba(0,0,0,0)'],
    [0.25, 'rgba(27,184,146,0.10)'],
    [1.0,  '#1bb892'],                 // green core
  ],
}

export function addFloodMockLayers(map: mapboxgl.Map): void {
  map.addSource('flood-heatmap', { type: 'geojson', data: FLOOD_POINTS })

  // Render low → critical so critical sits on top in z-order
  const LEVELS: Level[] = ['low', 'moderate', 'high', 'critical']

  for (const level of LEVELS) {
    const stops = LEVEL_COLOR_RAMP[level]
    const colorExpr: mapboxgl.ExpressionSpecification = [
      'interpolate', ['linear'], ['heatmap-density'],
      ...stops.flat(),
    ]

    map.addLayer({
      id:     `flood-heatmap-${level}`,
      type:   'heatmap',
      source: 'flood-heatmap',
      filter: ['==', ['get', 'riskLevel'], level],
      paint: {
        // Normalized weight: every level peaks at 1.0, so sizes are equal
        'heatmap-weight': ['interpolate', ['linear'], ['get', 'w'], 0, 0, 1, 1],

        // Moderate intensity → density builds gradually → diffuse look
        'heatmap-intensity': ['interpolate', ['linear'], ['zoom'], 0, 0.8, 15, 2.5],

        'heatmap-color': colorExpr,

        // Uniform radius — all levels occupy the same footprint
        'heatmap-radius': [
          'interpolate', ['linear'], ['zoom'],
          0,  3,
          12, 200,
          15, 420,
        ],

        'heatmap-opacity': 0.42,
      },
    })
  }
}
