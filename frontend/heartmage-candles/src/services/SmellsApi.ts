import { Smell } from '../types/Smell';
import { SmellRequest } from '../types/Requests/SmellRequest';
import { ApiResponse } from './ApiResponse';
import { AuthHelper } from '../helpers/AuthHelper';

import { apiUrl } from '../config';

export const SmellsApi = {
  getAll: async (): Promise<Smell[]> => {
    const response = await fetch(`${apiUrl}/admin/smells`, {
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

  getById: async (id: string): Promise<Smell> => {
    const response = await fetch(`${apiUrl}/admin/smells/${id}`, {
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

  create: async (smell: SmellRequest): Promise<void> => {
    const response = await fetch(`${apiUrl}/admin/smells`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: AuthHelper.getAuthorizationString(),
      },
      body: JSON.stringify(smell),
    });

    if (!response.ok) {
      const errorBody = await response.text();
      throw new Error(`Error ${response.status}: ${errorBody}`);
    }

    return;
  },

  update: async (id: string, smell: SmellRequest): Promise<void> => {
    const response = await fetch(`${apiUrl}/admin/smells/${id}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        Authorization: AuthHelper.getAuthorizationString(),
      },
      body: JSON.stringify(smell),
    });

    if (!response.ok) {
      const errorBody = await response.text();
      throw new Error(`Error ${response.status}: ${errorBody}`);
    }

    return;
  },

  delete: async (id: string): Promise<ApiResponse<void>> => {
    const response = await fetch(`${apiUrl}/admin/smells/${id}`, {
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
