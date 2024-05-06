import { FC } from 'react';

import { Image } from '../../types/Image';
import CustomImage from '../shared/Image';

import Style from './ImagePreview.module.css';

export interface ImagePreviewProps {
  images: Image[];
}

const ImagePreview: FC<ImagePreviewProps> = ({ images }) => {
  return (
    <div className={Style.imagePrevBlock}>
      {images.map((image, index) => (
        <div className={Style.imagePrev} key={index}>
          <CustomImage
            name={image.fileName}
            alt={image.alternativeName}
            className={Style.squareImage}
          />
        </div>
      ))}
    </div>
  );
};

export default ImagePreview;
