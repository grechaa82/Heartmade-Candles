import { FC } from 'react';

import ProductCart from '../../components/constructor/ProductCart';
import { ConfiguredCandleDetail } from '../../typesV2/constructor/ConfiguredCandleDetail';

import Style from './ListProductsCart.module.css';

export interface ListProductsCartProps {
  products: ConfiguredCandleDetail[];
  onChangeCandleDetailWithQuantity: (CandleDetailWithQuantity: ConfiguredCandleDetail[]) => void;
}

const ListProductsCart: FC<ListProductsCartProps> = ({
  products,
  onChangeCandleDetailWithQuantity,
}) => {
  const handleChangingQuantityProduct = (newQuantity: number, index: number) => {
    if (products[index]) {
      const updatedConfiguredCandleDetail = [...products];
      if (newQuantity <= 0) {
        updatedConfiguredCandleDetail.splice(index, 1);
      } else {
        updatedConfiguredCandleDetail[index].quantity = newQuantity;
      }
      onChangeCandleDetailWithQuantity(updatedConfiguredCandleDetail);
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
