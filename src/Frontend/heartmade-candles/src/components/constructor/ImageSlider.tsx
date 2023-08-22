import { FC, useState } from 'react';

import { Image } from '../../types/Image';

import Style from './ImageSlider.module.css';

interface ImageSliderProps {
  images: Image[];
}

const ImageSlider: FC<ImageSliderProps> = ({ images }) => {
  const [currentImageIndex, setCurrentImageIndex] = useState(0);
  const urlToImage = 'http://localhost:5000/StaticFiles/Images/';

  return (
    <div className={Style.imageBlock}>
      <img
        className={Style.mainImage}
        src={urlToImage + images[currentImageIndex].fileName}
        alt={images[currentImageIndex].alternativeName}
      />
      <div className={Style.slider}>
        {images.map((image, index) => (
          <div className={Style.imageSliderBlock}>
            <button
              className={`${Style.sliderBtn} ${currentImageIndex === index ? Style.selected : ''}`}
              type="button"
              onClick={() => setCurrentImageIndex(index)}
            >
              <div className={Style.image}>
                <img key={index} src={urlToImage + image.fileName} />
              </div>
            </button>
          </div>
        ))}
      </div>
    </div>
  );
};

{
  /* 
<button
  className={`${Style.selectBtn} ${isSelected ? Style.selected : ''}`}
  type="button"
  onClick={() => handleSelectProduct()}
  >
    <div className={Style.image}>
      {firstImage && (
        <img src={urlToImage + firstImage.fileName} alt={firstImage.alternativeName} />
      )}
    </div>
</button> 
*/
}

export default ImageSlider;
