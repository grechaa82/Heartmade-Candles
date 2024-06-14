import { FC, useState } from 'react';

import { Smell } from '../../types/Smell';
import SmellForm from '../../components/admin/Form/Smell/SmellForm';

import Style from './MainInfoSmell.module.css';

export interface MainInfoSmellProps {
  data: Smell;
  handleChangesSmell: (smell: Smell) => void;
  onSave?: (saveSmell: Smell) => void;
}

const MainInfoSmell: FC<MainInfoSmellProps> = ({
  data,
  handleChangesSmell,
  onSave,
}) => {
  const [smell, setSmell] = useState<Smell>(data);

  const handleOnSubmit = (data: Smell) => {
    const newSmell: Smell = {
      id: data.id,
      title: data.title,
      description: data.description,
      images: smell.images,
      isActive: data.isActive,
      price: data.price,
    };
    onSave(newSmell);
  };

  return (
    <div className={Style.smellInfo}>
      <SmellForm defaultValues={smell} onSubmit={handleOnSubmit} />
    </div>
  );
};

export default MainInfoSmell;
