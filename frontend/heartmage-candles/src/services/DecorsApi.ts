import { Decor } from '../types/Decor';
import { DecorRequest } from '../types/Requests/DecorRequest';
import { AuthHelper } from '../helpers/AuthHelper';
import { PaginationSettings } from '../typesV2/shared/PaginationSettings';

import { apiUrl } from '../config';

export const DecorsApi = {
  getAll: async (
    pagination: PaginationSettings = { pageSize: 20, pageIndex: 0 },
  ): Promise<[Decor[], number | null]> => {
    const queryParams = new URLSearchParams();
    queryParams.append('Limit', pagination.pageSize.toString());
    queryParams.append('Index', pagination.pageIndex.toString());

    const response = await fetch(
      `${apiUrl}/admin/decors?${queryParams.toString()}`,
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

    const data: Decor[] = await response.json();
    const totalCountHeader = response.headers.get('X-Total-Count');
    const totalCount = totalCountHeader ? parseInt(totalCountHeader, 10) : null;

    return [data, totalCount];
  },

  getById: async (id: string): Promise<Decor> => {
    const response = await fetch(`${apiUrl}/admin/decors/${id}`, {
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

  create: async (decor: DecorRequest): Promise<void> => {
    const response = await fetch(`${apiUrl}/admin/decors`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: AuthHelper.getAuthorizationString(),
      },
      body: JSON.stringify(decor),
    });

    if (!response.ok) {
      const errorBody = await response.text();
      throw new Error(`Error ${response.status}: ${errorBody}`);
    }

    return;
  },

  update: async (id: string, decor: DecorRequest): Promise<void> => {
    const response = await fetch(`${apiUrl}/admin/decors/${id}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        Authorization: AuthHelper.getAuthorizationString(),
      },
      body: JSON.stringify(decor),
    });

    if (!response.ok) {
      const errorBody = await response.text();
      throw new Error(`Error ${response.status}: ${errorBody}`);
    }

    return;
  },

  delete: async (id: string): Promise<void> => {
    const response = await fetch(`${apiUrl}/admin/decors/${id}`, {
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
