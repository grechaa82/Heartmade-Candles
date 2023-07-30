import { Candle } from './BaseProduct';

export interface CandleTypeWithCandles {
  type: string;
  candles: Candle[];
}
