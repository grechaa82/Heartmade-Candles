import {
  Candle,
  NumberOfLayer,
  LayerColor,
  Wick,
  Decor,
  Smell,
} from '../shared/BaseProduct';
import { CandleDetail } from './CandleDetail';
import { OrderItemFilter } from '../shared/OrderItemFilter';

export class ConfiguredCandleDetail {
  candle: Candle;
  quantity: number;
  numberOfLayer?: NumberOfLayer;
  layerColors?: LayerColor[];
  wick?: Wick;
  decor?: Decor;
  smell?: Smell;
  filter?: string;
  isValid: boolean;
  errors: string[];

  constructor(
    candle: Candle,
    quantity: number,
    numberOfLayer?: NumberOfLayer,
    layerColors?: LayerColor[],
    wick?: Wick,
    decor?: Decor,
    smell?: Smell,
    isValid: boolean = false,
    errors: string[] = [],
  ) {
    this.candle = candle;
    this.layerColors = layerColors;
    this.numberOfLayer = numberOfLayer;
    this.wick = wick;
    this.quantity = quantity;
    this.decor = decor;
    this.smell = smell;
    this.isValid = isValid;
    this.errors = errors;
  }

  getFilter(): string {
    if (!this.filter) {
      const parts = [
        `c-${this.candle.id}`,
        `n-${this.numberOfLayer?.id}`,
        this.layerColors
          ? `l-${this.layerColors.map((item) => item.id).join('_')}`
          : '',
        this.decor ? `d-${this.decor.id}` : '',
        this.smell ? `s-${this.smell.id}` : '',
        `w-${this.wick?.id}`,
        `q-${this.quantity}`,
      ];

      return parts.filter((part) => part !== '').join('~');
    }

    return this.filter;
  }
}

export function validateConfiguredCandleDetail(
  candleDetail: CandleDetail,
  orderItemFilter: OrderItemFilter,
): ConfiguredCandleDetail {
  const errorMessageInvalidCandleComponents: string[] = [];
  let isValid = true;

  if (
    candleDetail.decors &&
    orderItemFilter.decorId &&
    !candleDetail.decors.some((d) => d.id === orderItemFilter.decorId)
  ) {
    errorMessageInvalidCandleComponents.push(
      generateErrorMessage(
        candleDetail.candle.title,
        'декора',
        orderItemFilter.decorId.toString(),
      ),
    );
    isValid = false;
  }

  if (
    candleDetail.layerColors &&
    orderItemFilter.layerColorIds &&
    !orderItemFilter.layerColorIds.every((layerColorId) =>
      candleDetail.layerColors?.some(
        (layerColor) => layerColor.id === layerColorId,
      ),
    )
  ) {
    errorMessageInvalidCandleComponents.push(
      generateErrorMessage(
        candleDetail.candle.title,
        'слоя',
        orderItemFilter.layerColorIds.join(','),
      ),
    );
    isValid = false;
  }

  if (
    candleDetail.numberOfLayers &&
    orderItemFilter.numberOfLayerId &&
    !candleDetail.numberOfLayers.some(
      (n) => n.id === orderItemFilter.numberOfLayerId,
    )
  ) {
    errorMessageInvalidCandleComponents.push(
      generateErrorMessage(
        candleDetail.candle.title,
        'количества слоев',
        orderItemFilter.numberOfLayerId.toString(),
      ),
    );
    isValid = false;
  }

  if (
    candleDetail.smells &&
    orderItemFilter.smellId &&
    !candleDetail.smells.some((s) => s.id === orderItemFilter.smellId)
  ) {
    errorMessageInvalidCandleComponents.push(
      generateErrorMessage(
        candleDetail.candle.title,
        'запаха',
        orderItemFilter.smellId.toString(),
      ),
    );
    isValid = false;
  }

  if (
    candleDetail.wicks &&
    orderItemFilter.wickId &&
    !candleDetail.wicks.some((w) => w.id === orderItemFilter.wickId)
  ) {
    errorMessageInvalidCandleComponents.push(
      generateErrorMessage(
        candleDetail.candle.title,
        'фитиля',
        orderItemFilter.wickId.toString(),
      ),
    );
    isValid = false;
  }

  return createConfiguredCandleDetail(
    candleDetail,
    orderItemFilter,
    isValid,
    errorMessageInvalidCandleComponents,
  );
}

function generateErrorMessage(
  candleTitle: string,
  property: string,
  id: string,
) {
  return `У свечи '${candleTitle}' нет ${property}: ${id}`;
}

function createConfiguredCandleDetail(
  candleDetail: CandleDetail,
  orderItemFilter: OrderItemFilter,
  isValid: boolean,
  errors: string[],
): ConfiguredCandleDetail | undefined {
  const decor = candleDetail.decors?.find(
    (d) => d.id === orderItemFilter.decorId,
  );

  const layerColors = candleDetail.layerColors.filter((layerColor) =>
    orderItemFilter.layerColorIds.includes(layerColor.id),
  );

  const numberOfLayer = candleDetail.numberOfLayers?.find(
    (numberOfLayer) => numberOfLayer.id === orderItemFilter.numberOfLayerId,
  );

  const smell = candleDetail.smells?.find(
    (smell) => smell.id === orderItemFilter.smellId,
  );

  const wick = candleDetail.wicks?.find(
    (wick) => wick.id === orderItemFilter.wickId,
  );

  if (numberOfLayer && wick) {
    const configuredCandleDetail = new ConfiguredCandleDetail(
      candleDetail.candle,
      orderItemFilter.quantity,
      numberOfLayer,
      layerColors,
      wick,
      decor,
      smell,
      isValid,
      errors,
    );

    return configuredCandleDetail;
  }

  return undefined;
}
