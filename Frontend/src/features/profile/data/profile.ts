import { RISKS, isUrgent } from '../../risk/data/risks'
import { SENSORS } from '../../sensor/data/sensors'

export const USER_PROFILE = {
  name:              'Eng. Ricardo Silva',
  role:              'Coordenador de Operações – Bacia do Tietê',
  initials:          'RS',
  telemetryInterval: '60s',
}

export const ALERT_COUNT  = RISKS.filter(r => isUrgent(r.level)).length
export const SENSOR_COUNT = SENSORS.length
