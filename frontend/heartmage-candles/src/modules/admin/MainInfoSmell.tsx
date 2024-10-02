import { FC } from 'react';

import { Smell } from '../../types/Smell';
import SmellForm from '../../components/admin/Form/Smell/SmellForm';

import Style from './MainInfoSmell.module.css';

export interface MainInfoSmellProps {
  data: Smell;
  onSave?: (saveSmell: Smell) => void;
}

const MainInfoSmell: FC<MainInfoSmellProps> = ({ data, onSave }) => {
  const handleOnSubmit = (smell: Smell) => {
    const newSmell: Smell = {
      id: smell.id,
      title: smell.title,
      description: smell.description,
      images: data.images,
      isActive: smell.isActive,
      price: smell.price,
    };
    onSave(newSmell);
  };

  return (
    <div className={Style.smellInfo}>
      <SmellForm defaultValues={data} onSubmit={handleOnSubmit} />
    </div>
  );
};

export default MainInfoSmell;
