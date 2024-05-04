import { FC } from 'react';

import { apiUrlToImage } from '../../config';

import Style from './Image.module.css';

export interface CustomImageProps {
  name: string;
  alt: string;
  url?: string;
  width?: number;
  height?: number;
  className?: string;
  src?: string;
}

const CustomImage: FC<CustomImageProps> = ({
  name,
  alt,
  url,
  width,
  height,
  className,
  src,
}) => {
  const newSrc = src
    ? src
    : url
    ? `${url}/${name}`
    : `${apiUrlToImage}/${name}`;
  const style = {
    width: width ? `${width}px` : '100%',
    height: height ? `${height}px` : '100%',
  };

  return (
    <div className={`${Style.image} ${className ? className : ''}`}>
      <img src={newSrc} alt={alt} style={style} loading="lazy" />
    </div>
  );
};

export default CustomImage;
