import { apiFetch }                                                      from '../../../shared/services/api'
import { SENSORS, SENSOR_DETAILS, type SensorEntry, type SensorDetail } from '../data/sensors'

const MOCK = import.meta.env.VITE_MOCK_DATA === 'true'

export async function getSensors(): Promise<SensorEntry[]> {
  if (MOCK) return SENSORS
  const res = await apiFetch<SensorEntry[]>('/api/sensors?page=1&pageSize=50')
  return res.data ?? []
}

export async function getSensorDetail(id: string): Promise<SensorDetail | undefined> {
  if (MOCK) return SENSOR_DETAILS[id]
  const res = await apiFetch<SensorDetail>(`/api/sensors/${id}`)
  return res.data
}
