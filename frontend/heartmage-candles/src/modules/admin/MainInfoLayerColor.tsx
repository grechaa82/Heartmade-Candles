import { FC, useState } from 'react';

import { LayerColor } from '../../types/LayerColor';
import ImageSlider from '../../components/admin/ImageSlider';
import { Image } from '../../types/Image';
import LayerColorForm from '../../components/admin/Form/LayerColor/LayerColorForm';

import Style from './MainInfoLayerColor.module.css';

export interface MainInfoLayerColorProps {
  data: LayerColor;
  onSave?: (saveLayerColor: LayerColor) => void;
}

const MainInfoLayerColor: FC<MainInfoLayerColorProps> = ({ data, onSave }) => {
  const [layerColor, setLayerColor] = useState<LayerColor>(data);

  const handleOnSubmit = (data: LayerColor) => {
    const newLayerColor: LayerColor = {
      id: 0,
      title: data.title,
      description: data.description,
      images: layerColor.images,
      isActive: data.isActive,
      pricePerGram: data.pricePerGram,
    };
    onSave(newLayerColor);
  };

  const handleChangeImages = (images: Image[]) => {
    const newLayerColor: LayerColor = {
      ...layerColor,
      images: [...layerColor.images, ...images],
    };
    setLayerColor(newLayerColor);
    onSave(newLayerColor);
  };

  const handleSetNewImages = (images: Image[]) => {
    const newLayerColor: LayerColor = { ...layerColor, images: images };
    setLayerColor(newLayerColor);
    onSave(newLayerColor);
  };

  return (
    <div className={Style.layerColorInfo}>
      <ImageSlider
        images={layerColor.images}
        updateImages={handleSetNewImages}
        addImages={handleChangeImages}
      />
      <LayerColorForm defaultValues={layerColor} onSubmit={handleOnSubmit} />
    </div>
  );
};

export default MainInfoLayerColor;
