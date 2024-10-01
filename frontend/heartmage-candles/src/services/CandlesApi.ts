import { Candle } from '../types/Candle';
import { CandleDetail } from '../types/CandleDetail';
import { CandleRequest } from '../types/Requests/CandleRequest';
import { AuthHelper } from '../helpers/AuthHelper';
import { PaginationSettings } from '../typesV2/shared/PaginationSettings';

import { apiUrl } from '../config';

export const CandlesApi = {
  getAll: async (
    typeFilter?: string,
    pagination: PaginationSettings = { pageSize: 20, pageIndex: 0 },
  ): Promise<Candle[]> => {
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

  getById: async (id: string): Promise<CandleDetail> => {
    const response = await fetch(`${apiUrl}/admin/candles/${id}`, {
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

  create: async (candle: CandleRequest): Promise<Candle> => {
    const response = await fetch(`${apiUrl}/admin/candles`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: AuthHelper.getAuthorizationString(),
      },
      body: JSON.stringify(candle),
    });

    if (!response.ok) {
      const errorBody = await response.text();
      throw new Error(`Error ${response.status}: ${errorBody}`);
    }

    return;
  },

  update: async (id: string, candle: CandleRequest): Promise<Candle> => {
    const response = await fetch(`${apiUrl}/admin/candles/${id}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        Authorization: AuthHelper.getAuthorizationString(),
      },
      body: JSON.stringify(candle),
    });

    if (!response.ok) {
      const errorBody = await response.text();
      throw new Error(`Error ${response.status}: ${errorBody}`);
    }

    return;
  },

  delete: async (id: string): Promise<void> => {
    const response = await fetch(`${apiUrl}/admin/candles/${id}`, {
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

  updateDecor: async (id: string, decorIds: number[]): Promise<void> => {
    const response = await fetch(`${apiUrl}/admin/candles/${id}/decors`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        Authorization: AuthHelper.getAuthorizationString(),
      },
      body: JSON.stringify(decorIds),
    });

    if (!response.ok) {
      const errorBody = await response.text();
      throw new Error(`Error ${response.status}: ${errorBody}`);
    }

    return;
  },

  updateLayerColor: async (
    id: string,
    layerColorIds: number[],
  ): Promise<void> => {
    const response = await fetch(`${apiUrl}/admin/candles/${id}/layerColors`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        Authorization: AuthHelper.getAuthorizationString(),
      },
      body: JSON.stringify(layerColorIds),
    });

    if (!response.ok) {
      const errorBody = await response.text();
      throw new Error(`Error ${response.status}: ${errorBody}`);
    }

    return;
  },

  updateNumberOfLayer: async (
    id: string,
    numberOfLayerIds: number[],
  ): Promise<void> => {
    const response = await fetch(
      `${apiUrl}/admin/candles/${id}/numberOfLayers`,
      {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          Authorization: AuthHelper.getAuthorizationString(),
        },
        body: JSON.stringify(numberOfLayerIds),
      },
    );

    if (!response.ok) {
      const errorBody = await response.text();
      throw new Error(`Error ${response.status}: ${errorBody}`);
    }

    return;
  },

  updateSmell: async (id: string, smellIds: number[]): Promise<void> => {
    const response = await fetch(`${apiUrl}/admin/candles/${id}/smells`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        Authorization: AuthHelper.getAuthorizationString(),
      },
      body: JSON.stringify(smellIds),
    });

    if (!response.ok) {
      const errorBody = await response.text();
      throw new Error(`Error ${response.status}: ${errorBody}`);
    }

    return;
  },

  updateWick: async (id: string, wickIds: number[]): Promise<void> => {
    const response = await fetch(`${apiUrl}/admin/candles/${id}/wicks`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        Authorization: AuthHelper.getAuthorizationString(),
      },
      body: JSON.stringify(wickIds),
    });

    if (!response.ok) {
      const errorBody = await response.text();
      throw new Error(`Error ${response.status}: ${errorBody}`);
    }

    return;
  },
};
