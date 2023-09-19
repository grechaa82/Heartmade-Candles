import {
  Candle,
  Decor,
  NumberOfLayer,
  Smell,
  Wick,
  LayerColorRequest,
} from '../shared/BaseProduct';

export interface CandleDetailResponse {
  candle: Candle;
  decors?: Decor[];
  layerColors: LayerColorRequest[];
  numberOfLayers: NumberOfLayer[];
  smells?: Smell[];
  wicks: Wick[];
}
