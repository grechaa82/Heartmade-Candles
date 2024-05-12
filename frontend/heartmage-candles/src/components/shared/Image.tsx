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

  const splitName = name.split('.');
  const fileName = splitName[0];
  const fileExtension = splitName[1];

  console.log(splitName);

  return (
    <div
      className={`${Style.image} ${className ? className : ''} ${
        Style.blurImage
      }`}
      style={{
        backgroundImage: `url(${apiUrlToImage}/${fileName}-preview.webp)`,
      }}
    >
      <img
        src={newSrc}
        alt={alt}
        style={style}
        // srcSet={`${apiUrlToImage}/${fileName}-preview.webp 20w, ${apiUrlToImage}/${fileName}-small.webp 200w, ${apiUrlToImage}/${fileName}-medium.webp 600w, ${apiUrlToImage}/${fileName}-large.webp 1200w`}
        loading="lazy"
      />
      {/* <img
        style={style}
        src={newSrc}
        alt={alt}
        decoding="async"
        // sizes="(max-width: 360px) 100vw, (max-width: 1920px) 64vw"
        sizes="100vw"
        srcSet={`${apiUrlToImage}/${fileName}-preview.webp 20w, ${apiUrlToImage}/${fileName}-small.webp 200w, ${apiUrlToImage}/${fileName}-medium.webp 600w, ${apiUrlToImage}/${fileName}-large.webp 1200w`}
        loading="lazy"
      /> */}
    </div>
  );
};

export default CustomImage;
