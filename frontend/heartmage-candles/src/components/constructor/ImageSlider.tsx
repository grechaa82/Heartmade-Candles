import { FC, useEffect, useState } from 'react';

import { Image } from '../../types/Image';
import IconChevronUpLarge from '../../UI/IconChevronUpLarge';
import IconChevronDownLarge from '../../UI/IconChevronDownLarge';
import PictureWithProgressBar from '../shared/PictureWithProgressBar';
import Picture, { SourceSettings } from '../shared/Picture';

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

  const handleSetCurrentImageIndex = (index: number) => {
    setTimeLeft(scrollTime);
    setCurrentImageIndex(index);
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

  const sourceSettingsForMainImage: SourceSettings[] = [
    {
      size: 'small',
      media: '(max-width: 200px)',
    },
    {
      size: 'medium',
      media: '(max-width: 630px)',
    },
    {
      size: 'large',
      media: '(max-width: 1200px)',
    },
    {
      size: 'medium',
      media: '(max-width: 1690px)',
    },
    {
      size: 'large',
      media: '(min-width: 1691px)',
    },
  ];

  return (
    <div className={Style.imageBlock}>
      <div className={Style.mainImage}>
        <Picture
          name={images[currentImageIndex].fileName}
          alt={images[currentImageIndex].alternativeName}
          className={Style.squareImage}
          sourceSettings={sourceSettingsForMainImage}
        />
      </div>
      <div className={Style.slider}>
        {images.map((image, index) => (
          <div className={Style.imageSliderBlock} key={index}>
            <button
              className={`${Style.sliderBtn} ${
                currentImageIndex === index ? Style.selected : ''
              }`}
              type="button"
              onClick={() => handleSetCurrentImageIndex(index)}
            >
              <PictureWithProgressBar
                name={image.fileName}
                alt={image.alternativeName}
                sourceSettings={[
                  {
                    size: 'small',
                  },
                ]}
                showProgressBar={currentImageIndex === index}
                progressValue={timeLeft}
                progressMax={scrollTime}
              />
            </button>
          </div>
        ))}
      </div>
    </div>
  );
};

export default ImageSlider;
