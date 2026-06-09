import { useRef, useState, useMemo } from 'react'
import { BrowserRouter, Routes, Route, Navigate, useLocation } from 'react-router-dom'
import { AnimatePresence, motion } from 'framer-motion'
import { TopAppBar } from './shared/components/layout/TopAppBar'
import { BottomNavBar } from './shared/components/layout/BottomNavBar'
import { TAB_ORDER } from './shared/config/navTabs'
import { isUrgent } from './features/risk/data/risks'
import { useRisks } from './features/risk/hooks/useRisks'
import { HomePage } from './pages/HomePage'
import { RisksPage } from './pages/RisksPage'
import { SensorsPage } from './pages/SensorsPage'
import { ProfilePage } from './pages/ProfilePage'
import { LoginPage } from './pages/LoginPage'
import { MapPage } from './pages/MapPage'
import { SensorDetailModal } from './features/sensor/components/SensorDetailModal'
import { RiskDetailModal } from './features/risk/components/RiskDetailModal'
import { AuthProvider } from './features/auth/context/AuthContext'
import { ProtectedRoute } from './shared/components/auth/ProtectedRoute'
import { RedirectToStaticLanding } from './shared/utils/RedirectToStaticLanding'

const HEADER_HEIGHT = '65px'
const NAV_HEIGHT    = '75px'

const EASE_OUT: [number, number, number, number] = [0.25, 0.46, 0.45, 0.94]
const EASE_IN:  [number, number, number, number] = [0.55, 0.06, 0.68, 0.19]

const pageVariants = {
  initial: (direction: number) => ({
    x: direction >= 0 ? 32 : -32,
    opacity: 0,
  }),
  animate: {
    x: 0,
    opacity: 1,
    transition: { duration: 0.25, ease: EASE_OUT },
  },
  exit: (direction: number) => ({
    x: direction >= 0 ? -32 : 32,
    opacity: 0,
    transition: { duration: 0.18, ease: EASE_IN },
  }),
}

function AppLayout() {
  const location = useLocation()

  const prevPathnameRef = useRef(location.pathname)
  const directionRef    = useRef(0)

  const [selectedSensorId, setSelectedSensorId] = useState<string | null>(null)
  const [selectedRiskId,   setSelectedRiskId]   = useState<string | null>(null)

  const { data: risks = [] } = useRisks()
  const alertPaths = useMemo(
    () => new Set(risks.some(r => isUrgent(r.level)) ? ['/app/risks'] : []),
    [risks],
  )

  if (prevPathnameRef.current !== location.pathname) {
    const from = TAB_ORDER.indexOf(prevPathnameRef.current)
    const to   = TAB_ORDER.indexOf(location.pathname)
    directionRef.current    = (from < 0 || to < 0) ? 0 : to >= from ? 1 : -1
    prevPathnameRef.current = location.pathname
  }

  return (
    <div className="min-h-[100dvh] flex flex-col bg-[var(--bg)]">
      <TopAppBar />

      <main
        className="flex-1 overflow-y-auto overflow-x-hidden"
        style={{
          paddingTop:    HEADER_HEIGHT,
          paddingBottom: `calc(${NAV_HEIGHT} + env(safe-area-inset-bottom))`,
        }}
      >
        <AnimatePresence mode="wait" custom={directionRef.current}>
          <motion.div
            key={location.pathname}
            custom={directionRef.current}
            variants={pageVariants}
            initial="initial"
            animate="animate"
            exit="exit"
          >
            <Routes location={location}>
              <Route index          element={<HomePage    onSelectSensor={setSelectedSensorId} onSelectRisk={setSelectedRiskId} />} />
              <Route path="risks"   element={<RisksPage   onSelectRisk={setSelectedRiskId} />} />
              <Route path="map"     element={<MapPage />} />
              <Route path="sensors" element={<SensorsPage onSelectSensor={setSelectedSensorId} />} />
              <Route path="profile" element={<ProfilePage />} />
            </Routes>
          </motion.div>
        </AnimatePresence>
      </main>

      <BottomNavBar alertPaths={alertPaths} />

      <AnimatePresence>
        {selectedSensorId && (
          <SensorDetailModal
            key={selectedSensorId}
            sensorId={selectedSensorId}
            onClose={() => setSelectedSensorId(null)}
          />
        )}
      </AnimatePresence>

      <AnimatePresence>
        {selectedRiskId && (
          <RiskDetailModal
            key={selectedRiskId}
            riskId={selectedRiskId}
            onClose={() => setSelectedRiskId(null)}
          />
        )}
      </AnimatePresence>
    </div>
  )
}

export default function App() {
  return (
    <BrowserRouter>
      <AuthProvider>
        <Routes>
          <Route path="/login" element={<LoginPage />} />
          <Route path="/" element={<Navigate to="/app" replace />} />
          <Route path="/landingpage" element={<RedirectToStaticLanding />} />
          <Route
            path="/app/*"
            element={
              <ProtectedRoute>
                <AppLayout />
              </ProtectedRoute>
            }
          />
        </Routes>
      </AuthProvider>
    </BrowserRouter>
  )
}
