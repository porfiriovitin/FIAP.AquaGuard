import { apiFetch }                                             from '../../../shared/services/api'
import { RISKS, RISK_DETAILS, type RiskEntry, type RiskDetail } from '../data/risks'

const MOCK = import.meta.env.VITE_MOCK_DATA === 'true'

export async function getRisks(): Promise<RiskEntry[]> {
  if (MOCK) return RISKS
  const res = await apiFetch<RiskEntry[]>('/api/risk?latitude=-23.55&longitude=-46.63')
  return res.data ?? []
}

export async function getRiskDetail(id: string): Promise<RiskDetail | undefined> {
  if (MOCK) return RISK_DETAILS[id]
  const res = await apiFetch<RiskDetail>(`/api/risk/${id}`)
  return res.data
}
