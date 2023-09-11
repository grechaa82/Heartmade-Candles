import { CandleDetailWithQuantityAndPrice, CreateOrderRequest } from '../typesV2/BaseProduct';

import { apiUrl } from '../config';

export const OrdersApi = {
  async get(configuredCandlesString: string): Promise<CandleDetailWithQuantityAndPrice[]> {
    const response = await fetch(`${apiUrl}/orders/${configuredCandlesString}`, {
      method: 'GET',
      headers: { 'Content-Type': 'application/json' },
    });
    return (await response.json()) as CandleDetailWithQuantityAndPrice[];
  },
  async createOrder(createOrderRequest: CreateOrderRequest): Promise<void> {
    await fetch(`${apiUrl}/orders`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(createOrderRequest),
    });
  },
};
