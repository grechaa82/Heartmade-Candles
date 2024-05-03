import { FC, useState } from 'react';

import ConstructorBanner2 from '../../assets/constructor-banner-2.png';
import VideoPrev from '../../assets/constructor-tutorial-video-prev.png';
import Video from '../../assets/constructor-tutorial-video.mp4';
import CustomImage from '../../components/shared/Image';

import Style from './TutorialBlock.module.css';

interface TutorialBlockProps {}

const TutorialBlock: FC<TutorialBlockProps> = () => {
  const [showVideo, setShowVideo] = useState(true);

  return (
    <div className={Style.tutorialBlock}>
      {showVideo ? (
        <video
          className={Style.video}
          src={Video}
          muted
          autoPlay
          controls
          onEnded={() => setShowVideo(false)}
        />
      ) : (
        <CustomImage
          name={ConstructorBanner2}
          alt={ConstructorBanner2}
          src={ConstructorBanner2}
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
            <CustomImage name={VideoPrev} alt={VideoPrev} src={VideoPrev} />
          </button>
        </div>
        <div className={Style.imageSliderBlock}>
          <button
            className={`${Style.sliderBtn} ${!showVideo ? Style.selected : ''}`}
            type="button"
            onClick={() => setShowVideo(false)}
          >
            <CustomImage
              name={ConstructorBanner2}
              alt={ConstructorBanner2}
              src={ConstructorBanner2}
            />
          </button>
        </div>
      </div>
    </div>
  );
};

export default TutorialBlock;
