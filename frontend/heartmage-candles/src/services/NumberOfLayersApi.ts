import { NumberOfLayer } from '../types/NumberOfLayer';
import { NumberOfLayerRequest } from '../types/Requests/NumberOfLayerRequest';

import { apiUrl } from '../config';

export const NumberOfLayersApi = {
  async getAll() {
    const response = await fetch(`${apiUrl}/admin/numberOfLayers`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
    });
    return (await response.json()) as NumberOfLayer[];
  },
  async create(numberOfLayer: NumberOfLayerRequest) {
    await fetch(`${apiUrl}/admin/numberOfLayers`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
      body: JSON.stringify(numberOfLayer),
    });
  },
  async delete(id: string) {
    await fetch(`${apiUrl}/admin/numberOfLayers/${id}`, {
      method: 'DELETE',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
    });
  },
};
