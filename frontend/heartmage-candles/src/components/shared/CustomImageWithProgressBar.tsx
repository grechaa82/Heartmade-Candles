import { FC } from 'react';

import CustomImage, { CustomImageProps } from './Image';

import Style from './CustomImageWithProgressBar.module.css';

interface CustomImageWithProgressBarProps extends CustomImageProps {
  showProgressBar: boolean;
  progressValue: number;
  progressMax: number;
}

const CustomImageWithProgressBar: FC<CustomImageWithProgressBarProps> = ({
  showProgressBar,
  progressValue,
  progressMax,
  ...props
}) => {
  return (
    <>
      <CustomImage {...props} />
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

export default CustomImageWithProgressBar;
