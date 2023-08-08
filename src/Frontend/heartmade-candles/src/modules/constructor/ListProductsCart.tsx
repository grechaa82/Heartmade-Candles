import { FC } from 'react';

import ProductCart from '../../components/constructor/ProductCart';
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
  const handleChangingQuantityProduct = (newQuantity: number, index: number) => {
    const updatedCandleDetailWithQuantity = [...products];
    if (updatedCandleDetailWithQuantity[index]) {
      if (newQuantity <= 0) {
        updatedCandleDetailWithQuantity.splice(index, 1);
        console.log('updatedCandleDetailWithQuantity', updatedCandleDetailWithQuantity);
      } else {
        updatedCandleDetailWithQuantity[index].quantity = newQuantity;
      }
      onChangeCandleDetailWithQuantity(updatedCandleDetailWithQuantity);
    }
  };

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
