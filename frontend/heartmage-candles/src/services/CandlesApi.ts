import { Candle } from '../types/Candle';
import { CandleDetail } from '../types/CandleDetail';
import { CandleRequest } from '../types/Requests/CandleRequest';

import { apiUrl } from '../config';

export const CandlesApi = {
  async getAll(): Promise<Candle[]> {
    const response = await fetch(`${apiUrl}/admin/candles`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
    });
    return (await response.json()) as Candle[];
  },
  async getById(id: string): Promise<CandleDetail> {
    const response = await fetch(`${apiUrl}/admin/candles/${id}`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
    });
    return (await response.json()) as CandleDetail;
  },
  async create(candle: CandleRequest): Promise<void> {
    await fetch(`${apiUrl}/admin/candles`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
      body: JSON.stringify(candle),
    });
  },
  async update(id: string, candle: CandleRequest): Promise<void> {
    await fetch(`${apiUrl}/admin/candles/${id}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
      body: JSON.stringify(candle),
    });
  },
  async delete(id: string): Promise<void> {
    await fetch(`${apiUrl}/admin/candles/${id}`, {
      method: 'DELETE',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
    });
  },
  async updateDecor(id: string, decorIds: number[]): Promise<void> {
    await fetch(`${apiUrl}/admin/candles/${id}/decors`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
      body: JSON.stringify(decorIds),
    });
  },
  async updateLayerColor(id: string, layerColorIds: number[]): Promise<void> {
    await fetch(`${apiUrl}/admin/candles/${id}/layerColors`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
      body: JSON.stringify(layerColorIds),
    });
  },
  async updateNumberOfLayer(id: string, numberOfLayerIds: number[]): Promise<void> {
    await fetch(`${apiUrl}/admin/candles/${id}/numberOfLayers`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
      body: JSON.stringify(numberOfLayerIds),
    });
  },
  async updateSmell(id: string, smellIds: number[]): Promise<void> {
    await fetch(`${apiUrl}/admin/candles/${id}/smells`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
      body: JSON.stringify(smellIds),
    });
  },
  async updateWick(id: string, wickIds: number[]): Promise<void> {
    await fetch(`${apiUrl}/admin/candles/${id}/wicks`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
      body: JSON.stringify(wickIds),
    });
  },
};
