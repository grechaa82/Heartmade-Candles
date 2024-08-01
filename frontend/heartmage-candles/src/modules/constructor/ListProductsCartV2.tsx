import { FC } from 'react';

import ConfiguredCandleCart from '../../components/constructor/ConfiguredCandleCart';
import { ConfiguredCandleDetail } from '../../typesV2/constructor/ConfiguredCandleDetail';
import Button from '../../components/shared/Button';

import Style from './ListProductsCart.module.css';

export interface ListProductsCartPropsV2 {}

const ListProductsCartV2: FC<ListProductsCartPropsV2> = ({}) => {
  // const handleChangingQuantityProduct = (
  //   newQuantity: number,
  //   index: number,
  // ) => {
  //   if (products[index]) {
  //     const updatedConfiguredCandleDetail = [...products];
  //     if (newQuantity <= 0) {
  //       updatedConfiguredCandleDetail.splice(index, 1);
  //     } else {
  //       updatedConfiguredCandleDetail[index].quantity = newQuantity;
  //     }
  //     onChangeCandleDetailWithQuantity(updatedConfiguredCandleDetail);
  //   }
  // };

  return (
    <>
      {/* <div className={Style.listProductsCart}>
        {products.map((product, index) => (
          <ConfiguredCandleCart
            key={index}
            index={index}
            product={product}
            onChangingQuantityProduct={handleChangingQuantityProduct}
            quantity={product.quantity}
            onSelect={onSelect}
          />
        ))}
        <div className={Style.infoBlock}>
          <div className={Style.priceBlock}>
            <span className={Style.priceTitle}>Итого</span>
            <span className={Style.price}>{price} р</span>
          </div>
          <Button
            text="Заказать"
            onClick={onCreateBasket}
            className={Style[buttonState]}
          />
        </div>
      </div> */}
    </>
  );
};

export default ListProductsCartV2;
