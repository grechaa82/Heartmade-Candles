import { Decor } from '../types/Decor';
import { DecorRequest } from '../types/Requests/DecorRequest';
import { ApiResponse } from './ApiResponse';
import { AuthHelper } from '../helpers/AuthHelper';

import { apiUrl } from '../config';

export const DecorsApi = {
  getAll: async (): Promise<ApiResponse<Decor[]>> => {
    try {
      const authorizationString = AuthHelper.getAuthorizationString();
      const response = await fetch(`${apiUrl}/admin/decors`, {
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
  getById: async (id: string): Promise<ApiResponse<Decor>> => {
    try {
      const authorizationString = AuthHelper.getAuthorizationString();
      const response = await fetch(`${apiUrl}/admin/decors/${id}`, {
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
  create: async (decor: DecorRequest): Promise<ApiResponse<void>> => {
    try {
      const authorizationString = AuthHelper.getAuthorizationString();
      const response = await fetch(`${apiUrl}/admin/decors`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: authorizationString,
        },
        body: JSON.stringify(decor),
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
    decor: DecorRequest,
  ): Promise<ApiResponse<void>> => {
    try {
      const authorizationString = AuthHelper.getAuthorizationString();
      const response = await fetch(`${apiUrl}/admin/decors/${id}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          Authorization: authorizationString,
        },
        body: JSON.stringify(decor),
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
      const response = await fetch(`${apiUrl}/admin/decors/${id}`, {
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
