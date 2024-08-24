import {
  Candle,
  Decor,
  LayerColor,
  NumberOfLayer,
  Smell,
  Wick,
} from '../shared/BaseProduct';
import { CandleDetail } from './CandleDetail';
import { CustomCandle } from './CustomCandle';

export interface BuildResult {
  success: boolean;
  errors?: string[];
  customCandle?: CustomCandle;
}

export class CustomCandleBuilder {
  public customCandle: CustomCandle = {
    candle: null,
    numberOfLayer: null,
    layerColors: [],
    wick: null,
    decor: null,
    smell: null,
    quantity: 1,
    filter: '',
    isValid: false,
    errors: [],
  };

  public getCustomCandle(): CustomCandle {
    return this.customCandle;
  }

  public getFilter(): string {
    if (!this.customCandle.isValid) {
      return;
    }
    if (!this.customCandle.filter) {
      const parts = [
        `c-${this.customCandle.candle.id}`,
        `n-${this.customCandle.numberOfLayer?.id}`,
        this.customCandle.layerColors
          ? `l-${this.customCandle.layerColors
              .map((item) => item.id)
              .join('_')}`
          : '',
        this.customCandle.decor ? `d-${this.customCandle.decor.id}` : '',
        this.customCandle.smell ? `s-${this.customCandle.smell.id}` : '',
        `w-${this.customCandle.wick?.id}`,
        `q-${this.customCandle.quantity}`,
      ];

      const filter = parts.filter((part) => part !== '').join('~');

      this.customCandle.filter = filter;

      return filter;
    }

    return this.customCandle.filter;
  }

  public setCandle(candle: Candle): CustomCandleBuilder {
    this.customCandle = {
      ...this.customCandle,
      candle: candle,
    };

    this.validate();
    return this;
  }

  public setNumberOfLayer(numberOfLayer: NumberOfLayer): CustomCandleBuilder {
    if (!numberOfLayer) {
      this.customCandle.errors.push('Необходимо указать количество слоев');
      return this;
    }

    this.customCandle = {
      ...this.customCandle,
      numberOfLayer: numberOfLayer,
      layerColors: [],
    };

    this.validate();
    return this;
  }

  public setLayerColor(layerColor: LayerColor[]): CustomCandleBuilder {
    if (
      !this.customCandle.numberOfLayer ||
      this.customCandle.numberOfLayer === null
    ) {
      this.customCandle.errors.push(
        'Сначала необходимо выбрать количество слоев',
      );
    }

    this.customCandle = {
      ...this.customCandle,
      layerColors: layerColor,
    };

    this.validate();
    return this;
  }

  public addLayerColor(layerColor: LayerColor): CustomCandleBuilder {
    if (!this.customCandle.numberOfLayer) {
      this.customCandle.errors.push(
        'Необходимо сначала выбрать количество слоев',
      );
      return this;
    }

    const newLayerColors = [...this.customCandle.layerColors];

    if (this.customCandle.numberOfLayer.number === newLayerColors.length) {
      newLayerColors[newLayerColors.length - 1] = layerColor;
    } else {
      newLayerColors.push(layerColor);
    }

    this.customCandle = {
      ...this.customCandle,
      layerColors: newLayerColors,
    };

    this.validate();
    return this;
  }

  public setWick(wick: Wick): CustomCandleBuilder {
    this.customCandle = {
      ...this.customCandle,
      wick: wick,
    };

    this.validate();
    return this;
  }

  public setDecor(decor: Decor): CustomCandleBuilder {
    this.customCandle = {
      ...this.customCandle,
      decor: decor,
    };

    this.validate();
    return this;
  }

  public setSmell(smell: Smell): CustomCandleBuilder {
    this.customCandle = {
      ...this.customCandle,
      smell: smell,
    };

    this.validate();
    return this;
  }

  public setQuantity(quantity: number): CustomCandleBuilder {
    this.customCandle = {
      ...this.customCandle,
      quantity: quantity,
    };

    this.validate();
    return this;
  }

  public setErrors(errors: string[]): CustomCandleBuilder {
    if (errors && errors.length > 0) {
      this.customCandle.errors = errors;
      this.customCandle.isValid = false;
    } else {
      this.customCandle.errors = [];
      this.customCandle.isValid = true;
    }

    return this;
  }

