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
  };
  public errors: string[] = [];
  public isValid: boolean = false;

  public getCustomCandle(): CustomCandle {
    return this.customCandle;
  }

  public getFilter(): string {
    if (!this.isValid) {
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
      this.errors.push('Необходимо указать количество слоев');
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
      this.errors.push('Сначала необходимо выбрать количество слоев');
      return this;
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
      this.errors.push('Необходимо сначала выбрать количество слоев');
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

  private validate(): void {
    this.errors = [];

    if (
      !this.customCandle.numberOfLayer ||
      this.customCandle.numberOfLayer === null
    ) {
      this.errors.push('Необходимо указать количество слоев');
    } else if (
      !this.customCandle.layerColors ||
      this.customCandle.layerColors.length < 0
    ) {
      this.errors.push('Необходимо указать цвет слоев');
    } else if (
      this.customCandle.numberOfLayer &&
      this.customCandle.numberOfLayer.number !==
        this.customCandle.layerColors.length
    ) {
      this.errors.push(
        'Количество слоев не соответствует количеству указанных цветов',
      );
    }

    if (!this.customCandle.wick) {
      this.errors.push('Необходимо указать фитиль');
    }

    this.isValid = this.errors.length === 0;
  }

  public checkCustomCandleAgainstCandleDetail(
    candleDetail: CandleDetail,
  ): boolean {
    this.errors = [];
    if (!candleDetail.candle) {
      this.errors.push('Необходимо указать свечу');
      return false;
    }

    if (
      this.customCandle.numberOfLayer &&
      !candleDetail.numberOfLayers.some(
        (layer) => layer.id === this.customCandle.numberOfLayer.id,
      )
    ) {
      this.errors.push('Выбранное количество слоев недоступно в списке');
    }

    if (this.customCandle.layerColors) {
      this.customCandle.layerColors.forEach((color) => {
        if (
          !candleDetail.layerColors.some(
            (layerColor) => layerColor.id === color.id,
          )
        ) {
          this.errors.push(`Цвет слоя ${color.title} недоступен`);
        }
      });
    }

    if (
      this.customCandle.decor &&
      !candleDetail.decors?.some(
        (decor) => decor.id === this.customCandle.decor.id,
      )
    ) {
      this.errors.push('Выбранный декор недоступен');
    }

    if (
      this.customCandle.smell &&
      !candleDetail.smells?.some(
        (smell) => smell.id === this.customCandle.smell.id,
      )
    ) {
      this.errors.push('Выбранный запах недоступен');
    }

    if (
      this.customCandle.wick &&
      !candleDetail.wicks?.some(
        (wick) => wick.id === this.customCandle.wick?.id,
      )
    ) {
      this.errors.push('Выбранный фитиль не доступен');
    }

    this.isValid = this.errors.length === 0;
    return this.isValid;
  }

  public build(): BuildResult {
    if (!this.isValid || this.errors.length > 0) {
      return {
        success: false,
        errors: this.errors,
      };
    }

    const customCandle: CustomCandle = {
      candle: this.customCandle.candle,
      numberOfLayer: this.customCandle.numberOfLayer,
      layerColors: this.customCandle.layerColors,
      wick: this.customCandle.wick,
      decor: this.customCandle.decor,
      smell: this.customCandle.smell,
      quantity: this.customCandle.quantity,
      filter: this.customCandle.filter,
    };

    return {
      success: true,
      customCandle,
    };
  }

  public getErrors(): string[] {
    return this.errors;
  }
}
