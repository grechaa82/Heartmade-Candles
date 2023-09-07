import { FC, useState, ChangeEvent } from 'react';

import { LayerColor } from '../../../types/LayerColor';
import Textarea from '../Textarea';
import CheckboxBlock from '../CheckboxBlock';
import PopUp, { PopUpProps } from './PopUp';

import Style from './CreateLayerColorPopUp.module.css';

export interface CreateLayerColorPopUpProps extends PopUpProps {
  title: string;
  onSave: (layerColor: LayerColor) => void;
}

const CreateLayerColorPopUp: FC<CreateLayerColorPopUpProps> = ({ onClose, title, onSave }) => {
  const [LayerColor, setLayerColor] = useState<LayerColor>({
    id: 0,
    title: '',
    description: '',
    images: [],
    isActive: false,
    pricePerGram: 0,
  });
  const [isModified, setIsModified] = useState(false);

  const handleChangeTitle = (event: ChangeEvent<HTMLTextAreaElement>) => {
    setLayerColor((prev) => ({ ...prev, title: event.target.value }));
    setIsModified(true);
  };

  const handleChangePricePerGram = (event: ChangeEvent<HTMLTextAreaElement>) => {
    setLayerColor((prev) => ({
      ...prev,
      pricePerGram: parseFloat(event.target.value),
    }));
    setIsModified(true);
  };

  const handleChangeIsActive = (isActive: boolean) => {
    setLayerColor((prev) => ({ ...prev, isActive }));
    setIsModified(true);
  };

  const handleChangeDescription = (event: ChangeEvent<HTMLTextAreaElement>) => {
    setLayerColor((prev) => ({ ...prev, description: event.target.value }));
    setIsModified(true);
  };

  return (
    <PopUp onClose={onClose}>
      <div className={Style.container}>
        <p className={Style.title}>{title}</p>
        <form className={`${Style.gridContainer} ${Style.formForLayerColor}`}>
          <div className={`${Style.formItem} ${Style.itemTitle}`}>
            <Textarea
              text={LayerColor.title}
              label="Название"
              limitation={{ limit: 48 }}
              onChange={handleChangeTitle}
            />
          </div>
          <div className={`${Style.formItem} ${Style.itemPrice}`}>
            <Textarea
              text={LayerColor.pricePerGram.toString()}
              label="Стоимость за грамм"
              onChange={handleChangePricePerGram}
            />
          </div>
          <div className={`${Style.formItem} ${Style.itemActive}`}>
            <CheckboxBlock
              text="Активна"
              checked={LayerColor.isActive}
              onChange={handleChangeIsActive}
            />
          </div>
          <div className={`${Style.formItem} ${Style.itemDescription}`}>
            <Textarea
              text={LayerColor.description}
              label="Описание"
              height={175}
              limitation={{ limit: 256 }}
              onChange={handleChangeDescription}
            />
          </div>
          {onSave && (
            <button
              type="button"
              className={`${Style.saveButton} ${isModified && Style.activeSaveButton}`}
              onClick={() => {
                onSave(LayerColor);
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

export default CreateLayerColorPopUp;
