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
  layerColors: LayerColor[];
  numberOfLayers: NumberOfLayer[];
  smells?: Smell[];
  wicks: Wick[];
}

export interface LayerColorRequest extends ImageProduct {
  pricePerGram: number;
}

// ConfiguredCandleDetail = CandleDetailWithQuantity
export class ConfiguredCandleDetail {
  candle: Candle;
  quantity: number;
  numberOfLayer?: NumberOfLayer;
  layerColors?: LayerColor[];
  wick?: Wick;
  decor?: Decor;
  smell?: Smell;
  filter?: string;

  constructor(
    candle: Candle,
    quantity: number,
    numberOfLayer?: NumberOfLayer,
    layerColors?: LayerColor[],
    wick?: Wick,
    decor?: Decor,
    smell?: Smell,
  ) {
    this.candle = candle;
    this.layerColors = layerColors;
    this.numberOfLayer = numberOfLayer;
    this.wick = wick;
    this.quantity = quantity;
    this.decor = decor;
    this.smell = smell;
  }

  getFilter(): string {
    if (!this.filter) {
      const parts = [
        `c-${this.candle.id}`,
        `n-${this.numberOfLayer?.id}`,
        this.layerColors ? `l-${this.layerColors.map((item) => item.id).join('_')}` : '',
        this.decor ? `d-${this.decor.id}` : '',
        this.smell ? `s-${this.smell.id}` : '',
        `w-${this.wick?.id}`,
        `q-${this.quantity}`,
      ];

      return parts.filter((part) => part !== '').join('~');
    }

    return this.filter;
  }
}
