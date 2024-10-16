import { FC } from 'react';

import Skeleton from '../../components/shared/Skeleton';

import Style from './MainInfoSkeleton.module.css';

const MainInfoSkeleton: FC = () => {
  return (
    <div className={Style.contentSkeleton}>
      <div className={Style.imageSliderSkeleton}>
        <Skeleton />
      </div>
      <div className={Style.formSkeleton}>
        <div className={Style.itemTitleSkeleton}>
          <Skeleton />
        </div>
        <div className={Style.itemPriceSkeleton}>
          <Skeleton />
        </div>
        <div className={Style.itemActiveSkeleton}>
          <Skeleton />
        </div>
        <div className={Style.itemDescriptionSkeleton}>
          <Skeleton />
        </div>
      </div>
    </div>
  );
};

export default MainInfoSkeleton;
