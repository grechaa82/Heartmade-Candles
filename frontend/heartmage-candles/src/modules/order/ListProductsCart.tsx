import { FC } from 'react';

import ProductCart from '../../components/order/ProductCart';
import { OrderItem } from '../../typesV2/order/OrderItem';

import Style from './ListProductsCart.module.css';

export interface ListProductsCartProps {
  products: OrderItem[];
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
