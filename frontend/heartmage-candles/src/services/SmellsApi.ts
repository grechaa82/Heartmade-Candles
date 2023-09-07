import { Smell } from '../types/Smell';
import { SmellRequest } from '../types/Requests/SmellRequest';

import { apiUrl } from '../config';

export const SmellsApi = {
  async getAll(): Promise<Smell[]> {
    const response = await fetch(`${apiUrl}/admin/smells`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
    });
    return (await response.json()) as Smell[];
  },
  async getById(id: string): Promise<Smell> {
    const response = await fetch(`${apiUrl}/admin/smells/${id}`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
    });
    return (await response.json()) as Smell;
  },
  async create(smell: SmellRequest): Promise<void> {
    await fetch(`${apiUrl}/admin/smells`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
      body: JSON.stringify(smell),
    });
  },
  async update(id: string, smell: SmellRequest): Promise<void> {
    await fetch(`${apiUrl}/admin/smells/${id}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
      body: JSON.stringify(smell),
    });
  },
  async delete(id: string): Promise<void> {
    await fetch(`${apiUrl}/admin/smells/${id}`, {
      method: 'DELETE',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
    });
  },
};
