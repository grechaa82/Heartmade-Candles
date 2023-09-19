import { Candle } from '../shared/BaseProduct';

export interface CandleTypeWithCandles {
  type: string;
  candles: Candle[];
}
