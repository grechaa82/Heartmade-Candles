import { TypeCandle } from '../types/TypeCandle';
import { TypeCandleRequest } from '../types/Requests/TypeCandleRequest';

const apiUrl = 'http://localhost:80/api/admin/typeCandles';

export const TypeCandlesApi = {
  async getAll() {
    const response = await fetch(`${apiUrl}`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
    });
    return (await response.json()) as TypeCandle[];
  },
  async create(typeCandle: TypeCandleRequest) {
    await fetch(`${apiUrl}`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
      body: JSON.stringify(typeCandle),
    });
  },
  async delete(id: string) {
    await fetch(`${apiUrl}/${id}`, {
      method: 'DELETE',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
    });
  },
};
