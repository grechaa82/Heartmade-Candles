import { FC } from 'react';

import Style from './ListProductsCartSkeleton.module.css';

const ListProductsCartSkeleton: FC = () => {
  const countProductsCartSkeleton = [0, 0, 0];

  const productsCartSkeleton = (
    <div className={Style.productCartSkeleton}>
      <div className={Style.imageSkeleton}></div>
      <div className={Style.quantityManagementSkeleton}>
        <div className={Style.iconBtnSkeleton}></div>
        <div className={Style.iconBtnSkeleton}></div>
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
