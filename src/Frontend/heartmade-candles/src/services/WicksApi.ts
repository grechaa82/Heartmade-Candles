import { Wick } from '../types/Wick';
import { WickRequest } from '../types/Requests/WickRequest';

const apiUrl = 'http://localhost:80/api/admin/wicks';

export const WicksApi = {
  async getAll(): Promise<Wick[]> {
    const response = await fetch(`${apiUrl}`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
    });
    return (await response.json()) as Wick[];
  },
  async getById(id: string): Promise<Wick> {
    const response = await fetch(`${apiUrl}/${id}`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
    });
    return (await response.json()) as Wick;
  },
  async create(wick: WickRequest): Promise<void> {
    await fetch(`${apiUrl}`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
      body: JSON.stringify(wick),
    });
  },
  async update(id: string, wick: WickRequest): Promise<void> {
    await fetch(`${apiUrl}/${id}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
      body: JSON.stringify(wick),
    });
  },
  async delete(id: string): Promise<void> {
    await fetch(`${apiUrl}/${id}`, {
      method: 'DELETE',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
    });
  },
};
