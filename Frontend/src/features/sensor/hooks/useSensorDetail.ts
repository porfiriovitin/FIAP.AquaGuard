import { useAsync }        from '../../../shared/hooks/useAsync'
import { getSensorDetail } from '../services/sensorService'

export function useSensorDetail(id: string | null) {
  return useAsync(() => (id ? getSensorDetail(id) : Promise.resolve(undefined)), [id])
}
