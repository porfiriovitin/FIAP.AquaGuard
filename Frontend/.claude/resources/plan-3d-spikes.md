# Plano: 3D Spike Chart para estações de risco no mapa

## Context
O usuário quer visualizar cada estação de monitoramento de risco como uma "agulha" 3D que sobe verticalmente do mapa, com um label flutuando no topo. O objetivo é criar um gráfico de barras 3D nativo sobre o mapa Mapbox, com altura proporcional à severidade do risco.

## Approach

### Técnica escolhida
`fill-extrusion` com micro-polígonos (círculo de ~9m de raio, 8 segmentos) + `symbol` layer com `text-offset` data-driven por nível. Sem bibliotecas externas — apenas trigonometria inline.

> Por que não HTML markers: markers HTML não acompanham o pitch 65° perspectivamente — uma linha vertical no marker fica estranha contra o mapa inclinado. `fill-extrusion` é nativa, perspectiva-correta, e já é o padrão usado para prédios.

---

## Arquivos a modificar/criar

### 1. NOVO — `src/features/map/data/riskSpikes.ts`

**Helper de geometria:**
```ts
function pointToCirclePolygon(lng, lat, radiusM=9, nSegments=8): [number,number][]
// Usa: dLat = radiusM/111_320 ; dLng = dLat/cos(lat_rad)
// Loop i=0..nSegments inclusive → fecha o anel automaticamente (i=nSegments → ângulo=2π = vértice inicial)
```

**Alturas e cores por nível** (hex literal — Mapbox GL não lê CSS `var()`):
| level    | height | color   |
|----------|--------|---------|
| critical | 600m   | #d63a3a |
| high     | 400m   | #ef7a1a |
| moderate | 250m   | #f1b50a |
| low      | 100m   | #1bb892 |
| normal   |  50m   | #2c79bf |

**`addRiskSpikes(map)`:**
1. Itera `RISKS` + `RISK_DETAILS` (importados de `../../risk/data/risks`)
2. Monta dois GeoJSON FeatureCollections:
   - `SOURCE_SPIKES = 'risk-spikes'` → Polygon features (micro-círculo por estação)
   - `SOURCE_LABELS = 'risk-spike-labels'` → Point features (mesmo lng/lat)
3. `map.addSource(...)` para cada um
4. `map.addLayer(LAYER_SPIKES)` — fill-extrusion:
   - `fill-extrusion-color`: `['get', 'color']` (propriedade do GeoJSON)
   - `fill-extrusion-height`: `['get', 'height']`
   - `fill-extrusion-base`: 0
   - `fill-extrusion-opacity`: 0.88
   - `fill-extrusion-vertical-gradient`: true (luminosidade no topo)
5. `map.addLayer(LAYER_LABELS)` — symbol:
   - `text-field`: `['get', 'stationCode']` (ex.: "RH-204" — curto, legível)
   - `text-font`: `['literal', ['DIN Offc Pro Medium', 'Arial Unicode MS Bold']]` ← **obrigatório**: glyphs do dark-v11; não usar web fonts como DM Sans
   - `text-size`: 10
   - `text-anchor`: 'center'
   - `text-offset`: expressão `['match', ['get', 'level'], 'critical', ['literal',[0,-7]], 'high', ['literal',[0,-5]], 'moderate', ['literal',[0,-3.5]], 'low', ['literal',[0,-2]], ['literal',[0,-1.5]]]` — calibrado para zoom=14 / pitch=65
   - `text-color`: mesma expressão `['match',...]` com hex por nível
   - `text-halo-color`: `rgba(6,26,51,0.90)` — navy-950
   - `text-halo-width`: 1.5, `text-halo-blur`: 0.5

### 2. UPDATE — `src/features/map/services/mapService.ts`
Adicionar export:
```ts
import { addRiskSpikes } from '../data/riskSpikes'

export function getRiskSpikesCallback(): (map: mapboxgl.Map) => void {
  return addRiskSpikes
  // sem guard MOCK — os dados são estáticos (baked in bundle)
}
```

### 3. UPDATE — `src/pages/MapPage.tsx`
- Importar `getRiskSpikesCallback` junto com `getFloodLayerCallback`
- Adicionar no `handleLoad` **após** `add3dBuildings`:
```ts
getRiskSpikesCallback()(map)
```
Ordem importa: spikes após buildings → z-order correto (agulhas na frente dos prédios).

---

## Armadilhas conhecidas

| Problema | Solução |
|---|---|
| `var(--risk-*)` não funciona em expressões Mapbox GL | Usar hex literais hardcoded nas properties do GeoJSON |
| Font web (DM Sans) não existe nos glyphs do dark-v11 | Usar `DIN Offc Pro Medium` + `Arial Unicode MS Bold` |
| Labels derivam do topo ao zoom/pitch (offset é screen-space fixo) | Aceitável para o view fixo zoom=14 / pitch=65 do MapPage |
| Polygon ring precisa fechar (primeiro = último vértice) | Loop até `i <= nSegments` fecha automaticamente |
| `RISK_DETAILS` é Partial — nem todas as entradas existem | Guard `if (!detail) return []` no flatMap |

## Verificação
1. `npm run dev` → navegar para `/app/map`
2. Aguardar mapa carregar → verificar 5 agulhas coloridas nos pontos das estações
3. Zoom in/out → agulhas devem escalar perspectivamente (confirmação de `fill-extrusion` nativo)
4. Pitch interativo → agulhas devem inclinar com o mapa
5. Labels devem aparecer próximos ao topo de cada agulha
6. Legenda existente já cobre os 5 níveis — nenhuma mudança necessária
