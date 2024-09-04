import { ConfiguredCandleDetail } from '../typesV2/constructor/ConfiguredCandleDetail';
import { CustomCandle } from '../typesV2/constructor/CustomCandle';

export const calculatePrice = (
  candleDetail: ConfiguredCandleDetail,
): number => {
  let totalPrice = candleDetail.candle.price;

  if (candleDetail.decor) {
    totalPrice += candleDetail.decor.price;
  }

  if (
    candleDetail.numberOfLayer &&
    candleDetail.layerColors &&
    candleDetail.layerColors.length > 0
  ) {
    const gramsInLayer =
      candleDetail.candle.weightGrams / candleDetail.numberOfLayer.number;

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

export const calculateCustomCandlePrice = (
  customCandle: CustomCandle,
): number => {
  let totalPrice = customCandle.candle.price;

  if (customCandle.decor) {
    totalPrice += customCandle.decor.price;
  }

  if (
    customCandle.numberOfLayer &&
    customCandle.layerColors &&
    customCandle.layerColors.length > 0
  ) {
    const gramsInLayer =
      customCandle.candle.weightGrams / customCandle.numberOfLayer.number;

    for (const layerColor of customCandle.layerColors) {
      totalPrice += gramsInLayer * layerColor.price;
    }
  }

  if (customCandle.smell) {
    totalPrice += customCandle.smell.price;
  }

  if (customCandle.wick) {
    totalPrice += customCandle.wick.price;
  }

  return totalPrice;
};
