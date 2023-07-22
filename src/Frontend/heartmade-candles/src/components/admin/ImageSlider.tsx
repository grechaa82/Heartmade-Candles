import { FC, useState } from 'react';

import IconChevronLeftLarge from '../../UI/IconChevronLeftLarge';
import IconChevronRightLarge from '../../UI/IconChevronRightLarge';
import { Image } from '../../types/Image';

import Style from './ImageSlider.module.css';

interface ImageSliderProps {
  images: Image[];
}

const ImageSlider: FC<ImageSliderProps> = ({ images }) => {
  const [currentImageIndex, setCurrentImageIndex] = useState(0);
  const urlToImage = 'http://localhost:5000/StaticFiles/Images/';

  const handleChangeImage = (index: number, moveToIndex?: number) => {
    let current = index;
    if (moveToIndex && moveToIndex == -1) {
      current -= 1;
      if (current < 0) {
        current = images.length - 1;
      }
    } else if (moveToIndex && moveToIndex == 1) {
      current += 1;
      if (current >= images.length) {
        current = 0;
      }
    }
    setCurrentImageIndex(current);
  };

  if (images.length === 0) {
    return (
      <div className={Style.image}>
        <div className={Style.imageNoAvailable}>Изображение отсутствует</div>
      </div>
    );
  }

  return (
    <div className={Style.image}>
      <img
        src={urlToImage + images[currentImageIndex].fileName}
        alt={images[currentImageIndex].alternativeName}
      />
      <div className={Style.slider}>
        <button
          className={Style.iconChevronBtn}
          onClick={() => handleChangeImage(currentImageIndex, -1)}
        >
          <IconChevronLeftLarge color="#777" />
        </button>
        {images.map((image, index) => (
          <img
            key={index}
            src={urlToImage + image.fileName}
            className={Style.sliderImage}
            onClick={() => handleChangeImage(index)}
          />
        ))}
        <button
          className={Style.iconChevronBtn}
          onClick={() => handleChangeImage(currentImageIndex, 1)}
        >
          <IconChevronRightLarge color="#777" />
        </button>
      </div>
    </div>
  );
};

export default ImageSlider;
