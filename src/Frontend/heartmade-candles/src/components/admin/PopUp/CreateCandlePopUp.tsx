import { FC, useState, ChangeEvent } from 'react';

import { Candle } from '../../../types/Candle';
import Textarea from '../Textarea';
import ButtonDropdown, { optionData } from '../ButtonDropdown';
import { TypeCandle } from '../../../types/TypeCandle';
import CheckboxBlock from '../CheckboxBlock';
import PopUp, { PopUpProps } from './PopUp';

import Style from './CreateCandlePopUp.module.css';

export interface CreateCandlePopUpProps extends PopUpProps {
  title: string;
  typeCandlesArray: TypeCandle[];
  onSave: (canlde: Candle) => void;
}

const CreateCandlePopUp: FC<CreateCandlePopUpProps> = ({
  onClose,
  title,
  typeCandlesArray,
  onSave,
}) => {
  const [candle, setCandle] = useState<Candle>({
    id: 0,
    title: '',
    description: '',
    images: [],
    isActive: false,
    price: 0,
    weightGrams: 0,
    typeCandle: typeCandlesArray[0],
    createdAt: new Date().toISOString(),
  });

  const [isModified, setIsModified] = useState(false);
  const optionData: optionData[] = typeCandlesArray.map(({ id, title }) => ({
    id: id.toString(),
    title,
  }));

  const handleChangeTitle = (event: ChangeEvent<HTMLTextAreaElement>) => {
    setCandle((prev) => ({ ...prev, title: event.target.value }));
    setIsModified(true);
  };

  const handleChangePrice = (event: ChangeEvent<HTMLTextAreaElement>) => {
    setCandle((prev) => ({
      ...prev,
      price: parseFloat(event.target.value),
    }));
    setIsModified(true);
  };

  const handleChangeWeightGrams = (event: ChangeEvent<HTMLTextAreaElement>) => {
    setCandle((prev) => ({
      ...prev,
      weightGrams: parseFloat(event.target.value),
    }));
    setIsModified(true);
  };

  const handleChangeTypeCandle = (id: string) => {
    const selectedTypeCandle = typeCandlesArray.find(
      (typeCandle) => typeCandle.id.toString() === id,
    );
    if (selectedTypeCandle) {
      setCandle((prev) => ({
        ...prev,
        typeCandle: selectedTypeCandle,
      }));
      setIsModified(true);
    }
  };

  const handleChangeIsActive = (isActive: boolean) => {
    setCandle((prev) => ({ ...prev, isActive }));
    setIsModified(true);
  };

  const handleChangeDescription = (event: ChangeEvent<HTMLTextAreaElement>) => {
    setCandle((prev) => ({ ...prev, description: event.target.value }));
    setIsModified(true);
  };

  return (
    <PopUp onClose={onClose}>
      <div className={Style.cont}>
        <h2 className={Style.title}>{title}</h2>
        <form className={`${Style.gridContainer} ${Style.formForCandle}`}>
          <div className={`${Style.formItem} ${Style.itemTitle}`}>
            <Textarea
              text={candle.title}
              label="Название"
              limitation={{ limit: 48 }}
              onChange={handleChangeTitle}
            />
          </div>
          <div className={`${Style.formItem} ${Style.itemPrice}`}>
            <Textarea
              text={candle.price.toString()}
              label="Стоимость"
              onChange={handleChangePrice}
            />
          </div>
          <div className={`${Style.formItem} ${Style.itemWeightGrams}`}>
            <Textarea
              text={candle.weightGrams.toString()}
              label="Вес в граммах"
              onChange={handleChangeWeightGrams}
            />
          </div>
          <div className={`${Style.formItem} ${Style.itemType}`}>
            <ButtonDropdown
              options={optionData}
              text={candle.typeCandle.title}
              onClick={handleChangeTypeCandle}
            />
          </div>
          <div className={`${Style.formItem} ${Style.itemActive}`}>
            <CheckboxBlock
              text="Активна"
              checked={candle.isActive}
              onChange={handleChangeIsActive}
            />
          </div>
          <div className={`${Style.formItem} ${Style.itemDescription}`}>
            <Textarea
              text={candle.description}
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
                onSave(candle);
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

export default CreateCandlePopUp;
