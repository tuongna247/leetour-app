import { createProxyHandlers } from '@/lib/apiProxy';

const handlers = createProxyHandlers('/api/bookings');

export const GET = handlers.GET;
export const PUT = handlers.PUT;
export const DELETE = handlers.DELETE;
