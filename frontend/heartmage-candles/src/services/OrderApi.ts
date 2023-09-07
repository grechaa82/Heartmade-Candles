import { CandleDetailWithQuantityAndPrice, OrderCreaterRequest } from '../typesV2/BaseProduct';

import { apiUrl } from '../config';

export const OrderApi = {
  async index(configuredCandlesString: string): Promise<CandleDetailWithQuantityAndPrice[]> {
    const response = await fetch(`${apiUrl}/order/${configuredCandlesString}`, {
      method: 'GET',
      headers: { 'Content-Type': 'application/json' },
    });
    return (await response.json()) as CandleDetailWithQuantityAndPrice[];
  },
  async createOrder(orderCreaterRequest: OrderCreaterRequest): Promise<void> {
    await fetch(`${apiUrl}/order`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(orderCreaterRequest),
    });
  },
};
