import { FC } from 'react';
import IconRemoveLarge from '../UI/IconRemoveLarge';
import { BaseProduct } from '../types/BaseProduct';
import ProductBlock from './ProductBlock';

import Style from './AddProductPopUp.module.css';

export interface AddProductPopUpProps<T extends BaseProduct> {
  allData: T[];
  selectedData: T[];
  onAddProduct: (product: BaseProduct) => void;
  onRemoveProduct: (product: BaseProduct) => void;
  title: string;
  onClose: () => void;
  isDataLoaded: boolean;
}

const AddProductPopUp: FC<AddProductPopUpProps<BaseProduct>> = ({
  allData,
  selectedData,
  onAddProduct,
  onRemoveProduct,
  title,
  onClose,
  isDataLoaded,
}) => {
  console.log('AddProductPopUp', allData, selectedData);

  const handleAddProduct = (product: BaseProduct) => {
    console.log('handleAddProduct');
    onAddProduct(product);
  };

  const handleRemoveProduct = (product: BaseProduct) => {
    console.log('handleRemoveProduct');
    onRemoveProduct(product);
  };

  const isProductSelected = (product: BaseProduct) => {
    console.log(
      'isProductSelected',
      selectedData.some((p) => p.id === product.id),
    );
    return selectedData.some((p) => p.id === product.id);
  };

  return (
    <div className={Style.overlay}>
      <div className={Style.popUp}>
        <button className={Style.closeBtn} onClick={onClose}>
          <IconRemoveLarge color="#777" />
        </button>
        <h2>{title}</h2>
        {isDataLoaded ? (
          <div className={Style.popUpGrid}>
            {allData.map((item: BaseProduct) => (
              <button
                className={`${Style.productButton} ${
                  isProductSelected(item) ? Style.selectedButton : ''
                }`}
                onClick={() =>
                  isProductSelected(item) ? handleRemoveProduct(item) : handleAddProduct(item)
                }
              >
                <ProductBlock key={item.id} product={item} />
              </button>
            ))}
          </div>
        ) : (
          <div>Loading...</div>
        )}
      </div>
    </div>
  );
};

export default AddProductPopUp;
