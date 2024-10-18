import { Candle } from '../shared/BaseProduct';

export interface CandlesByType {
  type: string;
  candles: Candle[];
  totalCount: number;
}
