import { CandleDetail } from '../typesV2/constructor/CandleDetail';
import { CandlesByType } from '../typesV2/constructor/CandlesByType';
import { Candle } from '../typesV2/shared/BaseProduct';

import { apiUrl } from '../config';

export const ConstructorApi = {
  getCandles: async (): Promise<CandlesByType[]> => {
    const response = await fetch(`${apiUrl}/constructor/candles`, {
      method: 'GET',
      mode: 'cors',
      headers: { 'Content-Type': 'application/json' },
    });

    if (!response.ok) {
      const errorBody = await response.text();
      throw new Error(`Error ${response.status}: ${errorBody}`);
    }

    return response.json();
  },
  getCandlesByType: async (
    typeCandle: string,
    pageSize: number = 15,
    pageIndex: number = 0,
  ): Promise<Candle[]> => {
    const response = await fetch(
      `${apiUrl}/constructor/candles/type?typeCandle=${encodeURIComponent(
        typeCandle,
      )}&pageSize=${pageSize}&pageIndex=${pageIndex}`,
      {
        method: 'GET',
        headers: { 'Content-Type': 'application/json' },
      },
    );

    if (!response.ok) {
      const errorBody = await response.text();
      throw new Error(`Error ${response.status}: ${errorBody}`);
    }

    return response.json();
  },
  getCandleById: async (candleId: string): Promise<CandleDetail> => {
    const response = await fetch(`${apiUrl}/constructor/candles/${candleId}`, {
      method: 'GET',
      headers: { 'Content-Type': 'application/json' },
    });

    if (!response.ok) {
      const errorBody = await response.text();
      throw new Error(`Error ${response.status}: ${errorBody}`);
    }

    return response.json();
  },
};
