import { NumberOfLayer } from '../types/NumberOfLayer';
import { NumberOfLayerRequest } from '../types/Requests/NumberOfLayerRequest';

const apiUrl = 'http://localhost:5000/api/admin/numberOfLayers';

export const NumberOfLayersApi = {
  async getAll() {
    const response = await fetch(`${apiUrl}`, {
      method: 'GET',
      headers: {
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
    });
    return (await response.json()) as NumberOfLayer[];
  },
  async create(numberOfLayer: NumberOfLayerRequest) {
    await fetch(`${apiUrl}`, {
      method: 'POST',
      headers: {
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
      body: JSON.stringify(numberOfLayer),
    });
  },
  async delete(id: string) {
    await fetch(`${apiUrl}/${id}`, {
      method: 'DELETE',
      headers: {
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
    });
  },
};
