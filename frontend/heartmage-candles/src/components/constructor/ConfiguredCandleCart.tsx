import { FC } from 'react';

import IconPlusLarge from '../../UI/IconPlusLarge';
import IconMinusLarge from '../../UI/IconMinusLarge';
import Picture from '../shared/Picture';
import { CustomCandle } from '../../typesV2/constructor/CustomCandle';

import Style from './ConfiguredCandleCart.module.css';

export interface ConfiguredCandleCartProps {
  product: CustomCandle;
  onChangingQuantityProduct: (newQuantity: number, key: number) => void;
  quantity: number;
  index: number;
  onSelect?: (customCandle: CustomCandle) => void;
}

const ConfiguredCandleCart: FC<ConfiguredCandleCartProps> = ({
  product,
  onChangingQuantityProduct,
  quantity,
  index,
  onSelect,
}) => {
  const firstImage =
    product.candle.images && product.candle.images.length > 0
      ? product.candle.images[0]
      : null;

  const onIncreasesQuantityProduct = () => {
    onChangingQuantityProduct(quantity + 1, index);
  };
  const onDecreasesQuantityProduct = () => {
    onChangingQuantityProduct(quantity - 1, index);
  };

  return (
    <div className={Style.productCart}>
      <div
        className={`${Style.imageBlock} ${
          product.isValid ? Style.valid : Style.invalid
        }`}
        onClick={() => onSelect(product)}
      >
        {firstImage && (
          <Picture
            name={firstImage.fileName}
            alt={firstImage.alternativeName}
            sourceSettings={[
              {
                size: 'small',
              },
            ]}
          />
        )}
      </div>
      <div className={Style.quantityManagement}>
        <button
          className={Style.iconBtn}
          onClick={() => onIncreasesQuantityProduct()}
        >
          <IconPlusLarge color="#aaa" />
        </button>
        <p className={Style.quantity}>{quantity}</p>
        <button
          className={Style.iconBtn}
          onClick={() => onDecreasesQuantityProduct()}
        >
          <IconMinusLarge color="#aaa" />
        </button>
      </div>
    </div>
  );
};

export default ConfiguredCandleCart;
