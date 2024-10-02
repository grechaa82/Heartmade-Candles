import { FC, useState } from 'react';

import { Wick } from '../../types/Wick';
import ImageSlider from '../../components/admin/ImageSlider';
import { Image } from '../../types/Image';
import WickForm from '../../components/admin/Form/Wick/WickForm';

import Style from './MainInfoWick.module.css';

export interface MainInfoWickProps {
  data: Wick;
  onSave?: (saveWick: Wick) => void;
}

const MainInfoWick: FC<MainInfoWickProps> = ({ data, onSave }) => {
  const [wick, setWick] = useState<Wick>(data);

  const handleOnSubmit = (data: Wick) => {
    const newWick: Wick = {
      id: data.id,
      title: data.title,
      description: data.description,
      images: wick.images,
      isActive: data.isActive,
      price: data.price,
    };
    onSave(newWick);
  };

  const handleChangeImages = (images: Image[]) => {
    const newWick: Wick = { ...wick, images: [...wick.images, ...images] };
    setWick(newWick);
    onSave(newWick);
  };

  const handleSetNewImages = (images: Image[]) => {
    const newWick: Wick = { ...wick, images: images };
    setWick(newWick);
    onSave(newWick);
  };

  return (
    <div className={Style.wickInfo}>
      <ImageSlider
        images={wick.images}
        updateImages={handleSetNewImages}
        addImages={handleChangeImages}
      />
      <WickForm defaultValues={wick} onSubmit={handleOnSubmit} />
    </div>
  );
};

export default MainInfoWick;
