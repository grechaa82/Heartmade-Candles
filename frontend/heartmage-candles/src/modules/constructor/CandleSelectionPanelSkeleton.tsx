import { FC } from 'react';

import Skeleton from '../../components/shared/skeleton';

import Style from './CandleSelectionPanelSkeleton.module.css';

const CandleSelectionPanelSkeleton: FC = () => {
  const countProductsSkeleton = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
  const countGridsSkeleton = [0, 0];

  const productsGridSelectorSkeleton = (
    <>
      <div className={Style.titleSkeleton}>
        <Skeleton />
      </div>
      <div className={Style.gridSkeleton}>
        {countProductsSkeleton.map((item, index) => (
          <div className={Style.productSkeleton}>
            <div className={Style.imageSkeleton} key={index}>
              <Skeleton />
            </div>
            <div className={Style.infoBlockSkeleton}>
              <Skeleton />
              <Skeleton />
            </div>
          </div>
        ))}
      </div>
      <div className={Style.btnSkeleton}>
        <Skeleton />
      </div>
    </>
  );

  return (
    <div className={Style.contentSkeleton}>
      {countGridsSkeleton.map((item, index) => (
        <div className={Style.productGridSkeleton} key={index}>
          {productsGridSelectorSkeleton}
        </div>
      ))}
    </div>
  );
};

export default CandleSelectionPanelSkeleton;
