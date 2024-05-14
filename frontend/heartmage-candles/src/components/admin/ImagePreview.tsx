import { FC } from 'react';

import { Image } from '../../types/Image';
import Picture from '../shared/Picture';

import Style from './ImagePreview.module.css';

export interface ImagePreviewProps {
  images: Image[];
}

const ImagePreview: FC<ImagePreviewProps> = ({ images }) => {
  return (
    <div className={Style.imagePrevBlock}>
      {images.map((image, index) => (
        <div className={Style.imagePrev} key={index}>
          <Picture
            name={image.fileName}
            alt={image.alternativeName}
            className={Style.squareImage}
            sourceSettings={[
              {
                size: 'small',
              },
            ]}
          />
        </div>
      ))}
    </div>
  );
};

export default ImagePreview;
