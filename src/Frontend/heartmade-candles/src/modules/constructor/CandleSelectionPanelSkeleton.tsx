import { FC } from 'react';

import Style from './CandleSelectionPanelSkeleton.module.css';

const CandleSelectionPanelSkeleton: FC = () => {
  const countProductsSkeleton = [0, 0, 0, 0, 0, 0];
  const countGridsSkeleton = [0, 0];

  const productsGridSelectorSkeleton = (
    <div className={Style.productGridSkeleton}>
      <div className={Style.titleSkeleton}></div>
      <div className={Style.gridSkeleton}>
        {countProductsSkeleton.map(() => (
          <div className={Style.productSkeleton}></div>
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
