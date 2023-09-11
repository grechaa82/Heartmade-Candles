import { FC } from 'react';

import Skeleton from '../../components/shared/skeleton';

import Style from './ListProductsCartSkeleton.module.css';

const ListProductsCartSkeleton: FC = () => {
  const countProductsCartSkeleton = [0, 0, 0];

  const productsCartSkeleton = (
    <>
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
    </>
  );

  return (
    <div className={Style.listProductsCartSkeleton}>
      {countProductsCartSkeleton.map((item, index) => {
        return (
          <div className={Style.productCartSkeleton} key={index}>
            {productsCartSkeleton}
          </div>
        );
      })}
    </div>
  );
};

export default ListProductsCartSkeleton;
