import { FC, useState } from 'react';

import { apiUrlToImage } from '../../config';
import Picture, { SourceSettings } from '../../components/shared/Picture';

import Style from './TutorialBlock.module.css';

interface TutorialBlockProps {}

const TutorialBlock: FC<TutorialBlockProps> = () => {
  const [showVideo, setShowVideo] = useState(true);

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
      media: '(max-width: 1100px)',
    },
    {
      size: 'medium',
      media: '(max-width: 1401px)',
    },
    {
      size: 'large',
      media: '(min-width: 1401px)',
    },
  ];

  return (
    <div className={Style.tutorialBlock}>
      {showVideo ? (
        <video
          className={Style.video}
          src={`${apiUrlToImage}/constructor-tutorial-video.webm`}
          muted
          autoPlay
          controls
          onEnded={() => setShowVideo(false)}
        />
      ) : (
        <Picture
          name="constructor-banner.jpg"
          alt="Уютная комната со свечами"
          className={Style.squareImage}
          sourceSettings={sourceSettingsForMainImage}
        />
      )}
      <div className={Style.slider}>
        <div className={Style.imageSliderBlock}>
          <button
            className={`${Style.sliderBtn} ${showVideo ? Style.selected : ''}`}
            type="button"
            onClick={() => setShowVideo(true)}
          >
            <Picture
              name="constructor-tutorial-video-prev.png"
              alt="Пример создания свечи"
              sourceSettings={[
                {
                  size: 'small',
                },
              ]}
            />
          </button>
        </div>
        <div className={Style.imageSliderBlock}>
          <button
            className={`${Style.sliderBtn} ${!showVideo ? Style.selected : ''}`}
            type="button"
            onClick={() => setShowVideo(false)}
          >
            <Picture
              name="constructor-banner.jpg"
              alt="Уютная комната со свечами"
              sourceSettings={[
                {
                  size: 'small',
                },
              ]}
            />
          </button>
        </div>
      </div>
    </div>
  );
};

export default TutorialBlock;
