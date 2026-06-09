
import { addFloodMockLayers } from '../data/floodLayers'

const MOCK = import.meta.env.VITE_MOCK_DATA === 'true'

// Returns the onLoad callback to inject flood-risk layers into a MapView.
// Mock: 4 concentric GeoJSON zones centred on SP.
// Real: undefined for now — future implementation fetches zones from the API.
export function getFloodLayerCallback(): ((map: mapboxgl.Map) => void) | undefined {
  if (MOCK) return addFloodMockLayers
  return undefined
}
