import { FC, useState } from 'react';

import Product from '../../components/constructor/Product';
import { ImageProduct } from '../../typesV2/BaseProduct';

import Style from './ProductsGrid.module.css';

export interface ProductsGridProps<ImageProduct> {
  title: string;
  data: ImageProduct[];
}

const ProductsGrid: FC<ProductsGridProps<ImageProduct>> = ({ title, data }) => {
  const [products, setProducts] = useState<ImageProduct[]>(data);

  return (
    <div className={Style.productGrid}>
      <h2>{title}</h2>
      <div className={Style.grid}>
        {products.map((product) => (
          <Product key={product.id} product={product} pageUrl="candles" />
        ))}
      </div>
    </div>
  );
};

export default ProductsGrid;
