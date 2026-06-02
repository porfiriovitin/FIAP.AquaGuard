import { useState, useEffect } from 'react'

export function useAsync<T>(fn: () => Promise<T>, deps: unknown[] = []) {
  const [data,      setData]      = useState<T | undefined>(undefined)
  const [isLoading, setIsLoading] = useState(true)
  const [error,     setError]     = useState<Error | null>(null)

  useEffect(() => {
    let cancelled = false
    setIsLoading(true)
    setError(null)
    fn()
      .then(r => { if (!cancelled) { setData(r);   setIsLoading(false) } })
      .catch(e => { if (!cancelled) { setError(e); setIsLoading(false) } })
    return () => { cancelled = true }
  }, deps) // eslint-disable-line react-hooks/exhaustive-deps

  return { data, isLoading, error }
}
