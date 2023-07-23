import { FC, useState, ChangeEvent } from 'react';

import { Smell } from '../../../types/Smell';
import Textarea from '../Textarea';
import CheckboxBlock from '../CheckboxBlock';
import PopUp, { PopUpProps } from './PopUp';

import Style from './CreateSmellPopUp.module.css';

export interface CreateSmellPopUpProps extends PopUpProps {
  title: string;
  onSave: (smell: Smell) => void;
}

const CreateSmellPopUp: FC<CreateSmellPopUpProps> = ({ onClose, title, onSave }) => {
  const [smell, setSmell] = useState<Smell>({
    id: 0,
    title: '',
    description: '',
    images: [],
    isActive: false,
    price: 0,
  });
  const [isModified, setIsModified] = useState(false);

  const handleChangeTitle = (event: ChangeEvent<HTMLTextAreaElement>) => {
    setSmell((prev) => ({ ...prev, title: event.target.value }));
    setIsModified(true);
  };

  const handleChangePrice = (event: ChangeEvent<HTMLTextAreaElement>) => {
    setSmell((prev) => ({
      ...prev,
      price: parseFloat(event.target.value),
    }));
    setIsModified(true);
  };

  const handleChangeIsActive = (isActive: boolean) => {
    setSmell((prev) => ({ ...prev, isActive }));
    setIsModified(true);
  };

  const handleChangeDescription = (event: ChangeEvent<HTMLTextAreaElement>) => {
    setSmell((prev) => ({ ...prev, description: event.target.value }));
    setIsModified(true);
  };

  return (
    <PopUp onClose={onClose}>
      <div className={Style.cont}>
        <h2 className={Style.title}>{title}</h2>
        <form className={`${Style.gridContainer} ${Style.formForSmell}`}>
          <div className={`${Style.formItem} ${Style.itemTitle}`}>
            <Textarea
              text={smell.title}
              label="Название"
              limitation={{ limit: 48 }}
              onChange={handleChangeTitle}
            />
          </div>
          <div className={`${Style.formItem} ${Style.itemPrice}`}>
            <Textarea
              text={smell.price.toString()}
              label="Стоимость"
              onChange={handleChangePrice}
            />
          </div>
          <div className={`${Style.formItem} ${Style.itemActive}`}>
            <CheckboxBlock
              text="Активна"
              checked={smell.isActive}
              onChange={handleChangeIsActive}
            />
          </div>
          <div className={`${Style.formItem} ${Style.itemDescription}`}>
            <Textarea
              text={smell.description}
              label="Описание"
              height={175}
              limitation={{ limit: 256 }}
              onChange={handleChangeDescription}
            />
          </div>
          {onSave && isModified && (
            <button
              type="button"
              className={Style.saveButton}
              onClick={() => {
                onSave(smell);
                onClose();
              }}
            >
              Сохранить
            </button>
          )}
        </form>
      </div>
    </PopUp>
  );
};

export default CreateSmellPopUp;
