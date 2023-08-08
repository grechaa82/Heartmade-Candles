import { CandleDetail } from '../typesV2/BaseProduct';

export const calculatePrice = (candleDetail: CandleDetail): number => {
  let totalPrice: number = candleDetail.candle.price;

  if (candleDetail.decors) {
    for (const decor of candleDetail.decors) {
      totalPrice += decor.price;
    }
  }

  if (candleDetail.numberOfLayers && candleDetail.layerColors) {
    const gramsInLayer = candleDetail.candle.weightGrams / candleDetail.numberOfLayers[0].number;

    for (const layerColor of candleDetail.layerColors) {
      totalPrice += gramsInLayer * layerColor.price;
    }
  }

  if (candleDetail.smells) {
    for (const smell of candleDetail.smells) {
      totalPrice += smell.price;
    }
  }

  if (candleDetail.wicks) {
    for (const wick of candleDetail.wicks) {
      totalPrice += wick.price;
    }
  }

  return Number(totalPrice.toFixed());
};
