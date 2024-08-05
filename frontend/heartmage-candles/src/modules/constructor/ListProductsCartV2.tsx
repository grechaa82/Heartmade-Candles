import { FC } from 'react';

import { useConstructorContext } from '../../contexts/ConstructorContext';
import { CustomCandle } from '../../typesV2/constructor/CustomCandle';
import ConfiguredCandleCartV2 from '../../components/constructor/ConfiguredCandleCartV2';

import Style from './ListProductsCart.module.css';
import Button from '../../components/shared/Button';

type ButtonState = 'default' | 'invalid' | 'valid';

export interface ListProductsCartPropsV2 {
  buttonState?: 'default' | 'invalid' | 'valid';
  onSelect: (customCandle: CustomCandle) => void;
  onCreateBasket: () => void;
}

const ListProductsCartV2: FC<ListProductsCartPropsV2> = ({
  buttonState = 'default',
  onSelect,
  onCreateBasket,
}) => {
  const { customCandles, totalPrice, setCustomCandles } =
    useConstructorContext();

  const handleChangingQuantityProduct = (
    newQuantity: number,
    index: number,
  ) => {
    const newCustomCandles = [...customCandles];

    if (newCustomCandles[index]) {
      if (newQuantity <= 0) {
        newCustomCandles.splice(index, 1);
      } else {
        newCustomCandles[index].quantity = newQuantity;
      }
    }

    setCustomCandles(newCustomCandles);
  };

  const handleOnSelect = (customCandle: CustomCandle) => {
    onSelect(customCandle);
  };

  return (
    <>
      <div className={Style.listProductsCart}>
        {customCandles.map((product, index) => (
          <ConfiguredCandleCartV2
            key={index}
            index={index}
            product={product}
            onChangingQuantityProduct={handleChangingQuantityProduct}
            quantity={product.quantity}
            onSelect={handleOnSelect}
          />
        ))}
        <div className={Style.infoBlock}>
          <div className={Style.priceBlock}>
            <span className={Style.priceTitle}>Итого</span>
            <span className={Style.price}>{totalPrice} р</span>
          </div>
          <Button
            text="Заказать"
            onClick={onCreateBasket}
            className={Style[buttonState]}
          />
        </div>
      </div>
    </>
  );
};

export default ListProductsCartV2;
