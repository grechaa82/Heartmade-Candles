import { FC } from 'react';

import ProductCart from '../../components/order/ProductCart';
import { BasketItem } from '../../typesV2/order/BasketItem';

import Style from './ListProductsCart.module.css';

export interface ListProductsCartProps {
  products: BasketItem[];
}

const ListProductsCart: FC<ListProductsCartProps> = ({ products }) => {
  return (
    <div className={Style.orderList}>
      {products.map((product, index) => (
        <ProductCart key={index} product={product} />
      ))}
    </div>
  );
};

export default ListProductsCart;
