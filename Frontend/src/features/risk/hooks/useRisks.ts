import { useAsync } from '../../../shared/hooks/useAsync'
import { getRisks } from '../services/riskService'

export function useRisks() {
  return useAsync(getRisks)
}
