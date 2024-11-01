import { FC, useState } from 'react';

import { apiUrlToImage } from '../../config';

import Style from './Picture.module.css';

// I have to pass "SourceSettings" because this issue has not been solved yet https://github.com/w3c/csswg-drafts/issues/5889

export interface SourceSettings {
  size: sizeType;
  media?: string;
}

export interface PictureProps {
  name: string;
  alt: string;
  url?: string;
  src?: string;
  withPreview?: boolean;
  withSize?: boolean;
  withWebP?: boolean;
  className?: string;
  sourceSettings: SourceSettings[];
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
  sourceSettings = [],
}) => {
  const [isLoaded, setIsLoaded] = useState(false);

  const newSrc = src
    ? src
    : url
    ? `${url}/${name}`
    : `${apiUrlToImage}/${name}/${name}`;

  const splitName = name.split('.');
  const fileName = splitName[0];
  const fileExtension = splitName[1];
  const pathToImage = url ? url : apiUrlToImage + '/' + fileName;

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
        !isLoaded && withPreview ? Style.blurImage : ''
      }`}
      style={
        withPreview && !isLoaded
          ? {
              backgroundImage: `url(${getSrc('preview', withWebP)})`,
            }
          : {}
      }
    >
      <picture>
        {withWebP &&
          sourceSettings.map((source, index) => (
            <source
              key={index}
              type="image/webp"
              srcSet={getSrc(source.size, withWebP)}
              media={source.media}
            />
          ))}
        {sourceSettings.map((source, index) => (
          <source
            key={index}
            type={`image/${fileExtension}`}
            srcSet={getSrc(source.size, false)}
            media={source.media}
          />
        ))}
        <img
          className={Style.image}
          src={getSrc()}
          alt={alt}
          decoding="async"
          loading="lazy"
          onLoad={() => setIsLoaded(true)}
        />
      </picture>
    </div>
  );
};

export default Picture;
