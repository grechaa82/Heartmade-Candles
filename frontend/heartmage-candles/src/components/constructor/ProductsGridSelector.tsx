import { FC } from 'react';

import Product from './Product';
import { ImageProduct } from '../../typesV2/shared/BaseProduct';

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
            isSelected={selectedData?.some(
              (selected) => selected.id === product.id,
            )}
            index={
              withIndex && selectedData
                ? selectedData.findIndex(
                    (selected) => selected.id === product.id,
                  ) + 1
                : undefined
            }
          />
        ))}
      </div>
    </div>
  );
};

export default ProductsGridSelector;
