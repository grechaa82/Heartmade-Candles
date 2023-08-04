import { FC, useEffect, useState } from 'react';

import ProductCart from '../../components/constructor/ProductCart';
import { ImageProduct } from '../../typesV2/BaseProduct';
import { CandleDetailWithQuantity } from '../../typesV2/BaseProduct';

import Style from './ListProductsCart.module.css';

export interface ListProductsCartProps {
  products: CandleDetailWithQuantity[];
  onChangeCandleDetailWithQuantity: (CandleDetailWithQuantity: CandleDetailWithQuantity[]) => void;
}

const ListProductsCart: FC<ListProductsCartProps> = ({
  products,
  onChangeCandleDetailWithQuantity,
}) => {
  const [candleDetailWithQuantity, setCandleDetailWithQuantity] =
    useState<CandleDetailWithQuantity[]>(products);

  const handleChangingQuantityProduct = (newQuantity: number, index: number) => {
    const updatedCandleDetailWithQuantity = [...products];
    if (updatedCandleDetailWithQuantity[index]) {
      updatedCandleDetailWithQuantity[index].quantity = newQuantity;
      setCandleDetailWithQuantity(updatedCandleDetailWithQuantity);
      onChangeCandleDetailWithQuantity(candleDetailWithQuantity);
    }
  };

  useEffect(() => {
    setCandleDetailWithQuantity(products);
  }, [products]);

  return (
    <>
      <div className={Style.listProductsCart}>
        {products.map((product, index) => (
          <ProductCart
            key={index}
            index={index}
            product={product.candle}
            onChangingQuantityProduct={handleChangingQuantityProduct}
            quantity={product.quantity}
          />
        ))}
      </div>
    </>
  );
};

export default ListProductsCart;
