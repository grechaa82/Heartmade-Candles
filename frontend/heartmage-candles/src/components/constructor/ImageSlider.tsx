import { FC, useState } from 'react';

import { Image } from '../../types/Image';
import { apiUrlToImage } from '../../config';

import Style from './ImageSlider.module.css';

interface ImageSliderProps {
  images: Image[];
}

const ImageSlider: FC<ImageSliderProps> = ({ images }) => {
  const [currentImageIndex, setCurrentImageIndex] = useState(0);

  return (
    <div className={Style.imageBlock}>
      <img
        className={Style.mainImage}
        src={`${apiUrlToImage}/${images[currentImageIndex].fileName}`}
        alt={images[currentImageIndex].alternativeName}
      />
      <div className={Style.slider}>
        {images.map((image, index) => (
          <div className={Style.imageSliderBlock} key={index}>
            <button
              className={`${Style.sliderBtn} ${currentImageIndex === index ? Style.selected : ''}`}
              type="button"
              onClick={() => setCurrentImageIndex(index)}
            >
              <div className={Style.image}>
                <img src={`${apiUrlToImage}/${image.fileName}`} />
              </div>
            </button>
          </div>
        ))}
      </div>
    </div>
  );
};

export default ImageSlider;
