import { FC, useEffect, useState } from 'react';

import Product from './Product';
import { ImageProduct } from '../../typesV2/BaseProduct';

import Style from './ProductsGridSelector.module.css';

export interface ProductsGridSelectorProps<ImageProduct> {
  title: string;
  data: ImageProduct[];
  selectedData?: ImageProduct[];
  onSelectProduct?: (product: ImageProduct) => void;
  onDeselectProduct?: (product: ImageProduct) => void;
  withIndex?: boolean;
}

const ProductsGridSelector: FC<ProductsGridSelectorProps<ImageProduct>> = ({
  title,
  data,
  selectedData,
  onSelectProduct,
  onDeselectProduct,
  withIndex = false,
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
        {withIndex
          ? data.map((product) => (
              <Product
                key={product.id}
                product={product}
                pageUrl="candles"
                onSelectProduct={onSelectProduct}
                onDeselectProduct={onDeselectProduct}
                isSelected={selectedProducts.includes(product)}
                index={selectedProducts.indexOf(product) + 1}
              />
            ))
          : data.map((product) => (
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
