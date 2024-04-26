import { FC, useEffect, useState } from 'react';

import { Image } from '../../types/Image';
import IconChevronUpLarge from '../../UI/IconChevronUpLarge';
import IconChevronDownLarge from '../../UI/IconChevronDownLarge';
import CustomImage from '../shared/Image';
import CustomImageWithProgressBar from '../shared/CustomImageWithProgressBar';

import Style from './ImageSlider.module.css';

interface ImageSliderProps {
  images: Image[];
}

const ImageSlider: FC<ImageSliderProps> = ({ images }) => {
  const [currentImageIndex, setCurrentImageIndex] = useState(0);
  const [scrollTime, setScrollTime] = useState(10);
  const [timeLeft, setTimeLeft] = useState(scrollTime);

  const handleChangeImage = (index: number, moveToIndex?: number) => {
    let current = index;
    if (moveToIndex && moveToIndex === -1) {
      current -= 1;
      if (current < 0) {
        current = images.length - 1;
      }
    } else if (moveToIndex && moveToIndex === 1) {
      current += 1;
      if (current >= images.length) {
        current = 0;
      }
    }
    setCurrentImageIndex(current);
    setTimeLeft(scrollTime);
  };

  useEffect(() => {
    const interval = setInterval(() => {
      if (timeLeft > 0) {
        setTimeLeft(timeLeft - 1);
      } else {
        handleChangeImage(currentImageIndex, 1);
      }
    }, scrollTime * 100);

    return () => clearInterval(interval);
  }, [timeLeft, currentImageIndex]);

  return (
    <div className={Style.imageBlock}>
      <CustomImage
        name={images[currentImageIndex].fileName}
        alt={images[currentImageIndex].alternativeName}
        className={Style.squareImage}
      />
      <div className={Style.slider}>
        <button
          className={Style.iconChevronBtn}
          onClick={() => handleChangeImage(currentImageIndex, -1)}
          type="button"
        >
          <IconChevronUpLarge color="#777" />
        </button>
        {images.map((image, index) => (
          <div className={Style.imageSliderBlock} key={index}>
            <button
              className={`${Style.sliderBtn} ${
                currentImageIndex === index ? Style.selected : ''
              }`}
              type="button"
              onClick={() => setCurrentImageIndex(index)}
            >
              <CustomImageWithProgressBar
                name={image.fileName}
                alt={image.alternativeName}
                showProgressBar={currentImageIndex === index}
                progressValue={timeLeft}
                progressMax={scrollTime}
              />
            </button>
          </div>
        ))}
        <button
          className={Style.iconChevronBtn}
          onClick={() => handleChangeImage(currentImageIndex, 1)}
          type="button"
        >
          <IconChevronDownLarge color="#777" />
        </button>
      </div>
    </div>
  );
};

export default ImageSlider;
