import { FC, useEffect, useState } from 'react';

import { BaseProduct } from '../../../types/BaseProduct';
import ProductBlock from '../ProductBlock';
import PopUp, { PopUpProps } from '../../shared/PopUp/PopUp';

import Style from './AddProductPopUp.module.css';

export interface AddProductPopUpProps<T extends BaseProduct>
  extends PopUpProps {
  title: string;
  selectedData: T[];
  setSelectedData: (data: T[]) => void;
  fetchAllData?: () => Promise<T[]>;
  onSave: (product: BaseProduct[]) => void;
}

const AddProductPopUp: FC<AddProductPopUpProps<BaseProduct>> = ({
  onClose,
  title,
  selectedData,
  setSelectedData,
  fetchAllData,
  onSave,
}) => {
  const [allData, setAllData] = useState<BaseProduct[]>([]);
  const [newSelectedData, setNewSelectedData] = useState<BaseProduct[]>([]);
  const [isDataLoaded, setIsDataLoaded] = useState(false);
  const [isModified, setIsModified] = useState(false);

  const handleAddProduct = (product: BaseProduct) => {
    const newSelectedData = [...selectedData, product];
    setNewSelectedData(newSelectedData);
    setIsModified(true);
  };

  const handleRemoveProduct = (product: BaseProduct) => {
    const newSelectedData = selectedData.filter((p) => p.id !== product.id);
    setNewSelectedData(newSelectedData);
    setIsModified(true);
  };

  useEffect(() => {
    const fetchData = async () => {
      try {
        const newData = await fetchAllData?.();
        setAllData(newData || []);
        setIsDataLoaded(true);
      } catch (error) {
        console.error(error);
      }
    };

    setNewSelectedData(selectedData);
    fetchData();
  }, []);

  return (
    <PopUp onClose={onClose}>
      <div className={Style.container}>
        <p className={Style.title}>{title}</p>
        {isDataLoaded ? (
          <div className={Style.popUpGrid}>
            {allData.map((item: BaseProduct) => (
              <button
                key={item.id}
                className={`${Style.productButton} ${
                  newSelectedData.some((p) => p.id === item.id)
                    ? Style.selectedButton
                    : ''
                }`}
                onClick={() =>
                  newSelectedData.some((p) => p.id === item.id)
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
        {onSave && (
          <button
            type="button"
            className={`${Style.saveButton} ${
              isModified && Style.activeSaveButton
            }`}
            onClick={() => {
              onSave(newSelectedData);
              onClose();
            }}
          >
            Сохранить
          </button>
        )}
      </div>
    </PopUp>
  );
};

export default AddProductPopUp;
