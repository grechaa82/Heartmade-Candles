import { Candle } from './Candle'
import { Decor } from './Decor'
import { LayerColor } from './LayerColor'
import { NumberOfLayer } from './NumberOfLayer'
import { Smell } from './Smell'
import { Wick } from './Wick'

export interface CandleDetail {
  candle: Candle;
  decors: Decor[];
  layerColors: LayerColor[];
  numberOfLayers: NumberOfLayer[];
  smells: Smell[];
  wicks: Wick[];
}