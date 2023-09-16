import { LayerColorRequest } from '../BaseProduct';
import { Candle, Decor, NumberOfLayer, Smell, Wick } from '../Candle';

export interface CandleDetailResponse {
  candle: Candle;
  decors?: Decor[];
  layerColors: LayerColorRequest[];
  numberOfLayers: NumberOfLayer[];
  smells?: Smell[];
  wicks: Wick[];
}
