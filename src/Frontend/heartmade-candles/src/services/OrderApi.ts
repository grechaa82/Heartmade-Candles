import {
  CandleDetailWithQuantityAndPrice,
  OrderCreaterRequest as OrderCreatorRequest,
} from '../typesV2/BaseProduct';

const apiUrl = 'http://localhost:80/api/order';

export const OrderApi = {
  async index(configuredCandlesString: string): Promise<CandleDetailWithQuantityAndPrice[]> {
    const response = await fetch(`${apiUrl}/${configuredCandlesString}`, {
      method: 'GET',
      headers: { 'Content-Type': 'application/json' },
    });
    return (await response.json()) as CandleDetailWithQuantityAndPrice[];
  },
  async createOrder(orderCreaterRequest: OrderCreatorRequest): Promise<void> {
    await fetch(`${apiUrl}`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(orderCreaterRequest),
    });
  },
};
