import { FC } from 'react';

import CornerTag from './CornerTag';
import { LayerColor } from '../../types/LayerColor';
import { Decor } from '../../types/Decor';
import { Candle } from '../../types/Candle';
import { Wick } from '../../types/Wick';
import { Smell } from '../../types/Smell';

import Style from './Product.module.css';

type ProductType = Candle | Decor | LayerColor | Smell | Wick;

export interface ProductProps {
  product: ProductType;
}

const Product: FC<ProductProps> = ({ product }) => {
  const urlToImage = 'http://localhost:5000/StaticFiles/Images/';
  const firstImage = product.images && product.images.length > 0 ? product.images[0] : null;
  let price = 0;

  if ('price' in product) {
    price = product.price;
  } else if ('pricePerGram' in product) {
    price = product.pricePerGram;
  }

  return (
    <div className={Style.product}>
      <div className={Style.image}>
        {firstImage && (
          <img src={urlToImage + firstImage.fileName} alt={firstImage.alternativeName} />
        )}
      </div>
      <div className={Style.price}>{price !== 0 && <CornerTag number={price} type="price" />}</div>
    </div>
  );
};

export default Product;
