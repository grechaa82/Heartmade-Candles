import { OrderItem } from '../typesV2/order/OrderItem';
import { CreateOrderRequest } from '../typesV2/order/CreateOrderRequest';

import { apiUrl } from '../config';

export const OrdersApi = {
  async get(configuredCandlesString: string): Promise<OrderItem[]> {
    const response = await fetch(`${apiUrl}/orders/${configuredCandlesString}`, {
      method: 'GET',
      headers: { 'Content-Type': 'application/json' },
    });
    return (await response.json()) as OrderItem[];
  },
  async createOrder(createOrderRequest: CreateOrderRequest): Promise<void> {
    await fetch(`${apiUrl}/orders`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(createOrderRequest),
    });
  },
};
