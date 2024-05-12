import { FC } from 'react';

import { apiUrlToImage } from '../../config';

import Style from './Picture.module.css';

export interface PictureProps {
  name: string;
  alt: string;
  url?: string;
  src?: string;
  withPreview?: boolean;
  withSize?: boolean;
  withWebP?: boolean;
  className?: string;
}

type sizeType = 'small' | 'medium' | 'large' | 'preview' | 'default';

const Picture: FC<PictureProps> = ({
  name,
  alt,
  url,
  src,
  withPreview = true,
  withSize = true,
  withWebP = true,
  className,
}) => {
  const newSrc = src
    ? src
    : url
    ? `${url}/${name}`
    : `${apiUrlToImage}/${name}`;

  const pathToImage = url ? url : apiUrlToImage;
  const splitName = name.split('.');
  const fileName = splitName[0];
  const fileExtension = splitName[1];

  const getSrc = (
    size: sizeType = 'default',
    useWebP: boolean = true,
  ): string => {
    return src
      ? src
      : `${pathToImage}/${fileName}${size === 'default' ? '' : `-${size}`}.${
          withWebP && useWebP ? 'webp' : fileExtension
        }`;
  };

  return (
    <div
      className={`${Style.picture} ${className ? className : ''} ${
        Style.blurImage
      }`}
      style={
        withPreview
          ? {
              backgroundImage: `url(${getSrc('preview', withWebP)})`,
            }
          : {}
      }
    >
      <picture>
        <source
          type="image/webp"
          srcSet={`${getSrc('small')} 200w`}
          media="(max-width: 200px)"
        />
        <source
          type="image/webp"
          srcSet={`${getSrc('medium')} 600w`}
          media="(max-width: 630px)"
        />
        <source
          type="image/webp"
          srcSet={`${getSrc('large')} 1200w`}
          media="(max-width: 768px)"
        />
        <source
          type="image/webp"
          srcSet={`${getSrc('medium')} 600w`}
          media="(max-width: 1896px)"
        />
        <source
          type="image/webp"
          srcSet={`${getSrc('large')} 1200w`}
          media="(min-width: 1897px)"
        />

        <source
          type={`image/${fileExtension}`}
          srcSet={`${getSrc('small', false)} 200w`}
          media="(max-width: 200px)"
        />
        <source
          type={`image/${fileExtension}`}
          srcSet={`${getSrc('medium', false)} 600w`}
          media="(max-width: 630px)"
        />
        <source
          type={`image/${fileExtension}`}
          srcSet={`${getSrc('large', false)} 1200w`}
          media="(max-width: 768px)"
        />
        <source
          type={`image/${fileExtension}`}
          srcSet={`${getSrc('medium', false)} 600w`}
          media="(max-width: 1896px)"
        />
        <source
          type={`image/${fileExtension}`}
          srcSet={`${getSrc('large', false)} 1200w`}
          media="(min-width: 1897px)"
        />
        <img
          className={Style.image}
          src={getSrc()}
          alt={alt}
          decoding="async"
          loading="lazy"
        />
      </picture>
    </div>
  );
};

export default Picture;
