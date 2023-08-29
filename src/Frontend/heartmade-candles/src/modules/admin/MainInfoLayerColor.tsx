import { FC, useState, ChangeEvent } from 'react';

import { LayerColor } from '../../types/LayerColor';
import Textarea from '../../components/admin/Textarea';
import CheckboxBlock from '../../components/admin/CheckboxBlock';
import ImageSlider from '../../components/admin/ImageSlider';
import { Image } from '../../types/Image';

import Style from './MainInfoLayerColor.module.css';

export interface MainInfoLayerColorProps {
  data: LayerColor;
  onChangesLayerColor: (layerColor: LayerColor) => void;
  onSave?: (saveLayerColor: LayerColor) => void;
}

const MainInfoLayerColor: FC<MainInfoLayerColorProps> = ({ data, onChangesLayerColor, onSave }) => {
  const [layerColor, setLayerColor] = useState<LayerColor>(data);
  const [isModified, setIsModified] = useState(false);

  const handleChangeTitle = (event: ChangeEvent<HTMLTextAreaElement>) => {
    setLayerColor((prev) => ({ ...prev, title: event.target.value }));
    onChangesLayerColor(layerColor);
    setIsModified(true);
  };

  const handleChangePrice = (event: ChangeEvent<HTMLTextAreaElement>) => {
    setLayerColor((prev) => ({
      ...prev,
      price: parseFloat(event.target.value),
    }));
    onChangesLayerColor(layerColor);
    setIsModified(true);
  };

  const handleChangeIsActive = (isActive: boolean) => {
    setLayerColor((prev) => ({ ...prev, isActive }));
    onChangesLayerColor(layerColor);
    setIsModified(true);
  };

  const handleChangeDescription = (event: ChangeEvent<HTMLTextAreaElement>) => {
    setLayerColor((prev) => ({ ...prev, description: event.target.value }));
    onChangesLayerColor(layerColor);
    setIsModified(true);
  };

  const handleChangeImages = (images: Image[]) => {
    setLayerColor((prev) => ({ ...prev, images: [...prev.images, ...images] }));
    onChangesLayerColor(layerColor);
    setIsModified(true);
  };

  const handleSetNewImages = (images: Image[]) => {
    const newLayerColor: LayerColor = { ...layerColor, images: images };
    setLayerColor(newLayerColor);
    onChangesLayerColor(newLayerColor);
    if (onSave) {
      onSave(newLayerColor);
    }
  };

  return (
    <div className={Style.layerColorInfo}>
      <ImageSlider
        images={layerColor.images}
        updateImages={handleSetNewImages}
        addImages={handleChangeImages}
      />
      <form className={`${Style.gridContainer} ${Style.formForLayerColor}`}>
        <div className={`${Style.formItem} ${Style.itemTitle}`}>
          <Textarea
            text={layerColor.title}
            label="Название"
            limitation={{ limit: 48 }}
            onChange={handleChangeTitle}
          />
        </div>
        <div className={`${Style.formItem} ${Style.itemPrice}`}>
          <Textarea
            text={layerColor.pricePerGram.toString()}
            label="Стоимость за грамм"
            onChange={handleChangePrice}
          />
        </div>
        <div className={`${Style.formItem} ${Style.itemActive}`}>
          <CheckboxBlock
            text="Активна"
            checked={layerColor.isActive}
            onChange={handleChangeIsActive}
          />
        </div>
        <div className={`${Style.formItem} ${Style.itemDescription}`}>
          <Textarea
            text={layerColor.description}
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
              onSave(layerColor);
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

export default MainInfoLayerColor;
