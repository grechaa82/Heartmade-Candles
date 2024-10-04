import { LayerColor } from '../types/LayerColor';
import { LayerColorRequest } from '../types/Requests/LayerColorRequest';
import { AuthHelper } from '../helpers/AuthHelper';
import { PaginationSettings } from '../typesV2/shared/PaginationSettings';

import { apiUrl } from '../config';

export const LayerColorsApi = {
  getAll: async (
    pagination: PaginationSettings = { pageSize: 20, pageIndex: 0 },
  ): Promise<LayerColor[]> => {
    const queryParams = new URLSearchParams();

    queryParams.append('Limit', pagination.pageSize.toString());
    queryParams.append('Index', pagination.pageIndex.toString());

    const response = await fetch(
      `${apiUrl}/admin/layerColors?${queryParams.toString()}`,
      {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          Authorization: AuthHelper.getAuthorizationString(),
        },
      },
    );

    if (!response.ok) {
      const errorBody = await response.text();
      throw new Error(`Error ${response.status}: ${errorBody}`);
    }

    return response.json();
  },

  getById: async (id: string): Promise<LayerColor> => {
    const response = await fetch(`${apiUrl}/admin/layerColors/${id}`, {
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

  create: async (layerColor: LayerColorRequest): Promise<void> => {
    const response = await fetch(`${apiUrl}/admin/layerColors`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: AuthHelper.getAuthorizationString(),
      },
      body: JSON.stringify(layerColor),
    });

    if (!response.ok) {
      const errorBody = await response.text();
      throw new Error(`Error ${response.status}: ${errorBody}`);
    }

    return;
  },

  update: async (id: string, layerColor: LayerColorRequest): Promise<void> => {
    const response = await fetch(`${apiUrl}/admin/layerColors/${id}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        Authorization: AuthHelper.getAuthorizationString(),
      },
      body: JSON.stringify(layerColor),
    });

    if (!response.ok) {
      const errorBody = await response.text();
      throw new Error(`Error ${response.status}: ${errorBody}`);
    }

    return;
  },

  delete: async (id: string): Promise<void> => {
    const response = await fetch(`${apiUrl}/admin/layerColors/${id}`, {
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
