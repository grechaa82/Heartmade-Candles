import { Candle, Decor, LayerColor, NumberOfLayer, Smell, Wick } from '../shared/BaseProduct';

export interface CandleDetail {
  candle: Candle;
  decors?: Decor[];
  layerColors: LayerColor[];
  numberOfLayers: NumberOfLayer[];
  smells?: Smell[];
  wicks: Wick[];
}
