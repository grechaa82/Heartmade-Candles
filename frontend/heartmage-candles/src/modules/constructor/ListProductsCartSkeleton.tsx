import { FC } from 'react';

import Skeleton from '../../components/shared/Skeleton';
import Button from '../../components/shared/Button';

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
    <div className={Style.listProductsCart}>
      {countProductsCartSkeleton.map((_, index) => {
        return (
          <div className={Style.productCartSkeleton} key={index}>
            {productsCartSkeleton}
          </div>
        );
      })}
      <div className={Style.infoBlock}>
        <div className={Style.priceBlock}>
          <span className={Style.priceTitle}>Итого</span>
          <span className={Style.price}>
            <Skeleton />
          </span>
        </div>
        <div className={Style.createBtn}>
          <Button
            color="#6FCF97"
            text="Заказать"
            onClick={() => console.log()}
          />
        </div>
      </div>
    </div>
  );
};

export default ListProductsCartSkeleton;
