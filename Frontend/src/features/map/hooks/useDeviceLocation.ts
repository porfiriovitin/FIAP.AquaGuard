import { useState, useEffect } from 'react'

export type LocationStatus = 'disabled' | 'loading' | 'granted' | 'denied' | 'unavailable'

export interface DeviceLocation {
  // [longitude, latitude] — Mapbox convention (lng first)
  coords: [number, number] | null
  status: LocationStatus
}

interface Options {
  // When false, the hook skips the geolocation API entirely (no permission prompt)
  // and stays in 'disabled' status. Default true preserves prior behavior.
  enabled?: boolean
}

const GEO_OPTIONS: PositionOptions = {
  enableHighAccuracy: true,
  timeout:            10_000,
  // Accept a cached position up to 1 minute old to avoid redundant GPS calls
  maximumAge:         60_000,
}

export function useDeviceLocation({ enabled = true }: Options = {}): DeviceLocation {
  const [location, setLocation] = useState<DeviceLocation>({
    coords: null,
    status: enabled ? 'loading' : 'disabled',
  })

  useEffect(() => {
    if (!enabled) {
      setLocation({ coords: null, status: 'disabled' })
      return
    }

    if (!navigator.geolocation) {
      setLocation({ coords: null, status: 'unavailable' })
      return
    }

    setLocation(prev => prev.status === 'loading' ? prev : { coords: null, status: 'loading' })

    navigator.geolocation.getCurrentPosition(
      (position) => {
        setLocation({
          coords: [position.coords.longitude, position.coords.latitude],
          status: 'granted',
        })
      },
      (error) => {
        // Compare to the error instance's own constant (universally supported)
        // instead of the constructor's — some browsers don't expose statics.
        const status = error.code === error.PERMISSION_DENIED ? 'denied' : 'unavailable'
        setLocation({ coords: null, status })
      },
      GEO_OPTIONS,
    )
  }, [enabled])

  return location
}
