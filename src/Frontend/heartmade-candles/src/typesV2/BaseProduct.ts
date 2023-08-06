export interface BaseProduct {
  id: number;
  title: string;
  description: string;
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

export interface Smell extends PriceProduct {}

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

export interface CandleDetailWithQuantity {
  candle: Candle;
  decors?: Decor[];
  layerColors?: LayerColor[];
  numberOfLayers?: NumberOfLayer[];
  smells?: Smell[];
  wicks?: Wick[];
  quantity: number;
}

export interface CandleDetailRequest {
  candle: Candle;
  decors?: Decor[];
  layerColors?: LayerColorRequest[];
  numberOfLayers?: NumberOfLayer[];
  smells?: Smell[];
  wicks?: Wick[];
}

export interface LayerColorRequest extends ImageProduct {
  pricePerGram: number;
}
