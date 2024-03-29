import { ConfiguredCandleDetail } from '../typesV2/constructor/ConfiguredCandleDetail';

export const calculatePrice = (candleDetail: ConfiguredCandleDetail): number => {
  let totalPrice = candleDetail.candle.price;

  if (candleDetail.decor) {
    totalPrice += candleDetail.decor.price;
  }

  if (
    candleDetail.numberOfLayer &&
    candleDetail.layerColors &&
    candleDetail.layerColors.length > 0
  ) {
    const gramsInLayer = candleDetail.candle.weightGrams / candleDetail.numberOfLayer.number;

    for (const layerColor of candleDetail.layerColors) {
      totalPrice += gramsInLayer * layerColor.price;
    }
  }

  if (candleDetail.smell) {
    totalPrice += candleDetail.smell.price;
  }

  if (candleDetail.wick) {
    totalPrice += candleDetail.wick.price;
  }

  return totalPrice;
};
