import { LayerColor } from '../types/LayerColor';
import { LayerColorRequest } from '../types/Requests/LayerColorRequest';
import { ApiResponse } from './ApiResponse';
import { AuthHelper } from '../helpers/AuthHelper';

import { apiUrl } from '../config';

export const LayerColorsApi = {
  getAll: async (): Promise<ApiResponse<LayerColor[]>> => {
    try {
      const authorizationString = AuthHelper.getAuthorizationString();
      const response = await fetch(`${apiUrl}/admin/layerColors`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          Authorization: authorizationString,
        },
      });

      if (response.ok) {
        return { data: await response.json(), error: null };
      } else {
        return { data: null, error: await response.text() };
      }
    } catch (error) {
      throw new Error(error as string);
    }
  },
  getById: async (id: string): Promise<ApiResponse<LayerColor>> => {
    try {
      const authorizationString = AuthHelper.getAuthorizationString();
      const response = await fetch(`${apiUrl}/admin/layerColors/${id}`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          Authorization: authorizationString,
        },
      });

      if (response.ok) {
        return { data: await response.json(), error: null };
      } else {
        return { data: null, error: await response.text() };
      }
    } catch (error) {
      throw new Error(error as string);
    }
  },
  create: async (layerColor: LayerColorRequest): Promise<ApiResponse<void>> => {
    try {
      const authorizationString = AuthHelper.getAuthorizationString();
      const response = await fetch(`${apiUrl}/admin/layerColors`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: authorizationString,
        },
        body: JSON.stringify(layerColor),
      });

      if (response.ok) {
        return { data: null, error: null };
      } else {
        return { data: null, error: await response.text() };
      }
    } catch (error) {
      throw new Error(error as string);
    }
  },
  update: async (
    id: string,
    layerColor: LayerColorRequest,
  ): Promise<ApiResponse<void>> => {
    try {
      const authorizationString = AuthHelper.getAuthorizationString();
      const response = await fetch(`${apiUrl}/admin/layerColors/${id}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          Authorization: authorizationString,
        },
        body: JSON.stringify(layerColor),
      });

      if (response.ok) {
        return { data: null, error: null };
      } else {
        return { data: null, error: await response.text() };
      }
    } catch (error) {
      throw new Error(error as string);
    }
  },
  delete: async (id: string): Promise<ApiResponse<void>> => {
    try {
      const authorizationString = AuthHelper.getAuthorizationString();
      const response = await fetch(`${apiUrl}/admin/layerColors/${id}`, {
        method: 'DELETE',
        headers: {
          'Content-Type': 'application/json',
          Authorization: authorizationString,
        },
      });

      if (response.ok) {
        return { data: null, error: null };
      } else {
        return { data: null, error: await response.text() };
      }
    } catch (error) {
      throw new Error(error as string);
    }
  },
};
