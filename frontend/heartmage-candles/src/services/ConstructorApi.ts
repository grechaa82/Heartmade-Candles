import { CandleDetail } from '../typesV2/constructor/CandleDetail';
import { CandleDetailResponse } from '../typesV2/constructor/CandleDetailResponse';
import { CandlesByType } from '../typesV2/constructor/CandlesByType';
import { ApiResponse } from './ApiResponse';

import { apiUrl } from '../config';

export const ConstructorApi = {
  getCandles: async (): Promise<ApiResponse<CandlesByType[]>> => {
    try {
      const response = await fetch(`${apiUrl}/constructor/candles`, {
        method: 'GET',
        mode: 'cors',
        headers: { 'Content-Type': 'application/json' },
      });
      if (response.ok) {
        return {
          data: (await response.json()) as CandlesByType[],
          error: null,
        };
      } else {
        return { data: null, error: await response.text() };
      }
    } catch (error) {
      throw new Error(error as string);
    }
  },
  getCandleById: async (
    candleId: string,
  ): Promise<ApiResponse<CandleDetail>> => {
    try {
      const response = await fetch(
        `${apiUrl}/constructor/candles/${candleId}`,
        {
          method: 'GET',
          headers: { 'Content-Type': 'application/json' },
        },
      );
      if (response.ok) {
        const responseData = (await response.json()) as CandleDetailResponse;

        const candleDetail: CandleDetail = {
          candle: responseData.candle,
          decors: responseData.decors,
          layerColors: responseData.layerColors?.map((layerColor) => ({
            ...layerColor,
            price: layerColor.pricePerGram,
          })),
          numberOfLayers: responseData.numberOfLayers,
          smells: responseData.smells,
          wicks: responseData.wicks,
        };

        return { data: candleDetail, error: null };
      } else {
        return { data: null, error: await response.text() };
      }
    } catch (error) {
      throw new Error(error as string);
    }
  },
};
