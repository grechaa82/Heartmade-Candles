import { Image } from '../shared/BaseProduct';

export interface ConfiguredCandle {
  candle: Candle;
  decor: Decor;
  layerColors?: LayerColor[];
  numberOfLayer: NumberOfLayer;
  smell?: Smell;
  wick: Wick;
}

interface Candle {
  id: number;
  title: string;
  price: number;
  weightGrams: number;
  images: Image[];
}

interface Decor {
  id: number;
  title: string;
  price: number;
}

interface LayerColor {
  id: number;
  title: string;
  pricePerGram: number;
}

interface Smell {
  id: number;
  title: string;
  price: number;
}

interface Wick {
  id: number;
  title: string;
  price: number;
}

interface NumberOfLayer {
  id: number;
  number: number;
}
