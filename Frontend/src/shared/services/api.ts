const BASE_URL = import.meta.env.VITE_API_BASE_URL as string

export interface PayloadResponse<T> {
  status: 'Success' | 'Error'
  data?: T
  message?: string
}

export async function apiFetch<T>(
  path: string,
  options?: RequestInit,
): Promise<PayloadResponse<T>> {
  const res = await fetch(`${BASE_URL}${path}`, {
    ...options,
    // Required so the browser attaches and stores the HttpOnly JWT cookie on every request.
    credentials: 'include',
    headers: { 'Content-Type': 'application/json', ...options?.headers },
  })

  if (res.status === 401) {
    // Decouples the HTTP layer from AuthContext: any component can react to session expiry
    // without a direct import dependency on the auth module.
    window.dispatchEvent(new Event('auth:unauthorized'))
    throw new Error('Unauthorized')
  }

  return res.json() as Promise<PayloadResponse<T>>
}
