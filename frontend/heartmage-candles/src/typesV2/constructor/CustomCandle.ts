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
  isValid: boolean;
  errors: string[];
}

export function getFilterFromCustomCandle(customCandle: CustomCandle) {
  const parts = [
    `c-${customCandle.candle.id}`,
    `n-${customCandle.numberOfLayer?.id}`,
    customCandle.layerColors
      ? `l-${customCandle.layerColors.map((item) => item.id).join('_')}`
      : '',
    customCandle.decor ? `d-${customCandle.decor.id}` : '',
    customCandle.smell ? `s-${customCandle.smell.id}` : '',
    `w-${customCandle.wick?.id}`,
    `q-${customCandle.quantity}`,
  ];

  const filter = parts.filter((part) => part !== '').join('~');

  return filter;
}

export function getFilter(
  candle: Candle,
  numberOfLayer?: NumberOfLayer,
  layerColors?: LayerColor[],
  wick?: Wick,
  decor?: Decor,
  smell?: Smell,
  quantity: number = 1,
) {
  const parts = [
    `c-${candle.id}`,
    `n-${numberOfLayer?.id}`,
    layerColors ? `l-${layerColors.map((item) => item.id).join('_')}` : '',
    decor ? `d-${decor.id}` : '',
    smell ? `s-${smell.id}` : '',
    `w-${wick?.id}`,
    `q-${quantity}`,
  ];

  const filter = parts.filter((part) => part !== '').join('~');

  return filter;
}
