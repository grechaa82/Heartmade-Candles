import { Candle, Decor, LayerColor, NumberOfLayer, Smell, Wick } from '../BaseProduct';

export interface CandleDetail {
  candle: Candle;
  decor: Decor;
  layerColors?: LayerColor[];
  numberOfLayer: NumberOfLayer;
  smell?: Smell;
  wick: Wick;
}
