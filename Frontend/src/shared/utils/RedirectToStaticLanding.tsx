import { useEffect } from 'react'

export function RedirectToStaticLanding() {
  useEffect(() => {
    window.location.replace('/landingpage/index.html')
  }, [])

  return null
}
