import { LayerColor } from '../types/LayerColor';
import { LayerColorRequest } from '../types/Requests/LayerColorRequest';

import { apiUrl } from '../config';

export const LayerColorsApi = {
  async getAll(): Promise<LayerColor[]> {
    const response = await fetch(`${apiUrl}/admin/layerColors`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
    });
    return (await response.json()) as LayerColor[];
  },
  async getById(id: string): Promise<LayerColor> {
    const response = await fetch(`${apiUrl}/admin/layerColors/${id}`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
    });
    return (await response.json()) as LayerColor;
  },
  async create(layerColor: LayerColorRequest): Promise<void> {
    await fetch(`${apiUrl}/admin/layerColors`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
      body: JSON.stringify(layerColor),
    });
  },
  async update(id: string, layerColor: LayerColorRequest): Promise<void> {
    await fetch(`${apiUrl}/admin/layerColors/${id}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
      body: JSON.stringify(layerColor),
    });
  },
  async delete(id: string): Promise<void> {
    await fetch(`${apiUrl}/admin/layerColors/${id}`, {
      method: 'DELETE',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
    });
  },
};
