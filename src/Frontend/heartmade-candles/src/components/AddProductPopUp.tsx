import { FC, useState } from "react";
import IconRemoveLarge from "../UI/IconRemoveLarge";
import { BaseProduct } from "../types/BaseProduct";
import ProductBlock from "./ProductBlock";

import Style from "./AddProductPopUp.module.css";

export interface AddProductPopUpProps<T extends BaseProduct> {
  allData: T[];
  selectedData: T[];
  onAddProduct: (product: BaseProduct) => void;
  onRemoveProduct: (product: BaseProduct) => void;
  title: string;
  onClose: () => void;
  isDataLoaded: boolean;
  onSave?: () => void;
}

const AddProductPopUp: FC<AddProductPopUpProps<BaseProduct>> = ({
  allData,
  selectedData,
  onAddProduct,
  onRemoveProduct,
  title,
  onClose,
  isDataLoaded,
  onSave,
}) => {
  const [isModified, setIsModified] = useState(false);

  const handleAddProduct = (product: BaseProduct) => {
    onAddProduct(product);
    setIsModified(true);
  };

  const handleRemoveProduct = (product: BaseProduct) => {
    onRemoveProduct(product);
    setIsModified(true);
  };

  const isProductSelected = (product: BaseProduct) => {
    return selectedData.some((p) => p.id === product.id);
  };

  return (
    <div className={Style.overlay}>
      <div className={Style.popUp}>
        <button className={Style.closeButton} onClick={onClose}>
          <IconRemoveLarge color="#777" />
        </button>
        <h2>{title}</h2>
        {isDataLoaded ? (
          <div className={Style.popUpGrid}>
            {allData.map((item: BaseProduct) => (
              <button
                className={`${Style.productButton} ${
                  isProductSelected(item) ? Style.selectedButton : ""
                }`}
                onClick={() =>
                  isProductSelected(item)
                    ? handleRemoveProduct(item)
                    : handleAddProduct(item)
                }
              >
                <ProductBlock key={item.id} product={item} />
              </button>
            ))}
          </div>
        ) : (
          <div>Loading...</div>
        )}
        {onSave && isModified && (
          <button
            type="button"
            className={Style.saveButton}
            onClick={() => {
              onSave();
              onClose();
            }}
          >
            Сохранить
          </button>
        )}
      </div>
    </div>
  );
};

export default AddProductPopUp;
