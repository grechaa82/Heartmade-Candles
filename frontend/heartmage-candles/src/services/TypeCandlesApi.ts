import { TypeCandle } from '../types/TypeCandle';
import { TypeCandleRequest } from '../types/Requests/TypeCandleRequest';
import { AuthHelper } from '../helpers/AuthHelper';

import { apiUrl } from '../config';

export const TypeCandlesApi = {
  getAll: async (): Promise<TypeCandle[]> => {
    const response = await fetch(`${apiUrl}/admin/typeCandles`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: AuthHelper.getAuthorizationString(),
      },
    });

    if (!response.ok) {
      const errorBody = await response.text();
      throw new Error(`Error ${response.status}: ${errorBody}`);
    }

    return response.json();
  },
  create: async (typeCandle: TypeCandleRequest): Promise<void> => {
    const response = await fetch(`${apiUrl}/admin/typeCandles`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: AuthHelper.getAuthorizationString(),
      },
      body: JSON.stringify(typeCandle),
    });

    if (!response.ok) {
      const errorBody = await response.text();
      throw new Error(`Error ${response.status}: ${errorBody}`);
    }

    return;
  },
  delete: async (id: string): Promise<void> => {
    const response = await fetch(`${apiUrl}/admin/typeCandles/${id}`, {
      method: 'DELETE',
      headers: {
        'Content-Type': 'application/json',
        Authorization: AuthHelper.getAuthorizationString(),
      },
    });

    if (!response.ok) {
      const errorBody = await response.text();
      throw new Error(`Error ${response.status}: ${errorBody}`);
    }

    return;
  },
};
