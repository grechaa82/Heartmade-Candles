import { FC, useEffect, useState } from 'react';

import { Image } from '../../types/Image';
import { apiUrlToImage } from '../../config';
import IconChevronUpLarge from '../../UI/IconChevronUpLarge';
import IconChevronDownLarge from '../../UI/IconChevronDownLarge';

import Style from './ImageSlider.module.css';

interface ImageSliderProps {
  images: Image[];
}

const ImageSlider: FC<ImageSliderProps> = ({ images }) => {
  const [currentImageIndex, setCurrentImageIndex] = useState(0);
  const [scrollTime, setScrollTime] = useState(15);
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
      <img
        className={Style.mainImage}
        src={`${apiUrlToImage}/${images[currentImageIndex].fileName}`}
        alt={images[currentImageIndex].alternativeName}
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
              <div className={Style.image}>
                <img src={`${apiUrlToImage}/${image.fileName}`} />
                {currentImageIndex === index && (
                  <progress
                    className={Style.progress}
                    value={timeLeft}
                    max={scrollTime}
                  ></progress>
                )}
              </div>
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
