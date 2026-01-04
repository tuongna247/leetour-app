import { proxyToAPI } from '@/lib/apiProxy';

export async function POST(request, context) {
  const { params } = context || {};
  const tourId = params?.id;
  return proxyToAPI(request, `/api/tours/${tourId}/reviews`, { method: 'POST' });
}

export async function GET(request, context) {
  const { params } = context || {};
  const tourId = params?.id;
  return proxyToAPI(request, `/api/tours/${tourId}/reviews`);
}