  private validate(): void {
    this.customCandle.errors = [];

    if (
      !this.customCandle.numberOfLayer ||
      this.customCandle.numberOfLayer === null
    ) {
      this.customCandle.errors.push('Необходимо указать количество слоев');
    } else if (
      !this.customCandle.layerColors ||
      this.customCandle.layerColors.length < 0
    ) {
      this.customCandle.errors.push('Необходимо указать цвет слоев');
    } else if (
      this.customCandle.numberOfLayer &&
      this.customCandle.numberOfLayer.number !==
        this.customCandle.layerColors.length
    ) {
      this.customCandle.errors.push(
        'Количество слоев не соответствует количеству указанных цветов',
      );
    }

    if (!this.customCandle.wick) {
      this.customCandle.errors.push('Необходимо указать фитиль');
    }

    this.customCandle.isValid = this.customCandle.errors.length === 0;

    this.customCandle.filter = this.getFilter();
  }

  public static checkCustomCandleAgainstCandleDetail(
    customCandle: CustomCandle,
    candleDetail: CandleDetail,
  ): CustomCandle {
    const errors: string[] = [];
    const newCustomCandleBuilder = new CustomCandleBuilder();

    if (!candleDetail?.candle) {
      errors.push('Необходимо указать свечу');
      return {
        ...customCandle,
        isValid: false,
        errors: [...errors, 'Необходимо указать свечу'],
      };
    }

    if (
      customCandle.numberOfLayer &&
      !candleDetail.numberOfLayers.some(
        (layer) => layer.id === customCandle.numberOfLayer.id,
      )
    ) {
      errors.push('Выбранное количество слоев недоступно в списке');
    }

    if (customCandle.layerColors) {
      customCandle.layerColors.forEach((color) => {
        if (
          !candleDetail.layerColors.some(
            (layerColor) => layerColor.id === color.id,
          )
        ) {
          errors.push(`Цвет слоя ${color.title} недоступен`);
        }
      });
    }

    if (
      customCandle.decor &&
      !candleDetail.decors?.some((decor) => decor.id === customCandle.decor.id)
    ) {
      errors.push('Выбранный декор недоступен');
    } else if (candleDetail.decors) {
      newCustomCandleBuilder.setDecor(null);
    }

    if (
      customCandle.smell &&
      !candleDetail.smells?.some((smell) => smell.id === customCandle.smell.id)
    ) {
      errors.push('Выбранный запах недоступен');
    } else if (candleDetail.smells) {
      newCustomCandleBuilder.setSmell(null);
    }

    if (
      customCandle.wick &&
      !candleDetail.wicks?.some((wick) => wick.id === customCandle.wick?.id)
    ) {
      errors.push('Выбранный фитиль не доступен');
    }

    newCustomCandleBuilder
      .setCandle(customCandle.candle)
      .setNumberOfLayer(customCandle.numberOfLayer)
      .setLayerColor(customCandle.layerColors)
      .setWick(customCandle.wick)
      .setDecor(customCandle.decor)
      .setSmell(customCandle.smell)
      .setQuantity(customCandle.quantity);

    const customCandleBuildResult = newCustomCandleBuilder.build();

    return {
      ...customCandleBuildResult.customCandle,
      isValid: errors.length === 0,
      errors: errors,
    };
  }

  public build(): BuildResult {
    const customCandle: CustomCandle = {
      candle: this.customCandle.candle,
      numberOfLayer: this.customCandle.numberOfLayer,
      layerColors: this.customCandle.layerColors,
      wick: this.customCandle.wick,
      decor: this.customCandle.decor,
      smell: this.customCandle.smell,
      quantity: this.customCandle.quantity,
      filter: this.customCandle.filter,
      isValid: this.customCandle.isValid,
      errors: this.customCandle.errors,
    };

    return {
      success: this.customCandle.isValid,
      errors: this.customCandle.errors,
      customCandle: customCandle,
    };
  }

  public getErrors(): string[] {
    return this.getCustomCandle().errors;
  }
}
