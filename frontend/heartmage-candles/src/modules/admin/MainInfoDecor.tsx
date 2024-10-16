import { FC, useState } from 'react';

import { Decor } from '../../types/Decor';
import ImageSlider from '../../components/admin/ImageSlider';
import { Image } from '../../types/Image';
import DecorForm from '../../components/admin/Form/Decor/DecorForm';

import Style from './MainInfoDecor.module.css';

export interface MainInfoDecorProps {
  data: Decor;
  onSave: (saveDecor: Decor) => void;
}

const MainInfoDecor: FC<MainInfoDecorProps> = ({ data, onSave }) => {
  const [decor, setDecor] = useState<Decor>(data);

  const handleOnSubmit = (data: Decor) => {
    const newDecor: Decor = {
      id: data.id,
      title: data.title,
      description: data.description,
      images: decor.images,
      isActive: data.isActive,
      price: data.price,
    };
    onSave(newDecor);
  };

  const handleChangeImages = (images: Image[]) => {
    const newDecor: Decor = { ...decor, images: [...decor.images, ...images] };
    setDecor(newDecor);
    onSave(newDecor);
  };

  const handleSetNewImages = (images: Image[]) => {
    const newDecor: Decor = { ...decor, images: images };
    setDecor(newDecor);
    onSave(newDecor);
  };

  return (
    <div className={Style.decorInfo}>
      <ImageSlider
        images={decor.images}
        updateImages={handleSetNewImages}
        addImages={handleChangeImages}
      />
      <DecorForm defaultValues={decor} onSubmit={handleOnSubmit} />
    </div>
  );
};

export default MainInfoDecor;
