import { Candle } from '../types/Candle';
import { CandleDetail } from '../types/CandleDetail';
import { CandleRequest } from '../types/Requests/CandleRequest';
import { ApiResponse } from './ApiResponse';
import { AuthHelper } from '../helpers/AuthHelper';
import { PaginationSettings } from '../typesV2/shared/PaginationSettings';

import { apiUrl } from '../config';

export const CandlesApi = {
  getAll: async (
    typeFilter?: string,
    pagination: PaginationSettings = { pageSize: 20, pageIndex: 0 },
  ): Promise<Candle[]> => {
    try {
      const authorizationString = AuthHelper.getAuthorizationString();
      const queryParams = new URLSearchParams();

      if (typeFilter) {
        queryParams.append('TypeFilter', typeFilter);
      }
      queryParams.append(
        'PaginationSettings.PageSize',
        pagination.pageSize.toString(),
      );
      queryParams.append(
        'PaginationSettings.PageIndex',
        pagination.pageIndex.toString(),
      );

      const response = await fetch(
        `${apiUrl}/admin/candles?${queryParams.toString()}`,
        {
          method: 'GET',
          headers: {
            'Content-Type': 'application/json',
            Authorization: authorizationString,
          },
        },
      );

      if (!response.ok) {
        const errorBody = await response.text();
        throw new Error(`Ошибка ${response.status}: ${errorBody}`);
      }

      return response.json();
    } catch (error) {
      throw new Error(error as string);
    }
  },

  getById: async (id: string): Promise<ApiResponse<CandleDetail>> => {
    try {
      const authorizationString = AuthHelper.getAuthorizationString();
      const response = await fetch(`${apiUrl}/admin/candles/${id}`, {
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

  create: async (candle: CandleRequest): Promise<Candle> => {
    try {
      const authorizationString = AuthHelper.getAuthorizationString();

      const response = await fetch(`${apiUrl}/admin/candles`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: authorizationString,
        },
        body: JSON.stringify(candle),
      });

      if (!response.ok) {
        const errorBody = await response.text();
        throw new Error(`Ошибка ${response.status}: ${errorBody}`);
      }

      return;
    } catch (error) {
      console.error('Ошибка при создании свечи:', error);
      throw new Error(error.message || 'Неизвестная ошибка');
    }
  },

  update: async (id: string, candle: CandleRequest): Promise<Candle> => {
    try {
      const authorizationString = AuthHelper.getAuthorizationString();
      const response = await fetch(`${apiUrl}/admin/candles/${id}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          Authorization: authorizationString,
        },
        body: JSON.stringify(candle),
      });

      if (!response.ok) {
        const errorBody = await response.text();
        throw new Error(`Ошибка ${response.status}: ${errorBody}`);
      }

      return;
    } catch (error) {
      throw new Error(error as string);
    }
  },

  delete: async (id: string): Promise<void> => {
    try {
      const authorizationString = AuthHelper.getAuthorizationString();

      const response = await fetch(`${apiUrl}/admin/candles/${id}`, {
        method: 'DELETE',
        headers: {
          'Content-Type': 'application/json',
          Authorization: authorizationString,
        },
      });

      if (!response.ok) {
        throw new Error(`Ошибка удаления: ${response.statusText}`);
      }

      return;
    } catch (error) {
      throw new Error(error instanceof Error ? error.message : String(error));
    }
  },

  updateDecor: async (
    id: string,
    decorIds: number[],
  ): Promise<ApiResponse<void>> => {
    try {
      const authorizationString = AuthHelper.getAuthorizationString();
      const response = await fetch(`${apiUrl}/admin/candles/${id}/decors`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          Authorization: authorizationString,
        },
        body: JSON.stringify(decorIds),
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

  updateLayerColor: async (
    id: string,
    layerColorIds: number[],
  ): Promise<ApiResponse<void>> => {
    try {
      const authorizationString = AuthHelper.getAuthorizationString();
      const response = await fetch(
        `${apiUrl}/admin/candles/${id}/layerColors`,
        {
          method: 'PUT',
          headers: {
            'Content-Type': 'application/json',
            Authorization: authorizationString,
          },
          body: JSON.stringify(layerColorIds),
        },
      );

      if (response.ok) {
        return { data: null, error: null };
      } else {
        return { data: null, error: await response.text() };
      }
    } catch (error) {
      throw new Error(error as string);
    }
  },

  updateNumberOfLayer: async (
    id: string,
    numberOfLayerIds: number[],
  ): Promise<ApiResponse<void>> => {
    try {
      const authorizationString = AuthHelper.getAuthorizationString();
      const response = await fetch(
        `${apiUrl}/admin/candles/${id}/numberOfLayers`,
        {
          method: 'PUT',
          headers: {
            'Content-Type': 'application/json',
            Authorization: authorizationString,
          },
          body: JSON.stringify(numberOfLayerIds),
        },
      );

      if (response.ok) {
        return { data: null, error: null };
      } else {
        return { data: null, error: await response.text() };
      }
    } catch (error) {
      throw new Error(error as string);
    }
  },

  updateSmell: async (
    id: string,
    smellIds: number[],
  ): Promise<ApiResponse<void>> => {
    try {
      const authorizationString = AuthHelper.getAuthorizationString();
      const response = await fetch(`${apiUrl}/admin/candles/${id}/smells`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          Authorization: authorizationString,
        },
        body: JSON.stringify(smellIds),
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

  updateWick: async (
    id: string,
    wickIds: number[],
  ): Promise<ApiResponse<void>> => {
    try {
      const authorizationString = AuthHelper.getAuthorizationString();
      const response = await fetch(`${apiUrl}/admin/candles/${id}/wicks`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          Authorization: authorizationString,
        },
        body: JSON.stringify(wickIds),
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
