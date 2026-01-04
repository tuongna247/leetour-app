import { proxyToAPI } from '@/lib/apiProxy';

export async function POST(request) {
  return proxyToAPI(request, '/api/auth/login', { method: 'POST' });
}
