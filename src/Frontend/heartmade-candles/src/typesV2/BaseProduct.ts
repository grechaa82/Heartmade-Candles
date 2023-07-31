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

export interface NumberOfLayer {
  id: number;
  number: number;
}

export interface TypeCandle {
  id: number;
  title: string;
}

export interface Image {
  fileName: string;
  alternativeName: string;
}

export interface CandleDetail {
  candle: Candle;
  decors?: Decor[];
  layerColors?: LayerColor[];
  numberOfLayers?: NumberOfLayer[];
  smells?: Smell[];
  wicks?: Wick[];
}
