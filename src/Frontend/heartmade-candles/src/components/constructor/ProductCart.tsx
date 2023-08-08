import { FC } from 'react';

import CornerTag from './CornerTag';
import { ImageProduct } from '../../typesV2/BaseProduct';
import IconPlusLarge from '../../UI/IconPlusLarge';
import IconMinusLarge from '../../UI/IconMinusLarge';

import Style from './ProductCart.module.css';

export interface ProductCartProps {
  product: ImageProduct;
  onChangingQuantityProduct: (newQuantity: number, key: number) => void;
  quantity: number;
  index: number;
}

const ProductCart: FC<ProductCartProps> = ({
  product,
  onChangingQuantityProduct,
  quantity,
  index,
}) => {
  const urlToImage = 'http://localhost:5000/StaticFiles/Images/';
  const firstImage = product.images && product.images.length > 0 ? product.images[0] : null;

  const onIncreasesQuantityProduct = () => {
    onChangingQuantityProduct(quantity + 1, index);
  };
  const onDecreasesQuantityProduct = () => {
    onChangingQuantityProduct(quantity - 1, index);
  };

  return (
    <div className={Style.productCart}>
      <div className={Style.imageBlock}>
        <div className={Style.image}>
          {firstImage && (
            <img src={urlToImage + firstImage.fileName} alt={firstImage.alternativeName} />
          )}
        </div>
        <div className={Style.quantity}>
          <CornerTag number={quantity} type="coutner" />
        </div>
      </div>
      <div className={Style.quantityManagement}>
        <button className={Style.iconBtn} onClick={() => onIncreasesQuantityProduct()}>
          <IconPlusLarge color="#aaa" />
        </button>
        <button className={Style.iconBtn} onClick={() => onDecreasesQuantityProduct()}>
          <IconMinusLarge color="#aaa" />
        </button>
      </div>
    </div>
  );
};

export default ProductCart;
