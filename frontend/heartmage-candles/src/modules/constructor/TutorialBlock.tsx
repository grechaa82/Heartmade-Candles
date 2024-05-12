import { FC, useState } from 'react';

import CustomImage from '../../components/shared/Image';
import { apiUrlToImage } from '../../config';

import Style from './TutorialBlock.module.css';

interface TutorialBlockProps {}

const TutorialBlock: FC<TutorialBlockProps> = () => {
  const [showVideo, setShowVideo] = useState(true);

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
        <CustomImage
          name="constructor-banner.jpg"
          alt="Уютная комната со свечами"
          className={Style.squareImage}
        />
      )}
      <div className={Style.slider}>
        <div className={Style.imageSliderBlock}>
          <button
            className={`${Style.sliderBtn} ${showVideo ? Style.selected : ''}`}
            type="button"
            onClick={() => setShowVideo(true)}
          >
            <CustomImage
              name="constructor-tutorial-video-prev.png"
              alt="Пример создания свечи"
            />
          </button>
        </div>
        <div className={Style.imageSliderBlock}>
          <button
            className={`${Style.sliderBtn} ${!showVideo ? Style.selected : ''}`}
            type="button"
            onClick={() => setShowVideo(false)}
          >
            <CustomImage
              name="constructor-banner.jpg"
              alt="Уютная комната со свечами"
            />
          </button>
        </div>
      </div>
    </div>
  );
};

export default TutorialBlock;
