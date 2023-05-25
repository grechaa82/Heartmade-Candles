import { BaseProduct } from './BaseProduct';

export interface Candle extends BaseProduct {
  price: number;
  weightGrams: number;
  typeCandle: number;
  createdAt: string;
}
