import { NumberOfLayer } from '../types/NumberOfLayer';
import { NumberOfLayerRequest } from '../types/Requests/NumberOfLayerRequest';
import { ApiResponse } from './ApiResponse';
import { AuthHelper } from '../helpers/AuthHelper';

import { apiUrl } from '../config';

export const NumberOfLayersApi = {
  getAll: async (): Promise<NumberOfLayer[]> => {
    const response = await fetch(`${apiUrl}/admin/numberOfLayers`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: AuthHelper.getAuthorizationString(),
      },
    });

    if (!response.ok) {
      const errorBody = await response.text();
      throw new Error(`Error ${response.status}: ${errorBody}`);
    }

    return response.json();
  },

  create: async (numberOfLayer: NumberOfLayerRequest): Promise<void> => {
    const response = await fetch(`${apiUrl}/admin/numberOfLayers`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: AuthHelper.getAuthorizationString(),
      },
      body: JSON.stringify(numberOfLayer),
    });

    if (!response.ok) {
      const errorBody = await response.text();
      throw new Error(`Error ${response.status}: ${errorBody}`);
    }

    return;
  },
  delete: async (id: string): Promise<ApiResponse<void>> => {
    const response = await fetch(`${apiUrl}/admin/numberOfLayers/${id}`, {
      method: 'DELETE',
      headers: {
        'Content-Type': 'application/json',
        Authorization: AuthHelper.getAuthorizationString(),
      },
    });

    if (!response.ok) {
      const errorBody = await response.text();
      throw new Error(`Error ${response.status}: ${errorBody}`);
    }

    return;
  },
};
