import { BaseProduct } from './BaseProduct';
import { TypeCandle } from './TypeCandle';

export interface Candle extends BaseProduct {
  price: number;
  weightGrams: number;
  typeCandle: TypeCandle;
  createdAt: string;
}
