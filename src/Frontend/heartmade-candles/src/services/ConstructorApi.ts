import { CandleDetail } from '../typesV2/BaseProduct';
import { CandleTypeWithCandles } from '../typesV2/CandleTypeWithCandles';

const apiUrl = 'http://localhost:5000/api/constructor/candles';

export const ConstructorApi = {
  async getCandles(): Promise<CandleTypeWithCandles[]> {
    const response = await fetch(`${apiUrl}`, {
      method: 'GET',
      headers: { 'Content-Type': 'application/json' },
    });
    return (await response.json()) as CandleTypeWithCandles[];
  },
  async getCandleById(candleId: string): Promise<CandleDetail> {
    const response = await fetch(`${apiUrl}/${candleId}`, {
      method: 'GET',
      headers: { 'Content-Type': 'application/json' },
    });
    return (await response.json()) as CandleDetail;
  },
};
