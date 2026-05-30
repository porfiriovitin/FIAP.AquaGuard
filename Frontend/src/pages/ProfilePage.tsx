import { useState } from 'react'
import {
  User, Lock, ShieldCheck,
  Bell, FileText, MessageSquare,
  Timer, HelpCircle, Scale, LogOut,
} from 'lucide-react'
import { USER_PROFILE, ALERT_COUNT, SENSOR_COUNT } from '../features/profile/data/profile'
import { ProfileHeader }   from '../features/profile/components/ProfileHeader'
import { StatsRow }        from '../features/profile/components/StatsRow'
import { SettingsSection } from '../features/profile/components/SettingsSection'
import { SettingsRow }     from '../features/profile/components/SettingsRow'

export function ProfilePage() {
  const [prefs, setPrefs] = useState({
    criticalAlerts: true,
    dailyReports:   false,
    smsAlerts:      true,
  })

  const toggle = (key: keyof typeof prefs) =>
    setPrefs(p => ({ ...p, [key]: !p[key] }))

  return (
    <div className="flex flex-col gap-6 px-5 pt-5 pb-6 max-w-screen-xl mx-auto">

      <ProfileHeader
        name={USER_PROFILE.name}
        role={USER_PROFILE.role}
        initials={USER_PROFILE.initials}
      />

      <StatsRow alertCount={ALERT_COUNT} sensorCount={SENSOR_COUNT} />

      <SettingsSection title="CONTA E SEGURANÇA">
        <SettingsRow icon={User}        label="Dados Pessoais"              variant="link" />
        <SettingsRow icon={Lock}        label="Alterar Senha"               variant="link" />
        <SettingsRow icon={ShieldCheck} label="Autenticação em duas etapas" variant="link" />
      </SettingsSection>

      <SettingsSection title="PREFERÊNCIAS DE ALERTA">
        <SettingsRow
          icon={Bell}
          label="Notificações Críticas"
          variant="toggle"
          checked={prefs.criticalAlerts}
          onToggle={() => toggle('criticalAlerts')}
        />
        <SettingsRow
          icon={FileText}
          label="Relatórios Diários"
          variant="toggle"
          checked={prefs.dailyReports}
          onToggle={() => toggle('dailyReports')}
        />
        <SettingsRow
          icon={MessageSquare}
          label="Alertas via SMS"
          variant="toggle"
          checked={prefs.smsAlerts}
          onToggle={() => toggle('smsAlerts')}
        />
      </SettingsSection>

      <SettingsSection title="CONFIGURAÇÕES TÉCNICAS">
        <SettingsRow
          icon={Timer}
          label="Frequência de Telemetria"
          variant="value"
          valueText={USER_PROFILE.telemetryInterval}
        />
      </SettingsSection>

      <SettingsSection>
        <SettingsRow icon={HelpCircle} label="Centro de Ajuda" variant="link" />
        <SettingsRow icon={Scale}      label="Termos de Uso"   variant="link"   />
        <SettingsRow icon={LogOut}     label="Sair"            variant="danger" />
      </SettingsSection>

    </div>
  )
}
