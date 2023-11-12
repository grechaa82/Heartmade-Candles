import { Basket } from '../typesV2/order/Basket';
import { CandleDetailFilterBasketRequest } from '../typesV2/order/CandleDetailFilterBasketRequest';
import { ApiResponse } from './ApiResponse';

import { apiUrl } from '../config';

export const BasketApi = {
  getById: async (basketId: string): Promise<ApiResponse<Basket>> => {
    try {
      const response = await fetch(`${apiUrl}/baskets/${basketId}`, {
        method: 'GET',
        headers: { 'Content-Type': 'application/json' },
      });
      if (response.ok) {
        return { data: (await response.json()) as Basket, error: null };
      } else {
        return { data: null, error: await response.text() };
      }
    } catch (error) {
      throw new Error(error as string);
    }
  },
  createBasket: async (
    candleDetailFilterBasketRequest: CandleDetailFilterBasketRequest
  ): Promise<ApiResponse<string>> => {
    try {
      const response = await fetch(`${apiUrl}/baskets`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(candleDetailFilterBasketRequest),
      });
      if (response.ok) {
        var data = (await response.json()) as IdResponse;
        return { data: data.id, error: null };
      } else {
        return { data: null, error: await response.text() };
      }
    } catch (error) {
      throw new Error(error as string);
    }
  },
};

interface IdResponse {
  id: string;
}
