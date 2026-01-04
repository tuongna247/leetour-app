import { proxyToAPI } from '@/lib/apiProxy';

export async function GET(request) {
  return proxyToAPI(request, '/api/auth/me');
}