import { Wick } from '../types/Wick';
import { WickRequest } from '../types/Requests/WickRequest';
import { AuthHelper } from '../helpers/AuthHelper';
import { PaginationSettings } from '../typesV2/shared/PaginationSettings';

import { apiUrl } from '../config';

export const WicksApi = {
  getAll: async (
    pagination: PaginationSettings = { pageSize: 20, pageIndex: 0 },
  ): Promise<[Wick[], number | null]> => {
    const queryParams = new URLSearchParams();

    queryParams.append('Limit', pagination.pageSize.toString());
    queryParams.append('Index', pagination.pageIndex.toString());

    const response = await fetch(
      `${apiUrl}/admin/wicks?${queryParams.toString()}`,
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

    const data: Wick[] = await response.json();
    const totalCountHeader = response.headers.get('X-Total-Count');
    const totalCount = totalCountHeader ? parseInt(totalCountHeader, 10) : null;

    return [data, totalCount];
  },

  getById: async (id: string): Promise<Wick> => {
    const response = await fetch(`${apiUrl}/admin/wicks/${id}`, {
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

  create: async (wick: WickRequest): Promise<void> => {
    const response = await fetch(`${apiUrl}/admin/wicks`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: AuthHelper.getAuthorizationString(),
      },
      body: JSON.stringify(wick),
    });

    if (!response.ok) {
      const errorBody = await response.text();
      throw new Error(`Error ${response.status}: ${errorBody}`);
    }

    return;
  },

  update: async (id: string, wick: WickRequest): Promise<void> => {
    const response = await fetch(`${apiUrl}/admin/wicks/${id}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        Authorization: AuthHelper.getAuthorizationString(),
      },
      body: JSON.stringify(wick),
    });

    if (!response.ok) {
      const errorBody = await response.text();
      throw new Error(`Error ${response.status}: ${errorBody}`);
    }

    return;
  },

  delete: async (id: string): Promise<void> => {
    const response = await fetch(`${apiUrl}/admin/wicks/${id}`, {
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
