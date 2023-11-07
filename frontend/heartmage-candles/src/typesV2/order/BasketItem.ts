import { ConfiguredCandle } from './ConfiguredCandle';
import { ConfiguredCandleFilter } from './ConfiguredCandleFilter';

export interface BasketItem {
  configuredCandle: ConfiguredCandle;
  price: number;
  quantity: number;
  configuredCandleFilter: ConfiguredCandleFilter;
}
