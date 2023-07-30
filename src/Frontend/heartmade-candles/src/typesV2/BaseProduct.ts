import { Image } from '../types/Image';
import { TypeCandle } from '../types/TypeCandle';

export interface BaseProduct {
  id: number;
  title: string;
  description: string;
  isActive: boolean;
}

export interface PriceProduct extends BaseProduct {
  price: number;
}

export interface ImageProduct extends PriceProduct {
  images: Image[];
}

export interface Candle extends ImageProduct {
  weightGrams: number;
  typeCandle: TypeCandle;
  createdAt: string;
}

export interface Decor extends ImageProduct {}

export interface LayerColor extends ImageProduct {}

export interface Smell extends ImageProduct {}

export interface Wick extends ImageProduct {}
