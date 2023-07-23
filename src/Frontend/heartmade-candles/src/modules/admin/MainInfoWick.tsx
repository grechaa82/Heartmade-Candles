import { FC, useState, ChangeEvent } from 'react';

import { Wick } from '../../types/Wick';
import Textarea from '../../components/admin/Textarea';
import CheckboxBlock from '../../components/admin/CheckboxBlock';
import ImageSlider from '../../components/admin/ImageSlider';

import Style from './MainInfoWick.module.css';

export interface MainInfoWickProps {
  data: Wick;
  handleChangesWick: (wick: Wick) => void;
  onSave?: (saveWick: Wick) => void;
}

const MainInfoWick: FC<MainInfoWickProps> = ({ data, handleChangesWick, onSave }) => {
  const [wick, setWick] = useState<Wick>(data);
  const [isModified, setIsModified] = useState(false);

  const handleChangeTitle = (event: ChangeEvent<HTMLTextAreaElement>) => {
    setWick((prev) => ({ ...prev, title: event.target.value }));
    handleChangesWick(wick);
    setIsModified(true);
  };

  const handleChangePrice = (event: ChangeEvent<HTMLTextAreaElement>) => {
    setWick((prev) => ({
      ...prev,
      price: parseFloat(event.target.value),
    }));
    handleChangesWick(wick);
    setIsModified(true);
  };

  const handleChangeIsActive = (isActive: boolean) => {
    setWick((prev) => ({ ...prev, isActive }));
    handleChangesWick(wick);
    setIsModified(true);
  };

  const handleChangeDescription = (event: ChangeEvent<HTMLTextAreaElement>) => {
    setWick((prev) => ({ ...prev, description: event.target.value }));
    handleChangesWick(wick);
    setIsModified(true);
  };

  return (
    <div className={Style.wickInfo}>
      {/* <ImageSlider images={wick.images} /> */}
      <form className={`${Style.gridContainer} ${Style.formForWick}`}>
        <div className={`${Style.formItem} ${Style.itemTitle}`}>
          <Textarea
            text={wick.title}
            label="Название"
            limitation={{ limit: 48 }}
            onChange={handleChangeTitle}
          />
        </div>
        <div className={`${Style.formItem} ${Style.itemPrice}`}>
          <Textarea text={wick.price.toString()} label="Стоимость" onChange={handleChangePrice} />
        </div>
        <div className={`${Style.formItem} ${Style.itemActive}`}>
          <CheckboxBlock text="Активна" checked={wick.isActive} onChange={handleChangeIsActive} />
        </div>
        <div className={`${Style.formItem} ${Style.itemDescription}`}>
          <Textarea
            text={wick.description}
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
              onSave(wick);
              setIsModified(false);
            }}
          >
            Сохранить
          </button>
        )}
      </form>
    </div>
  );
};

export default MainInfoWick;
