import { FC } from 'react';

import Picture, { PictureProps } from './Picture';

import Style from './PictureWithProgressBar.module.css';

interface PictureWithProgressBarProps extends PictureProps {
  showProgressBar: boolean;
  progressValue: number;
  progressMax: number;
}

const PictureWithProgressBar: FC<PictureWithProgressBarProps> = ({
  showProgressBar,
  progressValue,
  progressMax,
  ...props
}) => {
  return (
    <>
      <Picture {...props} />
      {showProgressBar && (
        <progress
          className={Style.progress}
          value={progressValue}
          max={progressMax}
        ></progress>
      )}
    </>
  );
};

export default PictureWithProgressBar;
