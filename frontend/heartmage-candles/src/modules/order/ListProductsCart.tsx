import { FC } from 'react';

import ProductCart from '../../components/order/ProductCart';
import { CandleDetailWithQuantityAndPrice } from '../../typesV2/BaseProduct';

import Style from './ListProductsCart.module.css';

export interface ListProductsCartProps {
  products: CandleDetailWithQuantityAndPrice[];
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
