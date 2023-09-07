import { TypeCandle } from '../types/TypeCandle';
import { TypeCandleRequest } from '../types/Requests/TypeCandleRequest';

import { apiUrl } from '../config';

export const TypeCandlesApi = {
  async getAll() {
    const response = await fetch(`${apiUrl}/admin/typeCandles`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
    });
    return (await response.json()) as TypeCandle[];
  },
  async create(typeCandle: TypeCandleRequest) {
    await fetch(`${apiUrl}/admin/typeCandles`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
      body: JSON.stringify(typeCandle),
    });
  },
  async delete(id: string) {
    await fetch(`${apiUrl}/admin/typeCandles/${id}`, {
      method: 'DELETE',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
    });
  },
};
