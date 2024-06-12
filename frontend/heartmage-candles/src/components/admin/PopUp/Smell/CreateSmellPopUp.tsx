import { FC } from 'react';

import { Smell } from '../../../../types/Smell';
import PopUp, { PopUpProps } from '../PopUp';
import SmellForm from '../../Form/Smell/SmellForm';

import Style from './CreateSmellPopUp.module.css';

export interface CreateSmellPopUpProps extends PopUpProps {
  title: string;
  onSave: (smell: Smell) => void;
}

const CreateSmellPopUp: FC<CreateSmellPopUpProps> = ({
  onClose,
  title,
  onSave,
}) => {
  const handleOnSubmit = (data: Smell) => {
    const newSmell: Smell = {
      id: 0,
      title: data.title,
      description: data.description,
      images: [],
      isActive: data.isActive,
      price: data.price,
    };
    onSave(newSmell);
    onClose();
  };

  return (
    <PopUp onClose={onClose}>
      <div className={Style.container}>
        <p className={Style.title}>{title}</p>
        <SmellForm onSubmit={handleOnSubmit} />
      </div>
    </PopUp>
  );
};

export default CreateSmellPopUp;
