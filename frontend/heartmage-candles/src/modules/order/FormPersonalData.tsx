import { FC } from 'react';

import Input from '../../components/shared/Input';

import Style from './FormPersonalData.module.css';

export interface FormPersonalDataProps {
  itemsFormPersonalData: ItemFormPersonalData[];
}

export interface ItemFormPersonalData {
  label: string;
  value: string;
  onChange: (value: string) => void;
  isRequired: boolean;
  validation: (value: string) => boolean;
}

const FormPersonalData: FC<FormPersonalDataProps> = ({
  itemsFormPersonalData,
}) => {
  return (
    <div>
      <div className={Style.title}>Личные данные</div>
      <form action="" className={Style.form}>
        {/* {itemsFormPersonalData.map((item, index) => (
          <Input
            label={item.label}
            value={item.value}
            required={item.isRequired}
            onChange={item.onChange}
            validation={item.validation}
            key={index}
          />
        ))} */}
      </form>
    </div>
  );
};

export default FormPersonalData;
