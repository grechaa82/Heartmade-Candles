import { FC, useEffect, useState } from 'react';

import Product from '../../components/constructor/Product';
import { ImageProduct } from '../../typesV2/BaseProduct';

import Style from './ProductsGrid.module.css';

export interface ProductsGridProps<ImageProduct> {
  title: string;
  data: ImageProduct[];
  selectedData?: ImageProduct[];
  onSelectProduct?: (product: ImageProduct) => void;
  onDeselectProduct?: (product: ImageProduct) => void;
}

const ProductsGridSelector: FC<ProductsGridProps<ImageProduct>> = ({
  title,
  data,
  selectedData,
  onSelectProduct,
  onDeselectProduct,
}) => {
  const [selectedProducts, setSelectedProducts] = useState<ImageProduct[]>([]);

  useEffect(() => {
    if (selectedData) {
      setSelectedProducts(selectedData);
    }
  }, [selectedData]);

  return (
    <div className={Style.productGrid}>
      <h2>{title}</h2>
      <div className={Style.grid}>
        {data.map((product) => (
          <Product
            key={product.id}
            product={product}
            pageUrl="candles"
            onSelectProduct={onSelectProduct}
            onDeselectProduct={onDeselectProduct}
            isSelected={selectedProducts.includes(product)}
          />
        ))}
      </div>
    </div>
  );
};

export default ProductsGridSelector;
