import React, { FC } from 'react';
import { BaseProduct } from '../types/BaseProduct';
import Checkbox from './Checkbox';

import Style from './ProductBlock.module.css';

export interface ProductBlockProps<T extends BaseProduct> {
  product: T;
  width?: number;
}

const ProductBlock: FC<ProductBlockProps<BaseProduct>> = ({ product, width }) => {
  const productBlockStyle = {
    ...(width && { width: `${width}px` }),
  };

  return (
    <div className={Style.productBlock} style={productBlockStyle}>
      <div className={Style.image}></div>
      <div className={Style.info}>
        <p className={Style.title}>{product.title}</p>
        <p className={Style.description}>{product.description}</p>
      </div>
      <Checkbox checked={product.isActive} />
    </div>
  );
};

export default ProductBlock;
