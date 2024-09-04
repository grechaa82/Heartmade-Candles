import { FC, memo, useCallback, useEffect, useState } from 'react';

import { Image } from '../../types/Image';
import PictureWithProgressBar from '../shared/PictureWithProgressBar';
import Picture, { SourceSettings } from '../shared/Picture';

import Style from './ImageSlider.module.css';

interface ImageSliderProps {
  images: Image[];
}

const ImageSlider: FC<ImageSliderProps> = ({ images }) => {
  const [currentImageIndex, setCurrentImageIndex] = useState<number>(0);
  const [scrollTime] = useState<number>(10);
  const [timeLeft, setTimeLeft] = useState<number>(scrollTime);

  const handleChangeImage = useCallback(
    (index: number, moveToIndex?: number) => {
      let current = index;
      if (moveToIndex === -1) {
        current = (current - 1 + images.length) % images.length;
      } else if (moveToIndex === 1) {
        current = (current + 1) % images.length;
      }
      setCurrentImageIndex(current);
      setTimeLeft(scrollTime);
    },
    [images.length, scrollTime],
  );

  const handleSetCurrentImageIndex = useCallback(
    (index: number) => {
      setCurrentImageIndex(index);
      setTimeLeft(scrollTime);
    },
    [scrollTime],
  );

  useEffect(() => {
    const interval = setInterval(() => {
      if (timeLeft > 0) {
        setTimeLeft((prev) => prev - 1);
      } else {
        handleChangeImage(currentImageIndex, 1);
      }
    }, 1000);

    return () => clearInterval(interval);
  }, [timeLeft, currentImageIndex, handleChangeImage]);

  return (
    <div className={Style.imageBlock}>
      <MainImage image={images[currentImageIndex]} />
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
                sourceSettings={[{ size: 'small' }]}
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

const MainImage: FC<{ image: Image }> = memo(({ image }) => {
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
    <div className={Style.mainImage}>
      <Picture
        name={image.fileName}
        alt={image.alternativeName}
        className={Style.squareImage}
        sourceSettings={sourceSettingsForMainImage}
      />
    </div>
  );
});
