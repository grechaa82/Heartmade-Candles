import { FC } from 'react';

import Skeleton from '../../components/shared/skeleton';

import Style from './CandleSelectionPanelSkeleton.module.css';

const CandleSelectionPanelSkeleton: FC = () => {
  const countProductsSkeleton = [0, 0, 0, 0, 0, 0];
  const countGridsSkeleton = [0, 0];

  const productsGridSelectorSkeleton = (
    <div className={Style.productGridSkeleton}>
      <div className={Style.titleSkeleton}>
        <Skeleton />
      </div>
      <div className={Style.gridSkeleton}>
        {countProductsSkeleton.map(() => (
          <div className={Style.gridItem}>
            <Skeleton />
          </div>
        ))}
      </div>
    </div>
  );

  return (
    <div className={Style.contentSkeleton}>
      {countGridsSkeleton.map(() => {
        return productsGridSelectorSkeleton;
      })}
    </div>
  );
};

export default CandleSelectionPanelSkeleton;
