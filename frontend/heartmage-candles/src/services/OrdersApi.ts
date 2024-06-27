import { CreateOrderRequest } from '../typesV2/order/CreateOrderRequest';
import { Order } from '../typesV2/shared/Order';
import { OrdersAndTotalCount } from '../typesV2/shared/OrdersAndTotalCount';
import { OrderTableParameters } from '../typesV2/order/OrderTableParametersRequest';

import { AuthHelper } from '../helpers/AuthHelper';
import { ApiResponse } from './ApiResponse';

import { apiUrl } from '../config';

export const OrdersApi = {
  getAll: async (
    queryParams: OrderTableParameters
  ): Promise<ApiResponse<OrdersAndTotalCount>> => {
    try {
      const authorizationString = AuthHelper.getAuthorizationString();
      const response = await fetch(
        `${apiUrl}/admin/orders?${queryParams.toQueryString()}`,
        {
          method: 'GET',
          headers: {
            'Content-Type': 'application/json',
            Authorization: authorizationString,
          },
        }
      );

      if (response.ok) {
        return {
          data: (await response.json()) as OrdersAndTotalCount,
          error: null,
        };
      } else {
        return { data: null, error: await response.text() };
      }
    } catch (error) {
      throw new Error(error as string);
    }
  },
  getById: async (
    configuredCandlesString: string
  ): Promise<ApiResponse<Order>> => {
    try {
      const response = await fetch(
        `${apiUrl}/orders/${configuredCandlesString}`,
        {
          method: 'GET',
          headers: { 'Content-Type': 'application/json' },
        }
      );
      if (response.ok) {
        return { data: (await response.json()) as Order, error: null };
      } else {
        return { data: null, error: await response.text() };
      }
    } catch (error) {
      throw new Error(error as string);
    }
  },

  createOrder: async (
    createOrderRequest: CreateOrderRequest
  ): Promise<ApiResponse<string>> => {
    try {
      const response = await fetch(`${apiUrl}/orders`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(createOrderRequest),
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
