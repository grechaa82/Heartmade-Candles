import {
  Candle,
  Decor,
  LayerColor,
  NumberOfLayer,
  Smell,
  Wick,
} from '../shared/BaseProduct';

export interface CustomCandle {
  candle: Candle;
  numberOfLayer?: NumberOfLayer;
  layerColors?: LayerColor[];
  wick?: Wick;
  decor?: Decor;
  smell?: Smell;
  quantity: number;
  filter?: string;
}
