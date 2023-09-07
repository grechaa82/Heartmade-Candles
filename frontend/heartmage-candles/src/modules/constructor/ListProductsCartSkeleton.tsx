import { FC } from 'react';

import Skeleton from '../../components/shared/skeleton';

import Style from './ListProductsCartSkeleton.module.css';

const ListProductsCartSkeleton: FC = () => {
  const countProductsCartSkeleton = [0, 0, 0];

  const productsCartSkeleton = (
    <div className={Style.productCartSkeleton}>
      <div className={Style.imageSkeleton}>
        <Skeleton />
      </div>
      <div className={Style.quantityManagementSkeleton}>
        <div className={Style.iconBtnSkeleton}>
          <Skeleton />
        </div>
        <div className={Style.iconBtnSkeleton}>
          <Skeleton />
        </div>
      </div>
    </div>
  );

  return (
    <div className={Style.listProductsCartSkeleton}>
      {countProductsCartSkeleton.map((productsCart, index) => {
        return productsCartSkeleton;
      })}
    </div>
  );
};

export default ListProductsCartSkeleton;
