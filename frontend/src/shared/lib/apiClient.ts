const baseUrl = import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:5080'

/**
 * Tüm HTTP çağrılarının tek kapısı. Component'ler fetch'i doğrudan çağırmaz;
 * base URL, hata normalizasyonu ve JSON çözümleme burada yaşar.
 */

async function parseError(response: Response): Promise<never> {
  // Backend hata gövdesinde { title, detail } olabilir (ProblemDetails benzeri).
  let message = `API hatası (${response.status})`
  try {
    const body = await response.json()
    if (body?.title) {
      message = body.detail ? `${body.title}: ${body.detail}` : body.title
    }
  } catch {
    // JSON değilse varsayılan mesaj kalır.
  }
  throw new Error(message)
}

export async function apiGet<T>(path: string): Promise<T> {
  const response = await fetch(`${baseUrl}${path}`)
  if (!response.ok) {
    return parseError(response)
  }
  return (await response.json()) as T
}

export async function apiPost<T>(path: string, body?: unknown): Promise<T> {
  const response = await fetch(`${baseUrl}${path}`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: body === undefined ? undefined : JSON.stringify(body),
  })
  if (!response.ok) {
    return parseError(response)
  }
  // 204 No Content veya boş gövde durumunda json() patlamasın.
  const text = await response.text()
  return (text ? JSON.parse(text) : undefined) as T
}

export async function apiDelete(path: string): Promise<void> {
  const response = await fetch(`${baseUrl}${path}`, { method: 'DELETE' })
  if (!response.ok) {
    return parseError(response)
  }
}
