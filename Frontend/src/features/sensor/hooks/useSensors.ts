import { useAsync }   from '../../../shared/hooks/useAsync'
import { getSensors } from '../services/sensorService'

export function useSensors() {
  return useAsync(getSensors)
}
