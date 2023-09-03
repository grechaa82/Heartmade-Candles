import { Smell } from '../types/Smell';
import { SmellRequest } from '../types/Requests/SmellRequest';

const apiUrl = 'http://localhost:5000/api/admin/smells';

export const SmellsApi = {
  async getAll(): Promise<Smell[]> {
    const response = await fetch(`${apiUrl}`, {
      method: 'GET',
      headers: {
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
    });
    return (await response.json()) as Smell[];
  },
  async getById(id: string): Promise<Smell> {
    const response = await fetch(`${apiUrl}/${id}`, {
      method: 'GET',
      headers: {
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
    });
    return (await response.json()) as Smell;
  },
  async create(smell: SmellRequest): Promise<void> {
    await fetch(`${apiUrl}`, {
      method: 'POST',
      headers: {
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
      body: JSON.stringify(smell),
    });
  },
  async update(id: string, smell: SmellRequest): Promise<void> {
    await fetch(`${apiUrl}/${id}`, {
      method: 'PUT',
      headers: {
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
      body: JSON.stringify(smell),
    });
  },
  async delete(id: string): Promise<void> {
    await fetch(`${apiUrl}/${id}`, {
      method: 'DELETE',
      headers: {
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
    });
  },
};
