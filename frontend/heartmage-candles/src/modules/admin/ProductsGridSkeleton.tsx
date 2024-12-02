import { FC } from 'react';

import Skeleton from '../../components/shared/skeleton';

import Style from './ProductsGridSkeleton.module.css';

const ProductsGridSkeleton: FC = ({}) => {
  const countProductsSkeleton = [0, 0, 0, 0, 0, 0];

  return (
    <div className={Style.productsGridSkeleton}>
      <div className={Style.titleSkeleton}>
        <Skeleton />
      </div>
      <div className={Style.gridSkeleton}>
        {countProductsSkeleton.map((_, index) => {
          return (
            <div className={Style.productSkeleton} key={index}>
              <Skeleton />
            </div>
          );
        })}
      </div>
    </div>
  );
};

export default ProductsGridSkeleton;
