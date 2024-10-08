import { FC, useEffect, useState } from 'react';

import { BaseProduct } from '../../../types/BaseProduct';
import ProductBlock from '../ProductBlock';
import PopUp, { PopUpProps } from '../../shared/PopUp/PopUp';

import Style from './AddProductPopUp.module.css';

export interface AddProductPopUpProps<T extends BaseProduct>
  extends PopUpProps {
  title: string;
  selectedData: T[];
  allData: T[];
  onSave: (product: BaseProduct[]) => void;
  fetchQuery?: {
    fetchNextPage: () => void;
    hasNextPage: boolean;
  };
}

const AddProductPopUp: FC<AddProductPopUpProps<BaseProduct>> = ({
  onClose,
  title,
  selectedData,
  allData,
  onSave,
  fetchQuery,
}) => {
  const [tempSelectedData, setTempSelectedData] =
    useState<BaseProduct[]>(selectedData);
  const [isModified, setIsModified] = useState(false);

  const handleAddProduct = (product: BaseProduct) => {
    const newTempSelectedData = [...tempSelectedData, product];
    setTempSelectedData(newTempSelectedData);
    setIsModified(true);
  };

  const handleRemoveProduct = (product: BaseProduct) => {
    const newTempSelectedData = tempSelectedData.filter(
      (p) => p.id !== product.id,
    );
    setTempSelectedData(newTempSelectedData);
    setIsModified(true);
  };

  const handleSave = () => {
    onSave(tempSelectedData);
    onClose();
  };

  useEffect(() => {
    const fetchAllData = async () => {
      if (fetchQuery.hasNextPage) {
        fetchQuery.fetchNextPage();
      }
    };

    fetchAllData();
  }, [fetchQuery]);

  return (
    <PopUp onClose={onClose}>
      <div className={Style.container}>
        <p className={Style.title}>{title}</p>
        {allData.length > 0 ? (
          <div className={Style.popUpGrid}>
            {allData.map((item: BaseProduct) => (
              <button
                key={item.id}
                className={`${Style.productButton} ${
                  tempSelectedData.some((p) => p.id === item.id)
                    ? Style.selectedButton
                    : ''
                }`}
                onClick={() =>
                  tempSelectedData.some((p) => p.id === item.id)
                    ? handleRemoveProduct(item)
                    : handleAddProduct(item)
                }
              >
                <ProductBlock product={item} />
              </button>
            ))}
          </div>
        ) : (
          <div>Loading...</div>
        )}
        <button
          type="button"
          className={`${Style.saveButton} ${
            isModified && Style.activeSaveButton
          }`}
          onClick={handleSave}
        >
          Сохранить
        </button>
      </div>
    </PopUp>
  );
};

export default AddProductPopUp;
