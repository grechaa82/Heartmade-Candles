import { FC, useState, useEffect } from 'react';

import CandleForm from '../../components/admin/Form/Candle/CandleForm';
import { Candle } from '../../types/Candle';
import { TypeCandle } from '../../types/TypeCandle';
import ImageSlider from '../../components/admin/ImageSlider';
import { Image } from '../../types/Image';

import Style from './MainInfoCandle.module.css';

export interface MainInfoCandleProps {
  data: Candle;
  allTypeCandle: TypeCandle[];
  onSave?: (saveCandle: Candle) => void;
}

const MainInfoCandle: FC<MainInfoCandleProps> = ({
  data,
  allTypeCandle,
  onSave,
}) => {
  const [candle, setCandle] = useState<Candle>(data);

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
    if (onSave) {
      onSave(newCandle);
    }
  };

  const handleSetNewImages = (images: Image[]) => {
    const newCandle: Candle = { ...candle, images: images };
    setCandle(newCandle);
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
      {allTypeCandle.length > 0 && (
        <CandleForm
          typeCandlesArray={allTypeCandle}
          defaultValues={candle}
          onSubmit={handleOnSubmit}
        />
      )}
    </div>
  );
};

export default MainInfoCandle;
