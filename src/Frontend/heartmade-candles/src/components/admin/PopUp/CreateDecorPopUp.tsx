import { FC, useState, ChangeEvent } from 'react';

import { Decor } from '../../../types/Decor';
import Textarea from '../Textarea';
import CheckboxBlock from '../CheckboxBlock';
import PopUp, { PopUpProps } from './PopUp';

import Style from './CreateDecorPopUp.module.css';

export interface CreateDecorPopUpProps extends PopUpProps {
  title: string;
  onSave: (decor: Decor) => void;
}

const CreateDecorPopUp: FC<CreateDecorPopUpProps> = ({ onClose, title, onSave }) => {
  const [decor, setDecor] = useState<Decor>({
    id: 0,
    title: '',
    description: '',
    images: [],
    isActive: false,
    price: 0,
  });
  const [isModified, setIsModified] = useState(false);

  const handleChangeTitle = (event: ChangeEvent<HTMLTextAreaElement>) => {
    setDecor((prev) => ({ ...prev, title: event.target.value }));
    setIsModified(true);
  };

  const handleChangePrice = (event: ChangeEvent<HTMLTextAreaElement>) => {
    setDecor((prev) => ({
      ...prev,
      price: parseFloat(event.target.value),
    }));
    setIsModified(true);
  };

  const handleChangeIsActive = (isActive: boolean) => {
    setDecor((prev) => ({ ...prev, isActive }));
    setIsModified(true);
  };

  const handleChangeDescription = (event: ChangeEvent<HTMLTextAreaElement>) => {
    setDecor((prev) => ({ ...prev, description: event.target.value }));
    setIsModified(true);
  };

  return (
    <PopUp onClose={onClose}>
      <div className={Style.cont}>
        <h2 className={Style.title}>{title}</h2>
        <form className={`${Style.gridContainer} ${Style.formForDecor}`}>
          <div className={`${Style.formItem} ${Style.itemTitle}`}>
            <Textarea
              text={decor.title}
              label="Название"
              limitation={{ limit: 48 }}
              onChange={handleChangeTitle}
            />
          </div>
          <div className={`${Style.formItem} ${Style.itemPrice}`}>
            <Textarea
              text={decor.price.toString()}
              label="Стоимость"
              onChange={handleChangePrice}
            />
          </div>
          <div className={`${Style.formItem} ${Style.itemActive}`}>
            <CheckboxBlock
              text="Активна"
              checked={decor.isActive}
              onChange={handleChangeIsActive}
            />
          </div>
          <div className={`${Style.formItem} ${Style.itemDescription}`}>
            <Textarea
              text={decor.description}
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
                onSave(decor);
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

export default CreateDecorPopUp;
