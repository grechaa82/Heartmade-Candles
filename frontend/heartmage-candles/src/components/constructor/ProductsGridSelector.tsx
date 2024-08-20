import { FC, useState } from 'react';

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
  const [isOpen, setIsOpen] = useState(true);

  const toggleOpen = () => {
    setIsOpen((prev) => !prev);
  };

  return (
    <div className={Style.productGrid}>
      <div className={Style.headerContainer}>
        <h2 className={Style.sectionHeader} onClick={toggleOpen}>
          {title}
        </h2>
        <button className={Style.toggleButton} onClick={toggleOpen}>
          {isOpen ? '–' : '+'}
        </button>
      </div>
      {isOpen && (
        <div className={Style.grid}>
          {data.map((product) => (
            <Product
              key={product.id}
              product={product}
              pageUrl="candles"
              onSelectProduct={onSelectProduct}
              onDeselectProduct={onDeselectProduct}
              isSelected={selectedData?.some(
                (selected) => selected?.id === product.id,
              )}
              index={
                withIndex && selectedData
                  ? selectedData.findIndex(
                      (selected) => selected?.id === product.id,
                    ) + 1
                  : undefined
              }
            />
          ))}
        </div>
      )}
    </div>
  );
};

export default ProductsGridSelector;
