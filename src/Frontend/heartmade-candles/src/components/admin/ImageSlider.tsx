import { FC, useState } from 'react';

import IconChevronLeftLarge from '../../UI/IconChevronLeftLarge';
import IconChevronRightLarge from '../../UI/IconChevronRightLarge';

import Style from './ImageSlider.module.css';

interface ImageSliderProps {
  imageUrls: string[];
}

const ImageSlider: FC<ImageSliderProps> = ({ imageUrls }) => {
  const [currentImageIndex, setCurrentImageIndex] = useState(0);

  const handleChangeImage = (index: number, moveToIndex?: number) => {
    let current = index;
    if (moveToIndex && moveToIndex == -1) {
      current -= 1;
      if (current < 0) {
        current = imageUrls.length - 1;
      }
    } else if (moveToIndex && moveToIndex == 1) {
      current += 1;
      if (current >= imageUrls.length) {
        current = 0;
      }
    }
    setCurrentImageIndex(current);
  };

  return (
    <div className={Style.image}>
      <img src={imageUrls[currentImageIndex]} />
      <div className={Style.slider}>
        <button
          className={Style.iconChevronBtn}
          onClick={() => handleChangeImage(currentImageIndex, -1)}
        >
          <IconChevronLeftLarge color="#777" />
        </button>
        {imageUrls.map((url, index) => (
          <img
            key={index}
            src={url}
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
