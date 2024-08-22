import { Candle } from '../shared/BaseProduct';

export interface CandlesByType {
  type: string;
  candles: Candle[];
  pageSize?: number;
  pageIndex?: number;
}
