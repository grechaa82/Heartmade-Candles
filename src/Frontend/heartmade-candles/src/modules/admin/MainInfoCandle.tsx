import { FC, useState, useEffect, ChangeEvent } from 'react';

import ButtonDropdown, { optionData } from '../../components/admin/ButtonDropdown';
import CheckboxBlock from '../../components/admin/CheckboxBlock';
import Textarea from '../../components/admin/Textarea';
import { Candle } from '../../types/Candle';
import { TypeCandle } from '../../types/TypeCandle';
import ImageSlider from '../../components/admin/ImageSlider';

import Style from './MainInfoCandle.module.css';

export interface MainInfoCandleProps {
  data: Candle;
  fetchTypeCandles: FetchTypeCandle;
  handleChangesCandle: (candle: Candle) => void;
  onSave?: (saveCandle: Candle) => void;
}

export type FetchTypeCandle = () => Promise<TypeCandle[]>;

const MainInfoCandle: FC<MainInfoCandleProps> = ({
  data,
  fetchTypeCandles: fetchTypeCandle,
  handleChangesCandle,
  onSave,
}) => {
  const [candle, setCandle] = useState<Candle>(data);
  const [typesCandle, setTypesCandle] = useState<TypeCandle[]>([]);
  const typeCandlesArray: optionData[] = typesCandle.map(({ id, title }) => ({
    id: id.toString(),
    title,
  }));
  const [isModified, setIsModified] = useState(false);

  useEffect(() => {
    async function getTypeCandles() {
      const data = await fetchTypeCandle();
      setTypesCandle(data);
    }
    getTypeCandles();
  }, [fetchTypeCandle]);

  const handleChangeTitle = (event: ChangeEvent<HTMLTextAreaElement>) => {
    setCandle((prev) => ({ ...prev, title: event.target.value }));
    handleChangesCandle(candle);
    setIsModified(true);
  };

  const handleChangePrice = (event: ChangeEvent<HTMLTextAreaElement>) => {
    setCandle((prev) => ({
      ...prev,
      price: parseFloat(event.target.value),
    }));
    handleChangesCandle(candle);
    setIsModified(true);
  };

  const handleChangeWeightGrams = (event: ChangeEvent<HTMLTextAreaElement>) => {
    setCandle((prev) => ({
      ...prev,
      weightGrams: parseFloat(event.target.value),
    }));
    handleChangesCandle(candle);
    setIsModified(true);
  };

  const handleChangeTypeCandle = (id: string) => {
    const selectedTypeCandle = typesCandle.find((typeCandle) => typeCandle.id.toString() === id);
    if (selectedTypeCandle) {
      setCandle((prev) => ({
        ...prev,
        typeCandle: selectedTypeCandle,
      }));
      handleChangesCandle(candle);
      setIsModified(true);
    }
  };

  const handleChangeIsActive = (isActive: boolean) => {
    setCandle((prev) => ({ ...prev, isActive }));
    handleChangesCandle(candle);
    setIsModified(true);
  };

  const handleChangeDescription = (event: ChangeEvent<HTMLTextAreaElement>) => {
    setCandle((prev) => ({ ...prev, description: event.target.value }));
    handleChangesCandle(candle);
    setIsModified(true);
  };

  return (
    <div className={Style.candleInfo}>
      <ImageSlider imageUrls={candle.imageURL.split(',')} />
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
          <Textarea text={candle.price.toString()} label="Стоимость" onChange={handleChangePrice} />
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
            options={typeCandlesArray}
            text={candle.typeCandle.title}
            onClick={handleChangeTypeCandle}
          />
        </div>
        <div className={`${Style.formItem} ${Style.itemActive}`}>
          <CheckboxBlock text="Активна" checked={candle.isActive} onChange={handleChangeIsActive} />
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

export default MainInfoCandle;