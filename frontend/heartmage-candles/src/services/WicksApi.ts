import { Wick } from '../types/Wick';
import { WickRequest } from '../types/Requests/WickRequest';

import { apiUrl } from '../config';

export const WicksApi = {
  async getAll(): Promise<Wick[]> {
    const response = await fetch(`${apiUrl}/admin/wicks`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
    });
    return (await response.json()) as Wick[];
  },
  async getById(id: string): Promise<Wick> {
    const response = await fetch(`${apiUrl}/admin/wicks/${id}`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
    });
    return (await response.json()) as Wick;
  },
  async create(wick: WickRequest): Promise<void> {
    await fetch(`${apiUrl}/admin/wicks`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
      body: JSON.stringify(wick),
    });
  },
  async update(id: string, wick: WickRequest): Promise<void> {
    await fetch(`${apiUrl}/admin/wicks/${id}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
      body: JSON.stringify(wick),
    });
  },
  async delete(id: string): Promise<void> {
    await fetch(`${apiUrl}/admin/wicks/${id}`, {
      method: 'DELETE',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
    });
  },
};
