import { FC } from 'react';
import IconRemoveLarge from '../UI/IconRemoveLarge';
import { BaseProduct } from '../types/BaseProduct';
import ProductBlock from './ProductBlock';

import Style from './AddProductPopUp.module.css';

export interface AddProductPopUpProps<T extends BaseProduct> {
  data: T[];
  title: string;
  onClose: () => void;
}

const AddProductPopUp: FC<AddProductPopUpProps<BaseProduct>> = ({ title, data, onClose }) => {
  return (
    <div className={Style.overlay}>
      <div className={Style.popUp}>
        <button className={Style.closeBtn} onClick={onClose}>
          <IconRemoveLarge color="#777" />
        </button>
        <h2>{title}</h2>
        <div className={Style.popUpGrid}>
          {data.map((item: BaseProduct) => (
            <ProductBlock key={item.id} product={item} />
          ))}
        </div>
      </div>
    </div>
  );
};

export default AddProductPopUp;
