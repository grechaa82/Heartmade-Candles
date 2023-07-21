import { Image } from '../Image';
import { TypeCandle } from '../TypeCandle';

export interface CandleRequest {
  title: string;
  description: string;
  price: number;
  weightGrams: number;
  images: Image[];
  typeCandle: TypeCandle;
  isActive: boolean;
}
