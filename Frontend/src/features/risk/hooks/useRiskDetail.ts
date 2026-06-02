import { useAsync }      from '../../../shared/hooks/useAsync'
import { getRiskDetail } from '../services/riskService'

export function useRiskDetail(id: string | null) {
  return useAsync(() => (id ? getRiskDetail(id) : Promise.resolve(undefined)), [id])
}
