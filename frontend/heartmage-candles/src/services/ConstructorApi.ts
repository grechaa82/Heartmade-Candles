import { CandleDetails, CandleDetailRequest } from '../typesV2/BaseProduct';
import { CandleTypeWithCandles } from '../typesV2/CandleTypeWithCandles';

import { apiUrl } from '../config';

export const ConstructorApi = {
  async getCandles(): Promise<CandleTypeWithCandles[]> {
    const response = await fetch(`${apiUrl}/constructor/candles`, {
      method: 'GET',
      mode: 'cors',
      headers: { 'Content-Type': 'application/json' },
    });
    return (await response.json()) as CandleTypeWithCandles[];
  },
  async getCandleById(candleId: string): Promise<CandleDetails> {
    const response = await fetch(`${apiUrl}/constructor/candles/${candleId}`, {
      method: 'GET',
      headers: { 'Content-Type': 'application/json' },
    });
    const responseData = (await response.json()) as CandleDetailRequest;

    const candleDetail: CandleDetails = {
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

    return candleDetail;
  },
};
