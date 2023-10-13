import { OrderItem } from '../typesV2/order/OrderItem';
import { CreateOrderRequest } from '../typesV2/order/CreateOrderRequest';
import { ApiResponse } from './ApiResponse';

import { apiUrl } from '../config';

export const OrdersApi = {
  get: async (configuredCandlesString: string): Promise<ApiResponse<OrderItem[]>> => {
    try {
      const response = await fetch(`${apiUrl}/orders/${configuredCandlesString}`, {
        method: 'GET',
        headers: { 'Content-Type': 'application/json' },
      });
      if (response.ok) {
        return { data: (await response.json()) as OrderItem[], error: null };
      } else {
        return { data: null, error: await response.text() };
      }
    } catch (error) {
      throw new Error(error as string);
    }
  },
  createOrder: async (createOrderRequest: CreateOrderRequest): Promise<ApiResponse<void>> => {
    try {
      const response = await fetch(`${apiUrl}/orders`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(createOrderRequest),
      });
      if (response.ok) {
        return { data: null, error: null };
      } else {
        return { data: null, error: await response.text() };
      }
    } catch (error) {
      throw new Error(error as string);
    }
  },
};
