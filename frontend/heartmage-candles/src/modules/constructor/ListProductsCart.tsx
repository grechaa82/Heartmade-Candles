import { FC } from 'react';

import ProductCart from '../../components/constructor/ProductCart';
import { ConfiguredCandleDetail } from '../../typesV2/constructor/ConfiguredCandleDetail';
import Button from '../../components/shared/Button';

import Style from './ListProductsCart.module.css';

export interface ListProductsCartProps {
  products: ConfiguredCandleDetail[];
  onChangeCandleDetailWithQuantity: (
    CandleDetailWithQuantity: ConfiguredCandleDetail[]
  ) => void;
  price: number;
  onCreateBasket: () => void;
}

const ListProductsCart: FC<ListProductsCartProps> = ({
  products,
  onChangeCandleDetailWithQuantity,
  price,
  onCreateBasket,
}) => {
  const handleChangingQuantityProduct = (
    newQuantity: number,
    index: number
  ) => {
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
        <div className={Style.infoBlock}>
          <div className={Style.priceBlock}>
            <span className={Style.priceTitle}>Итого</span>
            <span className={Style.price}>{price} р</span>
          </div>
          <div className={Style.createBtn}>
            <Button color="#6FCF97" text="Заказать" onClick={onCreateBasket} />
          </div>
        </div>
      </div>
    </>
  );
};

export default ListProductsCart;
