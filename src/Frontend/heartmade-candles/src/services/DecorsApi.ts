import { Decor } from '../types/Decor';
import { DecorRequest } from '../types/Requests/DecorRequest';

const apiUrl = 'http://localhost:80/api/admin/decors';

export const DecorsApi = {
  async getAll(): Promise<Decor[]> {
    const response = await fetch(`${apiUrl}`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
    });
    return (await response.json()) as Decor[];
  },
  async getById(id: string): Promise<Decor> {
    const response = await fetch(`${apiUrl}/${id}`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
    });
    return (await response.json()) as Decor;
  },
  async create(decor: DecorRequest): Promise<void> {
    await fetch(`${apiUrl}`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
      body: JSON.stringify(decor),
    });
  },
  async update(id: string, decor: DecorRequest): Promise<void> {
    await fetch(`${apiUrl}/${id}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
      body: JSON.stringify(decor),
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
