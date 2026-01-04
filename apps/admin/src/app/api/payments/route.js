import { createProxyHandlers } from '@/lib/apiProxy';

const handlers = createProxyHandlers('/api/payments');

export const GET = handlers.GET;
export const POST = handlers.POST;
export const PUT = handlers.PUT;
export const DELETE = handlers.DELETE;
