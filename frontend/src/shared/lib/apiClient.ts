const baseUrl = import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:5080'

/**
 * Tüm HTTP çağrılarının tek kapısı. Component'ler fetch'i doğrudan çağırmaz;
 * base URL, hata normalizasyonu ve JSON çözümleme burada yaşar.
 */
export async function apiGet<T>(path: string): Promise<T> {
  const response = await fetch(`${baseUrl}${path}`)

  if (!response.ok) {
    throw new Error(`API hatası (${response.status})`)
  }

  return (await response.json()) as T
}
