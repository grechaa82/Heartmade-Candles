import { FC, useState, useEffect } from 'react';

import CandleForm from '../../components/admin/Form/Candle/CandleForm';
import { Candle } from '../../types/Candle';
import { TypeCandle } from '../../types/TypeCandle';
import ImageSlider from '../../components/admin/ImageSlider';
import { Image } from '../../types/Image';

import Style from './MainInfoCandle.module.css';

export interface MainInfoCandleProps {
  data: Candle;
  fetchTypeCandles: FetchTypeCandle;
  onChangesCandle: (candle: Candle) => void;
  onSave?: (saveCandle: Candle) => void;
}

export type FetchTypeCandle = () => Promise<TypeCandle[]>;

const MainInfoCandle: FC<MainInfoCandleProps> = ({
  data,
  fetchTypeCandles: fetchTypeCandle,
  onChangesCandle,
  onSave,
}) => {
  const [candle, setCandle] = useState<Candle>(data);
  const [typesCandle, setTypesCandle] = useState<TypeCandle[]>([]);

  useEffect(() => {
    async function getTypeCandles() {
      const data = await fetchTypeCandle();
      setTypesCandle(data);
    }
    getTypeCandles();
  }, [fetchTypeCandle]);

  const handleOnSubmit = (data: Candle) => {
    const newCandle: Candle = {
      id: data.id,
      title: data.title,
      description: data.description,
      images: candle.images,
      isActive: data.isActive,
      price: data.price,
      weightGrams: data.weightGrams,
      typeCandle: data.typeCandle,
      createdAt: data.createdAt,
    };
    onSave(newCandle);
  };

  const handleChangeImages = (images: Image[]) => {
    const newCandle: Candle = {
      ...candle,
      images: [...candle.images, ...images],
    };
    setCandle(newCandle);
    onChangesCandle(newCandle);
    if (onSave) {
      onSave(newCandle);
    }
  };

  const handleSetNewImages = (images: Image[]) => {
    const newCandle: Candle = { ...candle, images: images };
    setCandle(newCandle);
    onChangesCandle(newCandle);
    if (onSave) {
      onSave(newCandle);
    }
  };

  return (
    <div className={Style.candleInfo}>
      <ImageSlider
        images={candle.images}
        updateImages={handleSetNewImages}
        addImages={handleChangeImages}
      />
      {typesCandle.length > 0 && (
        <CandleForm
          typeCandlesArray={typesCandle}
          defaultValues={candle}
          onSubmit={handleOnSubmit}
        />
      )}
    </div>
  );
};

export default MainInfoCandle;
